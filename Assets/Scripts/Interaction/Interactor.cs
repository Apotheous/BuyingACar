using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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
    }
    public MrSellerVariables mrSellerVariables;

    [System.Serializable]
    public class CamVariables
    {
        public Transform interactirSource;
        public Transform driveCam;
        public Transform followPoint;
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

    [SerializeField] bool inCar= false;
    #endregion

    void Update()
    {
        SelectCar();
        TalkSeller();
    }

    private void SelectCar()
    {
        if (Input.GetKeyDown(KeyCode.E)&&inCar==false)
        {
            Ray r = new Ray(camVariables.interactirSource.position, camVariables.interactirSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();

                    DriveCarComps(hitInfo.transform);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && inCar == true)
        {
            Debug.Log("Trying to get out of the car");
            GettinOutCar(theCarImin);
        }

    }    
    private void TalkSeller()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray r = new Ray(camVariables.interactirSource.position, camVariables.interactirSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                
                if (hitInfo.collider.gameObject.name=="MrSeller")
                {
                    if (!mrSellerVariables.TradePanel.activeSelf)
                    {

                        ShowCursor();
                        mrSellerVariables.TradePanel.SetActive(true);
                        hitInfo.collider.GetComponent<MrSellerManager>().BttnSelectCar = null;
                        hitInfo.collider.GetComponent<MessageHandler>().MrSellerNextText();
                        mrSellerVariables.SelectedCarPropsPanel.SetActive(true);
                        characterCs.character.gameObject.SetActive(false);

                        foreach (Transform item in mrSellerVariables.content.transform)
                        {
                            if (!item.transform.gameObject.activeSelf)
                            {
                                ToggleActiveState(item.transform.gameObject);
                            }
                        }
                    }
                    else 
                    {
                        mrSellerVariables.TradePanel.SetActive(false);
                        mrSellerVariables.SelectedCarPropsPanel.SetActive(false);
                        characterCs.character.gameObject.SetActive(true);
                        // Fare imlecinin görünürlüðünü tersine çevir
                        Cursor.visible = !Cursor.visible;

                        // Fare imlecinin kilitli olup olmadýðýný tersine çevir
                        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
                    }

                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            mrSellerVariables.MrSeller.GetComponent<MrSellerManager>().BttnSelectCar=null;
            mrSellerVariables.MrSeller.GetComponent<MrSellerManager>().DeselectSelectCar();
            mrSellerVariables.MrSeller.GetComponent<MessageHandler>().MrSellerResetText();
        }
    }
    public void ToggleActiveState(GameObject obj)
    {
        // GameObject'in aktiflik durumunu tersine çevir
        obj.SetActive(!obj.activeSelf);
    }
    void ShowCursor()
    {
        // Fare imlecini görünür yap
        Cursor.visible = true;

        // Fare imlecinin serbest hareket etmesini saðla
        Cursor.lockState = CursorLockMode.None;
    }
    private void DriveCarComps(Transform car)
    {
        camVariables.driveCam.SetParent(car.transform);

        camVariables.driveCam.gameObject.SetActive(true);
        characterCs.character.gameObject.SetActive(false);
        characterCs.character.transform.SetParent(car.transform);
        camVariables.followPoint.SetParent(car.transform);
        Vector3 carPos = new Vector3(car.transform.position.x, 1f, car.transform.position.z);
        
        camVariables.followPoint.position = carPos;
        camVariables.followPoint.rotation = car.rotation;

        car.transform.gameObject.GetComponent<CarController>().enabled = true;
        inCar = true;
        theCarImin=car.transform;
    }
    private void GettinOutCar(Transform car)
    {
        camVariables.interactirSource.position = camVariables.driveCam.position;
        camVariables.driveCam.SetParent(null);
        camVariables.driveCam.gameObject.SetActive(false);
        characterCs.character.transform.SetParent(null);
        characterCs.character.gameObject.SetActive(true);
        car.GetComponent<Rigidbody>().isKinematic=true;
        car.transform.gameObject.GetComponent<CarController>().enabled = false;
        inCar = false;
        car.GetComponent<Rigidbody>().isKinematic = false;
    }
}
