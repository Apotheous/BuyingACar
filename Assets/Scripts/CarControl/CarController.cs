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

    public float dotProduct;
    //
    public float stabilizerForce;
    public float downforceValue;


    /// <summary>
    /// ForInformation
    /// </summary>
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeedII = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    public float forwardSpeed;
    public float speedFactor;
    public float currentMotorTorque;
    public float currentSteerRange;
    public bool isAccelerating;
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
        intervalTime = transitionTime / intervals;
        dragStep = (targetDrag - initialDrag) / intervals;
        currentDrag = rb.drag;

    }

    private void ForInformation()
    {
        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        // Arabanýn ileri yönüne göre mevcut hýzý hesapla
        // (geri doðru seyahat ederken negatif bir sayý döndürür)
         forwardSpeed = Vector3.Dot(transform.forward, rb.velocity);


        // Calculate how close the car is to top speed
        // as a number from zero to one
        // Arabanýn azami hýza ne kadar yakýn olduðunu hesapla
        // sýfýrdan bire kadar bir sayý olarak
         speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        // Ne kadar torkun mevcut olduðunu hesaplamak için bunu kullan
        // (azami hýzda sýfýr tork)
         currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        // …ve ne kadar direksiyon çevrileceðini hesaplamak için
        // (araba en yüksek hýzda daha yumuþak bir þekilde direksiyon çevriliyor)
         currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        // Kullanýcý giriþinin aracýn hýzýyla ayný yönde olup olmadýðýný kontrol edin
         isAccelerating = Mathf.Sign(verticalInput) == Mathf.Sign(forwardSpeed);

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
        ForInformation();
        CheckDirection();
        VerticalInputZero();
        DragForce();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        MaxSpeed();
        ApplyDownforce();

        ApplyHandBreaking();
    }

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

    private void VerticalInputZero()
    {
        if (verticalInput == 0)
        {
            // Arabanýn hýzýný yavaþça azaltmak için Lerp kullanýyoruz
            float decelerationRate = 0.2f; // Yavaþlama hýzý
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, decelerationRate * Time.deltaTime);
        }
        else
        {
            // Input olduðu sürece mevcut hýzý saklýyoruz
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
    private void CheckDirection()
    {
        // Rigidbody'nin velocity'sini al
        Vector3 velocity = rb.velocity;

        // Arabanýn ileri yönde olup olmadýðýný kontrol etmek için dot product kullan
        dotProduct = Vector3.Dot(transform.forward, velocity);

        if (dotProduct > 0)
        {
            // Dot product pozitifse, araba ileri gidiyor
            Debug.Log("Araba ileri gidiyor.");
        }
        else if (dotProduct < 0)
        {
            // Dot product negatifse, araba geri gidiyor
            Debug.Log("Araba geri gidiyor.");
        }
        else
        {
            // Dot product sýfýrsa, araba duruyor
            Debug.Log("Araba duruyor.");
        }
    }
    private void HandleMotor()
    {
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;

    }


    private void ApplyHandBreaking()
    {
        frontRightWheelCollider.brakeTorque = brakeInput * handBreakForce * 0.7f;
        frontLeftWheelCollider.brakeTorque = brakeInput * handBreakForce * 0.7f;

        rearLeftWheelCollider.brakeTorque = brakeInput * handBreakForce * 0.3f;
        rearRightWheelCollider.brakeTorque = brakeInput * handBreakForce * 0.3f;
        //frontRightWheelCollider.brakeTorque = currentHandBreakForce;
        //frontLeftWheelCollider.brakeTorque = currentHandBreakForce;
        //rearLeftWheelCollider.brakeTorque = currentHandBreakForce;
        //rearRightWheelCollider.brakeTorque = currentHandBreakForce;
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
        wheelTransform.transform.GetChild(0).transform.Rotate(wheelCollider.rpm * 0.01f * Time.deltaTime, 0, 0, Space.Self);
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

    private void ApplyDownforce()
    {
        rb.AddForce(-transform.up * downforceValue);
    }
}
