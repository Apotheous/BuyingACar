using System.Collections;
using System.Collections.Generic;
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

    }
}

