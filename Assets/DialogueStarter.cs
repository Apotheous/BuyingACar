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
    }


}
