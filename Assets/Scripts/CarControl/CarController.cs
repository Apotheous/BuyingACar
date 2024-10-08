using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static CarController;
using static Interactor;
using static System.TimeZoneInfo;

public class CarController : MonoBehaviour,ISelectionCar
{

    #region Veriables
    public MrSellerManager mySeller;
    [System.Serializable]
    public class CarObj
    {
        public Rigidbody rb;
        public Car car;

        [Header("Wheel Colliders")]
        public WheelCollider f_L_Wheel_Coll;
        public WheelCollider f_R_Wheel_Coll;
        public WheelCollider r_L_Wheel_Coll;
        public WheelCollider r_R_Wheel_Coll;

        [Header("Wheel Transforms")]
        public Transform f_L_Wh_Transform;
        public Transform f_R_Wh_Transform;
        public Transform r_L_Wh_Transform;
        public Transform r_R_Wh_Transform;

        [Header("Car Canvas Objects")]
        public GameObject carCanvas;
    }
    public CarObj carObj;

    [System.Serializable]
    public class CamVariables
    {
        public GameObject drvCamGameObj;
        public Transform mainTransform;
        public Camera interactirSource;
        public Transform driveCam;
        public Transform followPoint;
        public GameObject mainCamObj;
    }

    public CamVariables camVariables;

    public GameObject characterCs;

    [Header("Inputs")]
    [SerializeField]private float horizontalInput;
    [SerializeField]private float verticalInput;

    [Header("Motor Paramatreis")]
    [SerializeField] private float motorForce;
    [SerializeField] private float handBreakForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    public float maxSpeed;
    public float speed;
    public float frontWheels, rearWheels;
    public float camberAngle;
    public float brakeInput;

    float currentSpeed;
    float currentSteerAngle;

    Vector3 wheelPosition;
    Quaternion wheelRotation;

    #endregion

    private float speedClamped;
    public int isEngineRunning;
    void Awake()
    {
        camVariables.drvCamGameObj = GameObject.Find("Drive Cam");
        camVariables.mainCamObj = GameObject.Find("MainCamera");
        characterCs = GameObject.Find("PlayerCapsule");
        camVariables.interactirSource =camVariables.mainCamObj.GetComponent<Camera>();

        carObj.car = GetComponent<Car>();
        carObj.rb = GetComponent<Rigidbody>();
        carObj.rb.centerOfMass = new Vector3(0, -0.5f, 0);

        foreach (Transform t in camVariables.drvCamGameObj.transform)
        {
            if (t.GetComponent<CinemachineVirtualCamera>())
            {
                camVariables.driveCam = t;
            }
            else if (t.name == "Follow Point")
            {
                camVariables.followPoint = t;
            }
        }

    }
    private void Start()
    {
        mySeller.GetComponent<MrSellerManager>();
        maxSpeed= carObj.car.carObject.maxSpeed * 10*3.6f;  
        motorForce= carObj.car.carObject.torque * 1000;
        SupensionCase();
        StartCoroutine(CarControllerStopper());
    }
    private IEnumerator CarControllerStopper()
    {
        yield return new WaitForSeconds(0.5f);

        CarCntStopper();
    }

    private void CarCntStopper()
    {
        gameObject.GetComponent<CarController>().enabled = false;
    }

    private void Update()
    {
        speed = carObj.f_L_Wheel_Coll.rpm * carObj.f_R_Wheel_Coll.radius * 2f * Mathf.PI / 10f;
        speedClamped = Mathf.Lerp(speedClamped, speed, Time.deltaTime);
        GetInput();
        DragForce();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        MaxSpeed();
        ApplyHandBreaking();
    }

    #region Propeties Assignment
    void SupensionCase()
    {
        switch (carObj.car.carObject.Suspensions)
        {
            case 3:
                frontWheels = 0f;
                rearWheels = 0f;
                break;
            case 2:
                frontWheels = 0.33f;
                rearWheels = 0.33f;
                break;
            case 1:
                frontWheels = Random.Range(0.33f, 0.75f);
                rearWheels = Random.Range(0.33f, 0.75f);
                break;
            case 0:
                frontWheels = 0.5f;
                rearWheels = 0.5f;
                break;
        }
        SetSuspensionSpringTargetPosition(carObj.f_L_Wheel_Coll, carObj.f_R_Wheel_Coll, frontWheels);
        SetSuspensionSpringTargetPosition(carObj.r_L_Wheel_Coll, carObj.r_R_Wheel_Coll, rearWheels);
    }

    void SetSuspensionSpringTargetPosition(WheelCollider leftWheelCollider, WheelCollider rightWheelCollider, float targetPosition)
    {
        JointSpring suspensionSpring = leftWheelCollider.suspensionSpring;

        suspensionSpring.targetPosition = Mathf.Clamp01(targetPosition);

        leftWheelCollider.suspensionSpring = suspensionSpring;
        rightWheelCollider.suspensionSpring = suspensionSpring;
    }

    #endregion

    #region Autonomous Controls
    private void MaxSpeed()
    {
        if (carObj.rb.velocity.magnitude > maxSpeed)
        {
            carObj.rb.velocity = Vector3.ClampMagnitude(carObj.rb.velocity, maxSpeed);
        }
    }
    private void DragForce()
    {
        if (verticalInput == 0)
        {
            float decelerationRate = 0.5f;
            carObj.rb.velocity = Vector3.Lerp(carObj.rb.velocity, Vector3.zero, decelerationRate * Time.deltaTime);
            IncreaseDragOverTime(0.02f);
        }
        else
        {
            currentSpeed = carObj.rb.velocity.magnitude;
            carObj.rb.drag = 0;
        }
        void IncreaseDragOverTime(float increment)
        {
            carObj.rb.drag += increment * Time.deltaTime;
        }
    }
    #endregion

