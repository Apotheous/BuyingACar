using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MrSellerManager : MonoBehaviour
{
    public List<GameObject> ListCarSale = new List<GameObject>();
    public List<GameObject> SoldCarList= new List<GameObject>();

    public List<GameObject> carBtn = new List<GameObject>();
    Text carBttnText;
    public GameObject carBtnPref;
    public GameObject content;


    //----
    // Define a list of TextMeshProUGUI components
    public List<TextMeshProUGUI> textElements = new List<TextMeshProUGUI>(8);

    void Start()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<Car>() != null)
            {
                ListCarSale.Add(obj);
                if (ListCarSale.Count > carBtn.Count) 
                { 
                    Instantiate(carBtnPref, content.transform); 
                }
            }
        }
        Debug.Log("Found " + ListCarSale.Count + " cars in the scene.");
       
        for (int i = 0; i < ListCarSale.Count; i++)
        {
            carBtn[i].name= ListCarSale[i].name;    
            carBtn[i].SetActive(true);
            Debug.Log("carBtnList"+ carBtn[i].name);
            Debug.Log("carBtnList"+ carBtn[i].transform.GetChild(0).name);
            Debug.Log("carBtnList"+ ListCarSale[i].name);
            carBttnText = carBtn[i].transform.GetChild(0).GetComponent<Text>();
            carBttnText.text= ListCarSale[i].name;
        }
    }





}
