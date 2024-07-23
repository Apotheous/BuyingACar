using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;
    [SerializeField] private DialogNodeGraph beforeSelectCar;
    public void DialogStart()
    {
        //dialogBehaviour.StartDialog(beforeSelectCar);
        
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



