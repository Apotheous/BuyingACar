using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{
    // Define a list of TextMeshProUGUI components
    public List<TextMeshProUGUI> textElements = new List<TextMeshProUGUI>(8);
    public CarMain carObject = new CarMain();
    string carNameThis;
    void Start()
    {
        carObject = this.gameObject.AddComponent<CarMain>();
        carNameThis=this.gameObject.name;
        carObject.CarName(this.gameObject.name);

        carObject.InitializeRandomValues();
        Debug.Log($" {carObject.ToString()}");
        textElements[0].text = carObject.name;
        textElements[1].text = carObject.damagedParts.ToString();
        textElements[2].text = carObject.paintedParts.ToString();
        textElements[3].text = carObject.maxSpeed.ToString();
        textElements[4].text = carObject.torque.ToString();
        textElements[5].text = carObject.Suspensions.ToString();
        textElements[6].text = carObject.wheelCamberValues.ToString();
        textElements[7].text = carObject.price.ToString();
        

    }
    private void OldMetod()
    {
        CarMain car = new CarMain();
        car.carName = gameObject.name;
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

        car.price = (car.damagedParts + car.paintedParts + car.maxSpeed + car.torque) * 1000;
        textElements[7].text = car.price.ToString() + " $ ";

        Debug.Log($"Car Name = {car.carName}, " +
            $"Car Damaged Parts = {car.damagedParts}, " +
            $"Car Painted Parts = {car.paintedParts}, " +
            $"Car Max Speed = {car.maxSpeed}, " +
            $"Car Torque Value = {car.torque}, " +
            $"Car Suspensions = {car.Suspensions}, " +
            $"Car Wheel Camber Values = {car.wheelCamberValues}");
    }
}
