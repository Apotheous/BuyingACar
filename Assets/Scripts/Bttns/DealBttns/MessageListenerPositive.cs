using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageListenerPositive : MonoBehaviour
{
    public DealBttnPositive messageHandler;
    Text bttnText;
    private void Awake()
    {
        bttnText= messageHandler.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
    }
    void OnEnable()
    {   
        messageHandler.OnTextNumberChanged += OnTextNumberChanged;
    }

    void OnDestroy()
    {
        messageHandler.OnTextNumberChanged -= OnTextNumberChanged;
    }

    void OnTextNumberChanged(int newTextNumber)
    {
        switch (newTextNumber)
        {
            case 0:
                bttnText.text = "";
                break;  
            case 1:
                bttnText.text = "";
                break;
            case 2:
                bttnText.text = "I loved this car.";
                break;
            case 3:
                bttnText.text = "I don't need to try, I get it";
                break;

            default:
                break;
        }


    }



}
