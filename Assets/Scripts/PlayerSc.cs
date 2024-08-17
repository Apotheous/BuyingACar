using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSc : MonoBehaviour
{
    public int gold;
    public TextMeshProUGUI goldText; // TextMeshProUGUI kullan�yoruz
    public GameObject text;

    // Gold de�i�ti�inde tetiklenecek event
    public UnityEvent onGoldChanged;

    void Start()
    {
        gold = 100000;
        text = GameObject.Find("GoldText");
        goldText = text.GetComponent<TextMeshProUGUI>(); // TextMeshPro yerine TextMeshProUGUI kullan�yoruz
        UpdateGoldText(); // Ba�lang��ta metni g�ncelle
    }

    private void OnEnable()
    {
        // Gold de�i�ti�inde metni g�ncellemek i�in event'e metodu ekle
        onGoldChanged.AddListener(UpdateGoldText);
    }

    private void OnDisable()
    {
        // Event'ten metodu ��kar
        onGoldChanged.RemoveListener(UpdateGoldText);
    }

    public void ChangeGold(int amount)
    {
        gold += amount;
        onGoldChanged.Invoke(); // Gold de�i�ti�inde event'i tetikle
    }

    public void UpdateGoldText()
    {
        goldText.text = $"Gold: {gold}";
    }

}
