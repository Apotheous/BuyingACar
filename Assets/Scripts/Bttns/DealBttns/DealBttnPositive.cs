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
    // Event tanýmlama
    public event Action<int> OnTextNumberChanged;

    // Önceki deðeri saklamak için bir deðiþken
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
        // Baþlangýçta önceki deðeri mevcut deðere eþitle
        previousTextNumber = mrSellerComps.messageHandler.textNubber;
    }
    void Update()
    {
        // Her karede deðiþkenin deðeri deðiþti mi kontrol et
        if (mrSellerComps.messageHandler.textNubber != previousTextNumber)
        {
            // Deðer deðiþmiþse, önceki deðeri güncelle
            previousTextNumber = mrSellerComps.messageHandler.textNubber;

            // Deðiþim olduðunda event'i tetikle
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
