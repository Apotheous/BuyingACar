using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;

//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering.Universal;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    #region Variables
    [System.Serializable]
    public class MrSellerVariables
    {
        [Header("Objects")]
        public GameObject MrSeller;
        [Header("UI Objects")]
        [Tooltip("Mr seller's speech panel")]
        public GameObject TradePanel;
        [Tooltip("Panel where Mr Seller shows the features of the selected car")]
        public GameObject SelectedCarPropsPanel;
        [Tooltip("Content listing Mr Seller's buttons for interaction")]
        public GameObject content;
        public GameObject DealPanel;
    }
    public MrSellerVariables mrSellerVariables;

    [System.Serializable]
    public class CamVariables
    {
        public Transform interactirSource;
    }

    public CamVariables camVariables;

    [System.Serializable]
    public class Character
    {
        [Tooltip("Character Controller")]
        public Transform character;
    }

    public Character characterCs;
    
    [Tooltip("The car we are in")]
    public Transform theCarImin;

    public float interactRange;

    [SerializeField]
    public bool inCar= false;
    #endregion

    void Update()
    {
        SelectCar();

        SelectionSeller();
    }

    void SelectionSeller()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Ray r = new Ray(camVariables.interactirSource.position, camVariables.interactirSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                ISelectionSeller iSelectionSeller =hitInfo.collider.GetComponent<ISelectionSeller>();

                if (iSelectionSeller != null)
                {
                    iSelectionSeller.SelectionSeller();
                }
            }
        }
    }

    private void SelectCar()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCar == false)
        {
            Ray r = new Ray(camVariables.interactirSource.position, camVariables.interactirSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {

                if (hitInfo.collider.gameObject.TryGetComponent(out ISelectionCar getinthecar))
                {
                    
                    getinthecar.GetInTheCar();
                    theCarImin = hitInfo.transform;

                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.E) && inCar == true)
        {
            if (theCarImin.gameObject.TryGetComponent(out ISelectionCar getinthecar))
            {
                getinthecar.GetOutOfTheCar();
                theCarImin = null;
            }
 

        }
    }    

    //private void MrSellerTalkOn(RaycastHit hitInfo)
    //{
    //    ForResetPanels();
    //    ShowCursor();

    //    hitInfo.collider.GetComponent<MrSellerManager>().BttnSelectCar = null;

    //    mrSellerVariables.SelectedCarPropsPanel.SetActive(true);
    //    mrSellerVariables.TradePanel.SetActive(true);
    //    mrSellerVariables.content.SetActive(true);

    //    hitInfo.collider.GetComponent<MessageHandler>().MrSellerStartText();
    //    hitInfo.collider.GetComponent<MessageHandler>().textNubber = 1;

    //    characterCs.character.gameObject.SetActive(false);
    //}
    private void ForResetPanels()
    {
        mrSellerVariables.MrSeller.GetComponent<MrSellerManager>().BttnSelectCar = null;
        mrSellerVariables.MrSeller.GetComponent<MrSellerManager>().DeselectSelectCar();
        mrSellerVariables.MrSeller.GetComponent<MessageHandler>().MrSellerResetText();
        mrSellerVariables.MrSeller.GetComponent<MrSellerManager>().CarSelectionContentPanelOn();
    }


    //Called every time a conversation is closed in mrSeller's dialog panel
    public void MrSellerTalkOff()
    {
        mrSellerVariables.TradePanel.SetActive(false);
        mrSellerVariables.SelectedCarPropsPanel.SetActive(false);
        characterCs.character.gameObject.SetActive(true);
        // Invert the visibility of the mouse cursor
        Cursor.visible = !Cursor.visible;

        //Reverse whether the mouse cursor is locked or not
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void ToggleActiveState(GameObject obj)
    {
        // Reverse GameObject's active state
        obj.SetActive(!obj.activeSelf);
    }
    void ShowCursor()
    {
        //Make the mouse cursor visible
        Cursor.visible = true;

        // Make the mouse cursor move freely
        Cursor.lockState = CursorLockMode.None;
    }

}
