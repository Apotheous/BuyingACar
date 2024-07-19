using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageListenerNegative : MonoBehaviour
{
    public DealBttnNegative messageHandler;
    Text bttnText;
    private void Awake()
    {
        bttnText = messageHandler.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
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
                bttnText.text = "";//Thanks, the price seems good. I'm taking the car.
                break;
            case 2:
                bttnText.text = "00000000000000000000";
                break;
            case 3:
                bttnText.text = "Thanks, the price seems good. I'm taking the car.";//You want too much money for this junk.
                break;

            default:
                break;
        }


    }
}
