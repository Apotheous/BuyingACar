using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    Rigidbody rb;
    private float horizontalInput, verticalInput;
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
    // Alt objenin (örneðin, wheelChild) kamber açýsýný eklemek
    public float camberAngle; // Örnek olarak 10 derece kamber
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //maxSpeed=gameObject.GetComponent<Car>().carObject.maxSpeed;  
        motorForce=gameObject.GetComponent<Car>().carObject.torque*1000;

        float frontWheels, rearWheels;
        frontWheels=Random.Range(0.3f,0.7f);
        rearWheels=Random.Range(0.3f,0.7f);
        SetSuspensionSpringTargetPosition(frontLeftWheelCollider, frontWheels); // En alt pozisyon
        SetSuspensionSpringTargetPosition(frontRightWheelCollider, frontWheels); // En üst pozisyon
        SetSuspensionSpringTargetPosition(rearLeftWheelCollider, rearWheels); // Orta pozisyon
        SetSuspensionSpringTargetPosition(rearRightWheelCollider, rearWheels); // Biraz alt pozisyon
    }

    private void FixedUpdate()
    {
        GetInput();
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
    void SetSuspensionSpringTargetPosition(WheelCollider wheelCollider, float targetPosition)
    {
        // JointSpring struct'ýný al
        JointSpring suspensionSpring = wheelCollider.suspensionSpring;

        // targetPosition deðerini ayarla (0 ile 1 arasýnda olmalýdýr)
        suspensionSpring.targetPosition = Mathf.Clamp01(targetPosition);

        // JointSpring struct'ýný geri ata
        wheelCollider.suspensionSpring = suspensionSpring;
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
