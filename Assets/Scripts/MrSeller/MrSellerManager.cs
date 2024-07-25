using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MrSellerManager : MonoBehaviour
{
    [Header("Selected Car")]
    [Tooltip("The car of choice when trading")]
    public GameObject BttnSelectCar;

    [System.Serializable]
    public class UIElements
    {
        [Tooltip("Button prefab for car selection")]
        public GameObject carBtnPref;

        //[Tooltip("Button prefab for properties selection")]
        //public GameObject propsBtnPref;

        [Tooltip("Bontent from which buttons are created")]
        public GameObject carSelectionContent;
        public GameObject carSelectionContentPanel;

        //[Tooltip("Content used for deal management")]
        //public GameObject dealContent;
        //public GameObject dealContentPanel;

        [Tooltip("Text of Mr Seller")]
        public TextMeshProUGUI mrSellerText;
        public MessageHandler mrMessageHandler;
    }
    public UIElements uýElements;


    private GameObject[] allObjects;


    [Tooltip("List of cars for sale")]
    public List<GameObject> ListCarSale = new List<GameObject>();
    public List<int> gnrlPropValue = new List<int>();

    public List<GameObject> SoldCarList= new List<GameObject>();

    public int gnrlPropValueCars = 0;
    Text carBttnText;

    //----
    // Define a list of TextMeshProUGUI components
    public List<TextMeshProUGUI> textElements = new List<TextMeshProUGUI>(8);

    void Start()
    {
        MrSellerStartFoncs();
    }

    private void MrSellerStartFoncs()
    {
        allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<Car>() != null)
            {

                ListCarSale.Add(obj);

                Instantiate(uýElements.carBtnPref, uýElements.carSelectionContent.transform);

                uýElements.carBtnPref.name = obj.name;

                uýElements.carBtnPref.GetComponent<CarBttnPref>().carObjOfBttn = obj;

                uýElements.carBtnPref.SetActive(true);

                carBttnText = uýElements.carBtnPref.transform.GetChild(0).GetComponent<Text>();

                carBttnText.text = obj.name;
                gnrlPropValue.Add(obj.GetComponent<CarMain>().generalPropValue);
                gnrlPropValueCars = gnrlPropValueCars + obj.GetComponent<CarMain>().generalPropValue;

            }
        }
        //We empty the array that is no longer needed so that it does not take up space in memory.
        allObjects = null;

        uýElements.mrMessageHandler = gameObject.GetComponent<MessageHandler>();
        gnrlPropValueCars = gnrlPropValueCars / ListCarSale.Count;
        foreach (var item in ListCarSale)
        {
            if (item.GetComponent<CarMain>().generalPropValue <= gnrlPropValueCars)
            {
                item.GetComponent<Car>().isScrap = true;
            }
        }
    }

    public void PrintPropsSelectedCar()
    {
        textElements[0].text = BttnSelectCar.GetComponent<Car>().carObject.name;
        textElements[1].text = BttnSelectCar.GetComponent<Car>().carObject.damagedParts.ToString();
        textElements[2].text = BttnSelectCar.GetComponent<Car>().carObject.paintedParts.ToString();
        textElements[3].text = BttnSelectCar.GetComponent<Car>().carObject.maxSpeed.ToString();
        textElements[4].text = BttnSelectCar.GetComponent<Car>().carObject.torque.ToString();
        textElements[5].text = BttnSelectCar.GetComponent<Car>().carObject.Suspensions.ToString();
        textElements[6].text = BttnSelectCar.GetComponent<Car>().carObject.wheelCamberValues.ToString();
        textElements[7].text = BttnSelectCar.GetComponent<Car>().carObject.price.ToString();
        foreach (Transform  item in uýElements.carSelectionContent.transform)
        {
            item.transform.gameObject.SetActive(false);
        }
    }
    public void DeselectSelectCar()
    {
        foreach (var item in textElements)
        {
            item.text = null;
        }

        foreach (Transform item in uýElements.carSelectionContent.transform)
        {
            bool isSoldCar = false;

            if (SoldCarList != null)
            {
                foreach (GameObject obj in SoldCarList)
                {
                    if (obj.name + "(Clone)" == item.name)
                    {
                        isSoldCar = true;
                        break;
                    }
                }
            }

            if (isSoldCar)
            {
                Destroy(item.gameObject);
            }
            else
            {
                item.gameObject.SetActive(true);
            }
        }
    }
    void MrSellerTextMetod(string text)
    {
        uýElements.mrSellerText.text =  text;
    }
    public void CarSelectionContentPanelOn()
    {
        uýElements.carSelectionContentPanel.SetActive(true);
    }
    public void ToggleActiveState(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    public void SelectedCar()
    {
        if (BttnSelectCar!=null)
        {
            BttnSelectCar.GetComponent<Car>().IsActive=true;
        }     
    }
    public void MrSellerDealNegative()
    {
         uýElements.mrMessageHandler.MrSellerNegativeText();
    }
}
