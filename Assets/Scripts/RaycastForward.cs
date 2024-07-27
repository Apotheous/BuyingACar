using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastForward : MonoBehaviour
{
    void Update()
    {
        // Iþýn yönü (objenin ileri yönü)
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        // Iþýn atma
        Ray ray = new Ray(transform.position, forward);
        RaycastHit hit;

        // Iþýn bir objeye çarparsa
        if (Physics.Raycast(ray, out hit))
        {
            // Çarpýlan objenin ismini konsola yazdýr
            Debug.Log("Iþýn " + hit.collider.gameObject.name + " objesine çarptý!");

            // Çarpma noktasýný gösteren bir çizgi çiz
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            // Iþýnýn ileri yönünü gösteren bir çizgi çiz
            Debug.DrawLine(ray.origin, ray.origin + forward * 100, Color.green);
        }
    }
}
