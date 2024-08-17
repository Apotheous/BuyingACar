using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSc : MonoBehaviour
{
    public int gold;
    public TextMeshProUGUI goldText; // TextMeshProUGUI kullanýyoruz
    public GameObject text;

    // Gold deðiþtiðinde tetiklenecek event
    public UnityEvent onGoldChanged;

    void Start()
    {
        gold = 100000;
        text = GameObject.Find("GoldText");
        goldText = text.GetComponent<TextMeshProUGUI>(); // TextMeshPro yerine TextMeshProUGUI kullanýyoruz
        UpdateGoldText(); // Baþlangýçta metni güncelle
    }

    private void OnEnable()
    {
        // Gold deðiþtiðinde metni güncellemek için event'e metodu ekle
        onGoldChanged.AddListener(UpdateGoldText);
    }

    private void OnDisable()
    {
        // Event'ten metodu çýkar
        onGoldChanged.RemoveListener(UpdateGoldText);
    }

    public void ChangeGold(int amount)
    {
        gold += amount;
        onGoldChanged.Invoke(); // Gold deðiþtiðinde event'i tetikle
    }

    public void UpdateGoldText()
    {
        goldText.text = $"Gold: {gold}";
    }

}
