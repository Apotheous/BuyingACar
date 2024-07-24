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
        if (!m_Manager.SoldCarList.Contains(m_Manager.BttnSelectCar))
        {
            //foreach (Transform item in m_Manager.uýElements.carSelectionContent.transform)
            //{
            //    if (item.name == m_Manager.BttnSelectCar.name + "(Clone)")
            //    {
            //        Debug.Log("Destroy for car selected" + item.name);
            //        item.gameObject.SetActive(false);
            //    }
            //}
            m_Manager.SoldCarList.Add(m_Manager.BttnSelectCar);
            m_Manager.ListCarSale.Remove(m_Manager.BttnSelectCar);
        }
        else
        {
            Debug.LogWarning("This car is already on the sold list!");
        }
    }
}
