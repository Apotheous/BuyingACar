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
        // Mevcut d���m� kontrol etme
        Node currentNode = dialogBehaviour.CurrentNode;
        Debug.Log("Current Node: " + currentNode);

        // Mevcut d���m�n t�r�n� kontrol etme
        string nodeType = dialogBehaviour.GetCurrentNodeType();
        Debug.Log("Current Node Type: " + nodeType);

        // Mevcut d���m�n metnini kontrol etme
        string nodeText = dialogBehaviour.GetCurrentNodeText();
        Debug.Log("Current Node Text: " + nodeText);
    }
}



