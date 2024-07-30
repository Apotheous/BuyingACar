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
        public int generalPropValue { get; set; }
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
            torque = Random.Range(5, 10);

            maxSpeed = Random.Range(5, 15);

            damagedParts = Random.Range(0, 5);

            paintedParts = Random.Range(1, 10);

            Suspensions = Random.Range(0, 3);

            wheelCamberValues = Random.Range(1, 10);

            generalPropValue = damagedParts + paintedParts + maxSpeed + torque;

            price = (50000) ;
        }

        public override string ToString()
        {
            return $"Car Name From CarMain: {carName}, Damaged Parts: {damagedParts}" +
                $", Painted Parts: {paintedParts}, Max Speed: {maxSpeed}, Torque: {torque}" +
                $", Suspensions: {Suspensions}, Wheel Camber Values: {wheelCamberValues}, Price: {price}";
        }
    }
}

