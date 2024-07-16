using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MrSellerManager : MonoBehaviour
{
    
    public GameObject[] allObjects;

    public TextMeshProUGUI mrSellerText;

    public List<GameObject> ListCarSale = new List<GameObject>();

    public List<GameObject> SoldCarList= new List<GameObject>();

    Text carBttnText;
    //Bttn Prefab
    public GameObject carBtnPref;
    //For more bttns
    public GameObject content;

    //----
    // Define a list of TextMeshProUGUI components
    public List<TextMeshProUGUI> textElements = new List<TextMeshProUGUI>(8);

    //Select Car
    public GameObject BttnSelectCar;


    void Start()
    {

        allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<Car>() != null)
            {
                ListCarSale.Add(obj);

                Instantiate(carBtnPref, content.transform);

                carBtnPref.name = obj.name;

                carBtnPref.GetComponent<CarBttnPref>().carObjOfBttn = obj;
                
                carBtnPref.SetActive(false);

                carBttnText = carBtnPref.transform.GetChild(0).GetComponent<Text>();

                carBttnText.text = obj.name;

            }
        }
        //We empty the array that is no longer needed so that it does not take up space in memory.
        allObjects = null;
    }


    public void SelectCar()
    {
        textElements[0].text = BttnSelectCar.GetComponent<Car>().carObject.name;
        textElements[1].text = BttnSelectCar.GetComponent<Car>().carObject.damagedParts.ToString();
        textElements[2].text = BttnSelectCar.GetComponent<Car>().carObject.paintedParts.ToString();
        textElements[3].text = BttnSelectCar.GetComponent<Car>().carObject.maxSpeed.ToString();
        textElements[4].text = BttnSelectCar.GetComponent<Car>().carObject.torque.ToString();
        textElements[5].text = BttnSelectCar.GetComponent<Car>().carObject.Suspensions.ToString();
        textElements[6].text = BttnSelectCar.GetComponent<Car>().carObject.wheelCamberValues.ToString();
        textElements[7].text = BttnSelectCar.GetComponent<Car>().carObject.price.ToString();
        foreach (Transform  item in content.transform)
        {
            item.transform.gameObject.SetActive(false);
        }
    }

    void MrSellerTextMetod(string text)
    {
        mrSellerText.text =  text;
    }

}
