using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastForward : MonoBehaviour
{
    void Update()
    {
        // I��n y�n� (objenin ileri y�n�)
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        // I��n atma
        Ray ray = new Ray(transform.position, forward);
        RaycastHit hit;

        // I��n bir objeye �arparsa
        if (Physics.Raycast(ray, out hit))
        {
            // �arp�lan objenin ismini konsola yazd�r
            Debug.Log("I��n " + hit.collider.gameObject.name + " objesine �arpt�!");

            // �arpma noktas�n� g�steren bir �izgi �iz
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            // I��n�n ileri y�n�n� g�steren bir �izgi �iz
            Debug.DrawLine(ray.origin, ray.origin + forward * 100, Color.green);
        }
    }
}
