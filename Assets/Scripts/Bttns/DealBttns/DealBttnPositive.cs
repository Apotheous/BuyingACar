using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MrSellerManager;

public class DealBttnPositive : MonoBehaviour
{
    [System.Serializable]
    public class MrSellerComps
    {
        public MrSellerManager mrSellerManager;
        public MessageHandler messageHandler;

    }
    public MrSellerComps mrSellerComps;
    // Event tan�mlama
    public event Action<int> OnTextNumberChanged;

    // �nceki de�eri saklamak i�in bir de�i�ken
    private int previousTextNumber;

    private void Awake()
    {
        // Initialize MrSellerManager
        GameObject sellerManagerObject = GameObject.Find("MrSeller");

        if (sellerManagerObject != null)
        {
            mrSellerComps.mrSellerManager = sellerManagerObject.GetComponent<MrSellerManager>();
            mrSellerComps.messageHandler = sellerManagerObject.GetComponent<MessageHandler>();
        }

    }

    void Start()
    {
        // Ba�lang��ta �nceki de�eri mevcut de�ere e�itle
        previousTextNumber = mrSellerComps.messageHandler.textNubber;
    }
    void Update()
    {
        // Her karede de�i�kenin de�eri de�i�ti mi kontrol et
        if (mrSellerComps.messageHandler.textNubber != previousTextNumber)
        {
            // De�er de�i�mi�se, �nceki de�eri g�ncelle
            previousTextNumber = mrSellerComps.messageHandler.textNubber;

            // De�i�im oldu�unda event'i tetikle
            OnTextNumberChanged?.Invoke(previousTextNumber);
        }
    }
    public void DealPositivie()
    {
        mrSellerComps.mrSellerManager.SelectedCar();
    }
    public void DealNegative()
    {
        mrSellerComps.mrSellerManager.MrSellerDealNegative();
    }

}
