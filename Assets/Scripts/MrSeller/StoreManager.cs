using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public MrSellerManager mySeller;
    public List<GameObject> myCars = new List<GameObject>();
    public Vector3 spawnOffset = new Vector3(0,0,0);
    public float spawnOffsetX;
    public float spawnOffsetZ;
    void Start()
    {
        mySeller = transform.parent.GetComponent<MrSellerManager>();
        SpawnCars();
    }

    private void SpawnCars()
    {
        for (int i = 0; i < myCars.Count; i++)
        {
            Vector3 carSpawnPoint = new Vector3(
                mySeller.transform.position.x + (spawnOffsetX * i), 
                mySeller.transform.position.y , 
                mySeller.transform.position.z - spawnOffsetZ 
            );

            GameObject car = Instantiate(myCars[i], carSpawnPoint, Quaternion.identity);
            car.transform.SetParent(transform);
            car.name = myCars[i].name;
            car.GetComponent<CarController>().mySeller = mySeller;
            mySeller.ListCarSale.Add(car);

        }
    }
}
