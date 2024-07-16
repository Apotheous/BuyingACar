using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    //- Scriptable Object Referansý
    [SerializeField] private DialogueScriptableObject _dialogueScriptableObject;
    //-

    //UI
    [SerializeField] private TextMeshProUGUI dialogueText;
    public int textNubber;

    private void Start()
    {
        //StartCoroutine(DialogueLoop());
    }

    IEnumerator DialogueLoop()
    {
        if (_dialogueScriptableObject.text.Length < 1)
        {
            Debug.LogError("Text array is empty!");
            yield return null;
        }

        for (int i = 0; i < _dialogueScriptableObject.text.Length; i++)
        {
            dialogueText.text = _dialogueScriptableObject.text[i];
            if (i == _dialogueScriptableObject.text.Length - 1) i = -1;
            yield return new WaitForSeconds(2f);
        }
    }
    public void MrSellerNextText()
    {
        dialogueText.text = _dialogueScriptableObject.text[textNubber];
        textNubber++;
        //if (textNubber >= _dialogueScriptableObject.text.Length)
        //{
        //    textNubber = 0;
        //}
    }
    public void MrSellerResetText()
    {
        dialogueText.text = _dialogueScriptableObject.text[0];
        textNubber = 1;
    }
}
