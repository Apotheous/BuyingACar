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
    public class CamVariables
    {
        public Transform interactirSource;
    }

    public CamVariables camVariables;
    
    [Tooltip("The car we are in")]
    public Transform theCarImin;

    public float interactRange;

    [SerializeField]
    public bool inCar= false;
    #endregion

    public Speedometer speedometer;

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
                    speedometer.target = hitInfo.transform.gameObject.GetComponent<Rigidbody>();
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.E) && inCar == true)
        {
            if (theCarImin.gameObject.TryGetComponent(out ISelectionCar getinthecar))
            {
                getinthecar.GetOutOfTheCar();
                theCarImin = null;
                speedometer.target = null;
            }
        }
    }
}
