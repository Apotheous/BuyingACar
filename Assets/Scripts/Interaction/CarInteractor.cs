using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteractor : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Selected Car Name = " + gameObject.name);
    }
}
