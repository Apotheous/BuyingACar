using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Interactor;
using static System.TimeZoneInfo;

public class CarController : MonoBehaviour,ISelectionCar
{
    public MrSellerManager mySeller;
    #region Veriables
    Rigidbody rb;
    private Car carObj;
    public GameObject carCanvas;
    [SerializeField]private float horizontalInput, verticalInput;
    private float currentSteerAngle;

    Vector3 wheelPosition;
    Quaternion wheelRotation;

    // Settings
    [SerializeField] private float motorForce, handBreakForce,breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    public float maxSpeed;
    public float speed;
    public float frontWheels, rearWheels;
    public float camberAngle;


    //Drive
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

    #endregion

    //InputVerticalZero
    float currentSpeed;


    //
    public float stabilizerForce;
    public float downforceValue;

    //CrashControl
    public float crashFactor;


    public float brakeInput;


    void Awake()
    {
        camVariables.drvCamGameObj = GameObject.Find("Drive Cam");
        camVariables.mainCamObj = GameObject.Find("MainCamera");
        characterCs = GameObject.Find("PlayerCapsule");
        camVariables.interactirSource =camVariables.mainCamObj.GetComponent<Camera>() ;//Camera.main

        carObj = GetComponent<Car>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);

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
        maxSpeed=carObj.carObject.maxSpeed*100;  
        motorForce= carObj.carObject.torque * 1000;
        SupensionCase();
    }

    private void FixedUpdate()
    {
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
        switch (carObj.carObject.Suspensions)
        {
            case 3:
                frontWheels = 0f;
                rearWheels = 0f;
                break;
            case 2:
                frontWheels = 1f;
                rearWheels = 1f;
                break;
            case 1:
                frontWheels = Random.Range(0f, 1f);
                rearWheels = Random.Range(0f, 1f);
                break;
            case 0:
                frontWheels = 0.5f;
                rearWheels = 0.5f;
                break;
        }
        SetSuspensionSpringTargetPosition(frontLeftWheelCollider, frontRightWheelCollider, frontWheels);
        SetSuspensionSpringTargetPosition(rearLeftWheelCollider, rearRightWheelCollider, rearWheels);
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
        speed = rb.velocity.magnitude;
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }
    private void DragForce()
    {
        if (verticalInput == 0)
        {
            // Arabanýn hýzýný yavaþça azaltmak için Lerp kullanýyoruz
            float decelerationRate = 0.5f; // Yavaþlama hýzý
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, decelerationRate * Time.deltaTime);

            // Dragý her saniye 0.01 artýran metodu çaðýrýyoruz
            IncreaseDragOverTime(0.02f);
        }
        else
        {
            // Input olduðu sürece mevcut hýzý saklýyoruz
            currentSpeed = rb.velocity.magnitude;
            rb.drag = 0;
        }

        // Dragý artýran metod
        void IncreaseDragOverTime(float increment)
        {
            rb.drag += increment * Time.deltaTime;
        }
    }
    #endregion

    #region Inputs and Informations
    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        //fixed code to brake even after going on reverse by Andrew Alex 
        float movingDirection = Vector3.Dot(transform.forward, rb.velocity);
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
        if (carObj.IsActive == true && mySeller.SoldCarList.Contains(gameObject))
        {

            camVariables.driveCam.SetParent(carObj.transform);
            camVariables.followPoint.SetParent(carObj.transform);
            camVariables.driveCam.gameObject.SetActive(true);
            characterCs.gameObject.SetActive(false);
            characterCs.transform.SetParent(carObj.transform);

            Vector3 carPos = new Vector3(carObj.transform.position.x, carObj.transform.position.y + 1f, carObj.transform.position.z);
            carObj.GetComponent<Rigidbody>().isKinematic = false;
            camVariables.followPoint.position = carPos;
            camVariables.followPoint.rotation = carObj.transform.rotation;

            carObj.transform.gameObject.GetComponent<CarController>().enabled = true;
            camVariables.interactirSource.GetComponent<Interactor>().inCar = true;
            camVariables.interactirSource.GetComponent<Interactor>().theCarImin = carObj.transform;
            carCanvas.SetActive(false);
        }
    }

    public void GetOutOfTheCar()
    {
        camVariables.interactirSource.transform.position = camVariables.driveCam.position;
        camVariables.driveCam.SetParent(null);
        camVariables.driveCam.gameObject.SetActive(false);
        characterCs.transform.SetParent(null);
        characterCs.gameObject.SetActive(true);
        carObj.GetComponent<Rigidbody>().isKinematic = true;
        carObj.transform.gameObject.GetComponent<CarController>().enabled = false;
        camVariables.interactirSource.GetComponent<Interactor>().inCar = false;
        carCanvas.SetActive(true);
    }

    #endregion

    #region Functions that enable the car to move
    private void HandleMotor()
    {
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;
    }
    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void ApplyHandBreaking()
    {
        frontRightWheelCollider.brakeTorque = brakeInput * handBreakForce * 0.7f;
        frontLeftWheelCollider.brakeTorque = brakeInput * handBreakForce * 0.7f;

        rearLeftWheelCollider.brakeTorque = brakeInput * handBreakForce * 0.3f;
        rearRightWheelCollider.brakeTorque = brakeInput * handBreakForce * 0.3f;
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
        wheelTransform.transform.GetChild(0).transform.Rotate(wheelCollider.rpm * 0.01f * Time.deltaTime, 0, 0, Space.Self);
        wheelTransform.transform.position = wheelPosition;
    }
    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform, -camberAngle);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform, camberAngle);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform, -camberAngle);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform, camberAngle);
    }
    #endregion

}
