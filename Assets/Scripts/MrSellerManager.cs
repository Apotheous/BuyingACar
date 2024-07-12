using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrSellerManager : MonoBehaviour
{
    public List<GameObject> ListCarSale = new List<GameObject>();
    public List<GameObject> SoldCarList= new List<GameObject>();
    void Start()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<Car>() != null)
            {
                ListCarSale.Add(obj);
            }
        }
        Debug.Log("Found " + ListCarSale.Count + " cars in the scene.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
