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
    public void MrSellerPositiveText()
    {
        dialogueText.text = _dialogueScriptableObject.textPositive[textNubber];
        textNubber++;
    }    
    public void MrSellerNegativeText()
    {
        dialogueText.text = _dialogueScriptableObject.textNegative[textNubber];
        textNubber++;
    }
    public void MrSellerNumberText(int txtNmbr)
    {
        dialogueText.text = _dialogueScriptableObject.textPositive[textNubber];
        textNubber=txtNmbr;
    }
    public void MrSellerResetText()
    {
        textNubber = 0;
        dialogueText.text = _dialogueScriptableObject.textStart[textNubber];
        textNubber++;

    }
}