    #region Inputs and Informations
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        verticalInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(verticalInput) > 0 && isEngineRunning == 0)
        {
            StartCoroutine(GetComponent<EngineAudio>().StartEngine());
        }
        float movingDirection = Vector3.Dot(transform.forward, carObj.rb.velocity);
        if (movingDirection < -0.5f && verticalInput > 0)
        {
            brakeInput = Mathf.Abs(verticalInput);
        }
        else if (movingDirection > 0.5f && verticalInput < 0)
        {
            brakeInput = Mathf.Abs(verticalInput);
        }
        else
        {
            brakeInput = 0;
        }
    }

    #endregion

    #region Procedures performed when getting in and out of a car

    public void GetInTheCar()
    {
        if (carObj.car.IsActive == true && mySeller.SoldCarList.Contains(gameObject))
        {

            camVariables.driveCam.SetParent(carObj.car.transform);
            camVariables.followPoint.SetParent(carObj.car.transform);
            camVariables.driveCam.gameObject.SetActive(true);
            characterCs.gameObject.SetActive(false);
            characterCs.transform.SetParent(carObj.car.transform);

            Vector3 carPos = new Vector3(carObj.car.transform.position.x, carObj.car.transform.position.y + 1f, carObj.car.transform.position.z);
            carObj.car.GetComponent<Rigidbody>().isKinematic = false;
            camVariables.followPoint.position = carPos;
            camVariables.followPoint.rotation = carObj.car.transform.rotation;

            carObj.car.transform.gameObject.GetComponent<CarController>().enabled = true;
            camVariables.interactirSource.GetComponent<Interactor>().inCar = true;
            camVariables.interactirSource.GetComponent<Interactor>().theCarImin = carObj.car.transform;
            carObj.carCanvas.SetActive(false);
            gameObject.GetComponent<EngineAudio>().isEngineRunning = true;
            gameObject.GetComponent<EngineAudio>().startingSound.Play();

        }
    }

    public void GetOutOfTheCar()
    {
        camVariables.interactirSource.transform.position = camVariables.driveCam.position;
        camVariables.driveCam.SetParent(null);
        camVariables.driveCam.gameObject.SetActive(false);
        characterCs.transform.SetParent(null);
        characterCs.gameObject.SetActive(true);
        carObj.car.GetComponent<Rigidbody>().isKinematic = true;
        carObj.car.transform.gameObject.GetComponent<CarController>().enabled = false;
        camVariables.interactirSource.GetComponent<Interactor>().inCar = false;
        carObj.carCanvas.SetActive(true);
        gameObject.GetComponent<EngineAudio>().isEngineRunning=false;
    }

    #endregion

    #region Functions that enable the car to move
    private void HandleMotor()
    {
        if (isEngineRunning > 1)
        {
            if (Mathf.Abs(speed) < maxSpeed)
            {
                carObj.f_R_Wheel_Coll.motorTorque = verticalInput * motorForce;
                carObj.f_L_Wheel_Coll.motorTorque = verticalInput * motorForce;
            }
            else
            {
                carObj.f_R_Wheel_Coll.motorTorque = 0;
                carObj.f_L_Wheel_Coll.motorTorque = 0;
            }
        }

    }
    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        carObj.f_L_Wheel_Coll.steerAngle = currentSteerAngle;
        carObj.f_R_Wheel_Coll.steerAngle = currentSteerAngle;
    }

    private void ApplyHandBreaking()
    {
        carObj.f_R_Wheel_Coll.brakeTorque = brakeInput * handBreakForce * 0.7f;
        carObj.f_L_Wheel_Coll.brakeTorque = brakeInput * handBreakForce * 0.7f;

        carObj.r_L_Wheel_Coll.brakeTorque = brakeInput * handBreakForce * 0.3f;
        carObj.r_R_Wheel_Coll.brakeTorque = brakeInput * handBreakForce * 0.3f;
    }

    #endregion

    #region Turn the wheels (in accordance with the camber value)
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform, float cmbrAbngle)
    {
        if (cmbrAbngle >= 0)
        {
            camberAngle = camberAngle;
        }
        else
        {
            camberAngle = -camberAngle;
        }

        wheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);
        wheelTransform.transform.localRotation = Quaternion.Euler(0, wheelCollider.steerAngle, camberAngle);
        wheelTransform.transform.GetChild(0).transform.Rotate(wheelCollider.rpm * 6.6f * Time.deltaTime, 0, 0, Space.Self);
        wheelTransform.transform.position = wheelPosition;
    }
    private void UpdateWheels()
    {
        UpdateSingleWheel(carObj.f_L_Wheel_Coll, carObj.f_L_Wh_Transform, -camberAngle);
        UpdateSingleWheel(carObj.f_R_Wheel_Coll, carObj.f_R_Wh_Transform, camberAngle);
        UpdateSingleWheel(carObj.r_L_Wheel_Coll, carObj.r_L_Wh_Transform, -camberAngle);
        UpdateSingleWheel(carObj.r_R_Wheel_Coll, carObj.r_R_Wh_Transform, camberAngle);
    }
    #endregion

    #region About The Sound
    public float GetSpeedRaito()
    {
        var gas = Mathf.Clamp(Mathf.Abs(verticalInput), 0.5f, 1f);
        return speedClamped * gas / maxSpeed;
    }
    #endregion

}
