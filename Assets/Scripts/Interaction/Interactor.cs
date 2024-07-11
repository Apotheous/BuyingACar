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
    public Transform theCarImin;
    public float interactRange;

    [SerializeField] bool inCar= false;


    void Update()
    {
        SelectCar();
    }

    private void SelectCar()
    {
        if (Input.GetKeyDown(KeyCode.E)&&inCar==false)
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
        else if (Input.GetKeyDown(KeyCode.E) && inCar == true)
        {
            Debug.Log("Trying to get out of the car");
            GettinOutCar(theCarImin);
        }
    }

    private void DriveCarComps(Transform car)
    {
        driveCam.SetParent(car.transform);

        driveCam.gameObject.SetActive(true);
        character.gameObject.SetActive(false);
        character.transform.SetParent(car.transform);
        followPoint.SetParent(car.transform);
        Vector3 carPos = new Vector3(car.transform.position.x, 1f, car.transform.position.z);
        
        followPoint.position = carPos;
        followPoint.rotation = car.rotation;

        car.transform.gameObject.GetComponent<CarController>().enabled = true;
        inCar = true;
        theCarImin=car.transform;
    }
    private void GettinOutCar(Transform car)
    {
        interactirSource.position = driveCam.position;
        driveCam.SetParent(null);
        driveCam.gameObject.SetActive(false);
        character.transform.SetParent(null);
        character.gameObject.SetActive(true);
        car.GetComponent<Rigidbody>().isKinematic=true;
        car.transform.gameObject.GetComponent<CarController>().enabled = false;
        inCar = false;
        car.GetComponent<Rigidbody>().isKinematic = false;
    }
}
