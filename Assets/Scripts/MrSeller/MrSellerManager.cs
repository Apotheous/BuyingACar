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
    public UIElements uiElements;


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
        StartCoroutine(MrSellerStartFoncsDelayed());
    }

    private IEnumerator MrSellerStartFoncsDelayed()
    {
        yield return new WaitForSeconds(0.1f); // Kýsa bir gecikme

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

                GameObject newCarBtn = Instantiate(uiElements.carBtnPref, uiElements.carSelectionContent.transform);
                newCarBtn.name = obj.name;

                carBttnText = newCarBtn.transform.GetChild(0).GetComponent<Text>();
                carBttnText.text = obj.name;

                newCarBtn.GetComponent<CarBttnPref>().carObjOfBttn = obj;

                GetGeneralPropVal(obj);
            }
        }
        //We empty the array that is no longer needed so that it does not take up space in memory.
        allObjects = null;

        uiElements.mrMessageHandler = gameObject.GetComponent<MessageHandler>();
        gnrlPropValueCars = gnrlPropValueCars / ListCarSale.Count;
        foreach (var item in ListCarSale)
        {
            if (item.GetComponent<CarMain>().generalPropValue <= gnrlPropValueCars)
            {
                item.GetComponent<Car>().isScrap = true;
            }
        }
    }
    void GetGeneralPropVal(GameObject obj)
    {
        Debug.Log("obj carObject gnrl" + obj.GetComponent<Car>().carObject.generalPropValue);
        gnrlPropValue.Add(obj.GetComponent<Car>().carObject.generalPropValue);
        gnrlPropValueCars = gnrlPropValueCars + obj.GetComponent<Car>().carObject.generalPropValue;
    }
    public void PrintPropsSelectedCar()
    {
        Car selectedCar = BttnSelectCar.GetComponent<Car>();
        var carObject = selectedCar.carObject;

        string[] carProperties = {
        carObject.name,
        carObject.damagedParts.ToString(),
        carObject.paintedParts.ToString(),
        carObject.maxSpeed.ToString(),
        carObject.torque.ToString(),
        carObject.Suspensions.ToString(),
        carObject.wheelCamberValues.ToString(),
        carObject.price.ToString()
        };

        for (int i = 0; i < textElements.Count; i++)
        {
            textElements[i].text = carProperties[i];
        }
    }
    public void DeselectSelectCar()
    {
        foreach (var item in textElements)
        {
            item.text = null;
        }

        foreach (Transform item in uiElements.carSelectionContent.transform)
        {
            bool isSoldCar = false;

            if (SoldCarList != null)
            {
                foreach (GameObject obj in SoldCarList)
                {
                    if (obj.name == item.name)
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

    public void CarSelectionContentPanelOn()
    {
        uiElements.carSelectionContentPanel.SetActive(true);
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
    void MrSellerDealNegative()
    {
         uiElements.mrMessageHandler.MrSellerNegativeText();
    }
}
