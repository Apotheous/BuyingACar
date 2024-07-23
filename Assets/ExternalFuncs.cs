using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalFuncs : MonoBehaviour
{
    [SerializeField] public MrSellerManager m_Manager;


    private void Start()
    {
        m_Manager= gameObject.GetComponent<MrSellerManager>();
    }

    public void Discount()
    {
        m_Manager.BttnSelectCar.GetComponent<Car>().PriceCalculation();
        Debug.Log("The price of the car has been reduced");
        m_Manager.SelectCarPrintProps();
    }  
    public void ScrapBargaining()
    {
        if(m_Manager.BttnSelectCar.GetComponent<Car>().isScrap==true)
        Debug.Log("You are right. The car is scrap. I reduce the price.");
        m_Manager.BttnSelectCar.GetComponent <Car>().IsTheCarScrap();
        m_Manager.SelectCarPrintProps();
    }


}
