using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    Vector3 wheelPosition;
    Quaternion wheelRotation;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

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


    //DragForce
    private float targetDrag = 0.4f;
    private float initialDrag = 0.01f;
    private float transitionTime = 10f;
    private int intervals = 20;
    private float intervalTime;
    private float dragStep;
    private float currentDrag;
    private float transitionProgress = 0f;

    //InputVerticalZero
    float currentSpeed;

    void Awake()
    {
        camVariables.drvCamGameObj = GameObject.Find("Drive Cam");
        camVariables.mainCamObj = GameObject.Find("MainCamera");
        characterCs = GameObject.Find("PlayerCapsule");
        camVariables.interactirSource =camVariables.mainCamObj.GetComponent<Camera>() ;//Camera.main

        carObj = GetComponent<Car>();
        rb = GetComponent<Rigidbody>();
     
    
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
        maxSpeed=carObj.carObject.maxSpeed;  
        motorForce= carObj.carObject.torque * 1000;
        

        SupensionCase();
        intervalTime = transitionTime / intervals;
        dragStep = (targetDrag - initialDrag) / intervals;
        currentDrag = rb.drag;
    }
    public void GetInTheCar()
    {
        if (carObj.IsActive == true && mySeller.SoldCarList.Contains(gameObject))
        {

            camVariables.driveCam.SetParent(carObj.transform);
            camVariables.followPoint.SetParent(carObj.transform);
            camVariables.driveCam.gameObject.SetActive(true);
            characterCs.gameObject.SetActive(false);
            characterCs.transform.SetParent(carObj.transform);

            Vector3 carPos = new Vector3(carObj.transform.position.x, carObj.transform.position.y+1f, carObj.transform.position.z);
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

    private void FixedUpdate()
    {
        GetInput();
        VerticalInputZero();
        DragForce();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        MaxSpeed();
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void VerticalInputZero()
    {
        if (verticalInput == 0)
        {
            // Araban�n h�z�n� yava��a azaltmak i�in Lerp kullan�yoruz
            float decelerationRate = 0.2f; // Yava�lama h�z�
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, decelerationRate * Time.deltaTime);
        }
        else
        {
            // Input oldu�u s�rece mevcut h�z� sakl�yoruz
            currentSpeed = rb.velocity.magnitude;
        }
    }
    private void DragForce()
    {
        if (verticalInput == 0)
        {
            if (currentDrag < targetDrag)
            {
                transitionProgress += Time.deltaTime;

                if (transitionProgress >= intervalTime)
                {
                    currentDrag += dragStep;
                    transitionProgress = 0f;
                }

                rb.drag = Mathf.Min(currentDrag, targetDrag);
            }
        }
        else
        {
            currentDrag = initialDrag;
            rb.drag = currentDrag;
            transitionProgress = 0f;
        }
    }
    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform, -camberAngle);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform, camberAngle);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform, -camberAngle);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform, camberAngle);

    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform,float cmbrAbngle)
    {
        if (cmbrAbngle>=0)
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
    void SetSuspensionSpringTargetPosition(WheelCollider leftWheelCollider, WheelCollider rightWheelCollider, float targetPosition)
    {
        JointSpring suspensionSpring = leftWheelCollider.suspensionSpring;

        suspensionSpring.targetPosition = Mathf.Clamp01(targetPosition);

        leftWheelCollider.suspensionSpring = suspensionSpring;
        rightWheelCollider.suspensionSpring = suspensionSpring;
    }
    private void MaxSpeed()
    {
        speed = rb.velocity.magnitude;
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }


}
