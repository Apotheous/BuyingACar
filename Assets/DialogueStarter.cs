using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static cherrydev.DialogExternalFunctionsHandler;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;
    [SerializeField] private ExternalFuncs externalFunc;

    private void Start()
    {
        // Harici iþlevi baðlama
        externalFunc = GetComponent<ExternalFuncs>();
        dialogBehaviour.BindExternalFunction("Discount", externalFunc.Discount);
        dialogBehaviour.BindExternalFunction("ScrapBargaining", externalFunc.ScrapBargaining);
    }
    public void DialogStart()
    {

        
        dialogBehaviour.StartDialog(dialogGraph);
        // Mevcut düðümü kontrol etme
        Node currentNode = dialogBehaviour.CurrentNode;
        Debug.Log("Current Node: " + currentNode);

        // Mevcut düðümün türünü kontrol etme
        string nodeType = dialogBehaviour.GetCurrentNodeType();
        Debug.Log("Current Node Type: " + nodeType);

        // Mevcut düðümün metnini kontrol etme
        string nodeText = dialogBehaviour.GetCurrentNodeText();
        Debug.Log("Current Node Text: " + nodeText);
    }
}



