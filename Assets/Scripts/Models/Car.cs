using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CarMain car = new CarMain();
        car.carName = gameObject.name.ToString();

        car.damagedParts = Random.Range(1, 10);

        car.paintedParts = Random.Range(1, 10);

        car.maxSpeed = Random.Range(1, 10);

        car.torque = Random.Range(1, 10);

        car.Suspensions = Random.Range(1, 10);

        car.wheelCamberValues = Random.Range(1, 10);

        Debug.Log($"Car Name = {car.carName}, " +
            $"Car Damaged Parts = {car.damagedParts}, " +
            $"Car Painted Parts = {car.paintedParts}, " +
            $"Car Max Speed = {car.maxSpeed}, " +
            $"Car Torque Value = {car.torque}, " +
            $"Car Suspensions = {car.Suspensions}, " +
            $"Car Wheel Camber Values = {car.wheelCamberValues}");
    }
}
