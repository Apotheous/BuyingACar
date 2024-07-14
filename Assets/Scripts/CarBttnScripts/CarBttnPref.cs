using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CarBttnPref : MonoBehaviour
{

    public string btnName;
    public GameObject BttnCar;
    //public GameObject SelectedCarPanel;
    public MrSellerManager mrSellerManager;
   

   
    private void Start()
    {

        // Initialize MrSellerManager
        GameObject sellerManagerObject = GameObject.Find("MrSeller");
        
        BttnCar = GameObject.Find(this.gameObject.name);
        if (sellerManagerObject != null)
        {
            mrSellerManager = sellerManagerObject.GetComponent<MrSellerManager>();
        }
    }
    public void SelectCar()
    {
        BttnCar = GameObject.Find(this.gameObject.name);
        Debug.Log("BtnPrefab = " + BttnCar.GetComponent<Car>().carObject.name+ 
            "--" + BttnCar.GetComponent<Car>().carObject.damagedParts.ToString()+
            "--" + BttnCar.GetComponent<Car>().carObject.paintedParts.ToString()+ 
            "--" + BttnCar.GetComponent<Car>().carObject.maxSpeed.ToString()+
            "--" + BttnCar.GetComponent<Car>().carObject.torque.ToString());
            this.gameObject .SetActive(false);
        foreach (Transform item in mrSellerManager.transform)
        {
            Debug.Log(" Seller Childs " + item.name);
        }    
    }

    //public void ToggleActiveState(GameObject obj)
    //{
    //    // GameObject'in aktiflik durumunu tersine çevir
    //    obj.SetActive(!obj.activeSelf);
    //}
}
