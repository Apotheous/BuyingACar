using cherrydev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogGraph;
    void Start()
    {
        dialogBehaviour.StartDialog(dialogGraph);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
