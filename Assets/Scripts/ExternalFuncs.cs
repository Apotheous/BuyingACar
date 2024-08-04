using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalFuncs : MonoBehaviour
{
    MrSellerManager m_Manager;

    private void Start()
    {
        m_Manager= gameObject.GetComponent<MrSellerManager>();
    }

    public void Discount()
    {
        m_Manager.BttnSelectCar.GetComponent<Car>().PriceCalculation();
        m_Manager.PrintPropsSelectedCar();
    }  
    public void ScrapBargaining()
    {
        if (m_Manager.BttnSelectCar.GetComponent<Car>().isScrap == true)
        {
            m_Manager.BttnSelectCar.GetComponent<Car>().IsTheCarScrap();
            m_Manager.PrintPropsSelectedCar();
        }
    }

    public void SaleListToSoldList()
    {
        Debug.Log("SaleLisToSoldListCalled");
        if (!m_Manager.SoldCarList.Contains(m_Manager.BttnSelectCar))
        {
            m_Manager.SoldCarList.Add(m_Manager.BttnSelectCar);
            m_Manager.ListCarSale.Remove(m_Manager.BttnSelectCar);
        }
        else
        {
            Debug.LogWarning("This car is already on the sold list!");
        }
    }

    public void ILoveThisCar()
    {
        Debug.LogWarning("SaleLisToSoldListCalled2222");

    }
}
