using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{
    public List<TextMeshProUGUI> textElements = new List<TextMeshProUGUI>(8);
    public CarMain carObject = new CarMain();

    [SerializeField]
    private bool isActive;
    public bool isScrap;
    [Tooltip("If the car lock sold is true, it is sold. It means the car can be used.")]

    public event Action<bool> OnValueChanged;

    public bool IsActive
    {
        get => isActive;
        set
        {
            if (isActive != value)
            {
                isActive = value;
                OnValueChanged?.Invoke(isActive);
            }
        }
    }

    void Awake()
    {
        SetCarPropsStart();
        PrintProperties();
    }

    private void PrintProperties()
    {
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

    private void SetCarPropsStart()
    {
        carObject = this.gameObject.AddComponent<CarMain>();
        carObject.CarName(this.gameObject.name);
        carObject.InitializeRandomValues();
    }

    private void OnEnable()
    {
        OnValueChanged += HandleValueChanged;
    }
    void HandleValueChanged(bool newValue)
    {
        CarController carController = gameObject.GetComponent<CarController>();

        if (carController.enabled)
        {
            carController.enabled = false;
        }
    }
    public void PriceCalculation()
    {
        int newPrice;
        newPrice=carObject.maxSpeed+carObject.torque+carObject.paintedParts+carObject.torque;
        newPrice = newPrice * 1000;
        carObject.price = newPrice;
        PrintProperties();
    }
    public void IsTheCarScrap()
    {
        int scrapPrice = carObject.price-1000;
        carObject.price = scrapPrice;
        PrintProperties();
    }
}
