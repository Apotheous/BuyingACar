using UnityEngine;

public class CarBttnPref : MonoBehaviour
{
    public GameObject carObjOfBttn;
    public MrSellerManager mrSellerManager;

    private void Start()
    {       
        GameObject sellerManagerObject =transform.root.gameObject ;
        
        if (sellerManagerObject != null)
        {
            mrSellerManager = sellerManagerObject.GetComponent<MrSellerManager>();
        }
    }
    public void SelectCar()
    {
        mrSellerManager.BttnSelectCar = carObjOfBttn;
        mrSellerManager.PrintPropsSelectedCar();
        mrSellerManager.GetComponent<DialogueStarter>().DialogStart();
        mrSellerManager.SelectedCar();
    }
}
