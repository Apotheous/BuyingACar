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
        maxSpeed=gameObject.GetComponent<Car>().carObject.maxSpeed;  
        motorForce=gameObject.GetComponent<Car>().carObject.torque*1000;
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

        //Vector3 pos;
        //Quaternion rot;

        //// WheelCollider'dan pozisyon ve rotasyonu al
        //wheelCollider.GetWorldPose(out pos, out rot);


        //Quaternion camberRotation = Quaternion.Euler(0, 0, camberAngle);

        //// Alt nesneyi al
        //Transform wheelChild = wheelTransform.GetChild(0);

        //// Alt nesnenin rotasyonunu güncelle
        //wheelChild.localRotation = camberRotation;

        ////// WheelTransform'un rotasyonunu güncelle
        ////wheelTransform.rotation = rot;

        //// Pozisyonu güncelle
        //wheelTransform.position = pos;

        //Vector3 pos;
        //Quaternion rot;
        //wheelCollider.GetWorldPose(out pos, out rot);
        //wheelTransform.rotation = rot;
        //wheelTransform.position = pos;
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
