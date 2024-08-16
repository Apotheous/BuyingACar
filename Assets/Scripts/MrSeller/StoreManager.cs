using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public MrSellerManager mySeller;
    public List<GameObject> myCars = new List<GameObject>();

    public float spawnOffsetX;
    public float spawnOffsetZ;
    void Start()
    {
        SpawnCars();
    }

    private void SpawnCars()
    {
        for (int i = 0; i < myCars.Count; i++)
        {
            if (i != 0)
            {
                int mod;
                mod = i % 2;
                if (mod == 0)
                {
                    spawnOffsetX = spawnOffsetX * -1;
                    spawnOffsetX += 5;
                }
                else if (mod != 0)
                {
                    spawnOffsetX = -(spawnOffsetX);
                
                }
            }
            Vector3 carSpawnPoint = new Vector3(
            mySeller.transform.position.x + (spawnOffsetX),
            mySeller.transform.position.y,
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
