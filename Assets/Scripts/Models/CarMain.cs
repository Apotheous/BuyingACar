using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Models
{
    public class CarMain : MonoBehaviour
    {
        public string carName { get; set; }
        //Factors That Will Affect the Sales Value of Vehicles
        public int damagedParts { get; set; }
        public int paintedParts { get; set; }
        public int maxSpeed { get; set; }
        public int torque { get; set; }

        //Factors That Will Not Affect the Sales Value of Vehicles
        public int Suspensions { get; set; }
        public int wheelCamberValues { get; set; }
        public int price { get; set; }
        //Constructor
        public virtual void CarName(string name)
        {
            carName = name;
        }

        public void InitializeRandomValues()
        {
            damagedParts = Random.Range(1, 10);
            paintedParts = Random.Range(1, 10);
            maxSpeed = Random.Range(1, 10);
            torque = Random.Range(1, 10);
            Suspensions = Random.Range(1, 10);
            wheelCamberValues = Random.Range(1, 10);
            price = (damagedParts + paintedParts + maxSpeed + torque) * 1000;
        }

        public override string ToString()
        {
            return $"Car Name From CarMain: {carName}, Damaged Parts: {damagedParts}, Painted Parts: {paintedParts}, Max Speed: {maxSpeed}, Torque: {torque}, Suspensions: {Suspensions}, Wheel Camber Values: {wheelCamberValues}, Price: {price}";
        }

    }

}

