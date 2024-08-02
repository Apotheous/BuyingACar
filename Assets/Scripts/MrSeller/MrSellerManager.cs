using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Interactor;

public class MrSellerManager : MonoBehaviour,ISelectionSeller
{
    #region Veriables
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
        public GameObject changedCarPropsPanel;
        public GameObject tradePanel;

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

    #endregion
    void Start()
    {
        StartCoroutine(MrSellerStartFoncsDelayed());
    }
    #region TalkSellerMethods
    public void SelectionSeller()
    {

        ForResetPanelsMrSeller();
        ShowCursor();
        Debug.Log("Changed Seller = " + gameObject.name);
        if (!uiElements.changedCarPropsPanel.activeSelf)
        {


            uiElements.changedCarPropsPanel.SetActive(true);
            uiElements.tradePanel.SetActive(true);
            uiElements.carSelectionContent.SetActive(true);

            gameObject.GetComponent<MessageHandler>().MrSellerStartText();
            gameObject.GetComponent<MessageHandler>().textNubber =1;
        }
    }
    public void ForResetPanelsMrSeller()
    {
        BttnSelectCar = null;
        DeselectSelectCar();
        gameObject.GetComponent<MessageHandler>().MrSellerResetText();
        CarSelectionContentPanelOn();
    }

    void ShowCursor()
    {
        //Make the mouse cursor visible
        Cursor.visible = true;

        // Make the mouse cursor move freely
        Cursor.lockState = CursorLockMode.None;
    }
    #endregion
    private IEnumerator MrSellerStartFoncsDelayed()
    {
        yield return new WaitForSeconds(0.1f); // Kýsa bir gecikme

        MrSellerStartFoncs();
    }

    private void MrSellerStartFoncs()
    {

        foreach (GameObject obj in ListCarSale)
        {
 
            GameObject newCarBtn = Instantiate(uiElements.carBtnPref, uiElements.carSelectionContent.transform);
            newCarBtn.name = obj.name;

            carBttnText = newCarBtn.transform.GetChild(0).GetComponent<Text>();
            carBttnText.text = obj.name;

            newCarBtn.GetComponent<CarBttnPref>().carObjOfBttn = obj;

            GetGeneralPropVal(obj);

        }

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

}
