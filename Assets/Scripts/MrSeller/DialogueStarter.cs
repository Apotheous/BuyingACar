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
        dialogBehaviour.BindExternalFunction("SaleListToSoldList", externalFunc.SaleListToSoldList);
        //dialogBehaviour.BindExternalFunction("ILoveThisCar", externalFunc.SaleListToSoldList);
    }
    public void DialogStart()
    {
        

        // Dialog'u baþlatma
        dialogBehaviour.StartDialog(dialogGraph);


    }
}



