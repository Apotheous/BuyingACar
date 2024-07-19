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
        GameObject sellerManagerObject = GameObject.Find("MrSeller");
        
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
        mrSellerManager.SelectCar();
        mrSellerManager.CallDealContent();
    }

    //public void ToggleActiveState(GameObject obj)
    //{
    //    // GameObject'in aktiflik durumunu tersine çevir
    //    obj.SetActive(!obj.activeSelf);
    //}
}
