using Models;
using System;
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
    //string carNameThis;

 
    [SerializeField]
    private bool isActive;
    [Tooltip("If the car lock sold is true, it is sold. It means the car can be used.")]
    // Event tanýmlama
    public event Action<bool> OnValueChanged;

    // bool deðiþkeni için özellik
    public bool IsActive
    {
        get => isActive;
        set
        {
            if (isActive != value)
            {
                isActive = value;
                // Deðer deðiþtiðinde event'i tetikle
                OnValueChanged?.Invoke(isActive);
            }
        }
    }

    void Start()
    {
        carObject = this.gameObject.AddComponent<CarMain>();
        //carNameThis=this.gameObject.name;
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
    private void OnEnable()
    {
        // Event'e abone ol
        OnValueChanged += HandleValueChanged;

        // Test amaçlý olarak baþlangýçta deðeri deðiþtir
        //IsActive = true;
    }
    // Event tetiklendiðinde çaðrýlacak metod
    void HandleValueChanged(bool newValue)
    {
        Debug.Log("IsActive changed to: " + newValue);
        if (gameObject.GetComponent<CarController>().enabled ==false )
        {
            //gameObject.GetComponent<CarController>().enabled = true;
        }else { gameObject.GetComponent<CarController>().enabled = false; }
        
    }

 
}
