using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Car : MonoBehaviour
{
    // Define a list of TextMeshProUGUI components
    public List<TextMeshProUGUI> textElements = new List<TextMeshProUGUI>(7);
    void Start()
    {
        CarMain car = new CarMain();

        car.carName = gameObject.name.ToString();
        textElements[0].text = car.carName;

        car.damagedParts = Random.Range(1, 10);
        textElements[1].text = car.damagedParts.ToString();

        car.paintedParts = Random.Range(1, 10);
        textElements[2].text = car.paintedParts.ToString();

        car.maxSpeed = Random.Range(1, 10);
        textElements[3].text = car.maxSpeed.ToString();

        car.torque = Random.Range(1, 10);
        textElements[4].text = car.torque.ToString();

        car.Suspensions = Random.Range(1, 10);
        textElements[5].text = car.Suspensions.ToString();

        car.wheelCamberValues = Random.Range(1, 10);
        textElements[6].text = car.wheelCamberValues.ToString();

        Debug.Log($"Car Name = {car.carName}, " +
            $"Car Damaged Parts = {car.damagedParts}, " +
            $"Car Painted Parts = {car.paintedParts}, " +
            $"Car Max Speed = {car.maxSpeed}, " +
            $"Car Torque Value = {car.torque}, " +
            $"Car Suspensions = {car.Suspensions}, " +
            $"Car Wheel Camber Values = {car.wheelCamberValues}");


    }
}
