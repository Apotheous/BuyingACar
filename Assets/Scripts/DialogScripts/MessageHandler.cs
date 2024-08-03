using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    //- Scriptable Object Referansý
    [SerializeField] private DialogueScriptableObject _dialogueScriptableObject;

    //UI
    [SerializeField] private TextMeshProUGUI dialogueText;
    public int textNubber = 0;

    public void MrSellerStartText()
    {
        dialogueText.text = _dialogueScriptableObject.textStart[textNubber];
        textNubber++;
    }
    public void MrSellerResetText()
    {
        textNubber = 0;
        dialogueText.text = _dialogueScriptableObject.textStart[textNubber];
        //textNubber++;

    }
}
