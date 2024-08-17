using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalFuncs : MonoBehaviour
{
    MrSellerManager m_Manager;
    public PlayerSc playerSc;

    private void Start()
    {
        m_Manager= gameObject.GetComponent<MrSellerManager>();
        //playerSc = gameObject.GetComponent<PlayerSc>();

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
        if (playerSc.gold>m_Manager.BttnSelectCar.gameObject.GetComponent<CarMain>().price)
        {
            if (!m_Manager.SoldCarList.Contains(m_Manager.BttnSelectCar) )
            {
                m_Manager.gold += m_Manager.BttnSelectCar.gameObject.GetComponent<CarMain>().price;
                //playerSc.gold -=m_Manager.BttnSelectCar.gameObject.GetComponent<CarMain>().price;
                playerSc.ChangeGold(-m_Manager.BttnSelectCar.gameObject.GetComponent<CarMain>().price);
                m_Manager.SoldCarList.Add(m_Manager.BttnSelectCar);
                m_Manager.ListCarSale.Remove(m_Manager.BttnSelectCar);
            }
            else
            {
                Debug.LogWarning("This car is already on the sold list!");
            }
        }
        else
        {
            Debug.LogWarning("You don't have money to buy this car");
        }
    }
}
