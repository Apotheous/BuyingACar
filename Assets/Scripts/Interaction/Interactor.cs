using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;


interface IInteractable
{
    public void Interact();
}
public class Interactor : MonoBehaviour
{


    public Transform interactirSource;
    public Transform driveCam;
    public Transform followPoint;
    public Transform character;
    public float interactRange;

    [SerializeField] bool inCar= false;


    void Update()
    {
        SelectCar();
    }

    private void SelectCar()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(interactirSource.position, interactirSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();

                    DriveCarComps(hitInfo.transform);
                }
            }
        }
    }

    private void DriveCarComps(Transform car)
    {
        driveCam.SetParent(car.transform);
        driveCam.gameObject.SetActive(true);
        character.gameObject.SetActive(false);
        followPoint.SetParent(car.transform);
        Vector3 carPos = new Vector3(car.transform.position.x, 1f, car.transform.position.z);
        followPoint.position = carPos;
        car.transform.gameObject.GetComponent<CarController>().enabled = true;
    }
}
