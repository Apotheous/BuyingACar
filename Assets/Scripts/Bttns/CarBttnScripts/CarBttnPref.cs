using cherrydev;
using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CarBttnPref : MonoBehaviour
{

    public string btnName;
    public GameObject carObjOfBttn;
    public MrSellerManager mrSellerManager;

    public GameObject MrSellerContent;
   
    private void Start()
    {
        // Initialize MrSellerManager
        
        GameObject sellerManagerObject =transform.root.gameObject ;
        
        if (sellerManagerObject != null)
        {
            mrSellerManager = sellerManagerObject.GetComponent<MrSellerManager>();
        }
        foreach (Transform item in mrSellerManager.transform)
        {
            if (item.name== "MrSellerCanvas")
            {
                foreach (Transform child in item.transform)
                {
                    if (child.name == "TradeBg")
                    {
                        MrSellerContent = child.transform.gameObject;
                    }
                }
            }
        }

    }
    public void SelectCar()
    {
        mrSellerManager.BttnSelectCar = carObjOfBttn;
        mrSellerManager.GetComponent<MessageHandler>().MrSellerStartText();
        mrSellerManager.PrintPropsSelectedCar();
        mrSellerManager.CarSelectionContentPanelOn();
        mrSellerManager.GetComponent<DialogueStarter>().DialogStart();
        MrSellerContent.SetActive(false);
        mrSellerManager.SelectedCar();
    }

    //public void ToggleActiveState(GameObject obj)
    //{
    //    // GameObject'in aktiflik durumunu tersine �evir
    //    obj.SetActive(!obj.activeSelf);
    //}
}
