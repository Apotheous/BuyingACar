using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealisticCarStopping : MonoBehaviour
{
    public Rigidbody rb;
    public float frictionCoefficient = 0.015f; // Sürtünme katsayısı (μ)
    public float airDensity = 1.225f; // Hava yoğunluğu (ρ) kg/m^3
    public float dragCoefficient = 0.3f; // Sürüklenme katsayısı (Cd)
    public float frontalArea = 2.2f; // Aracın ön yüzey alanı (A) m^2
    public float dragScale = 0.01f; // Hava direnci ölçeği

    void Start()
    {
        rb.drag = 0; // Başlangıçta sürükleme direncini sıfır yapıyoruz
    }

    void Update()
    {
        float speed = rb.velocity.magnitude;
        float normalForce = rb.mass * Physics.gravity.magnitude;
        float frictionForce = frictionCoefficient * normalForce;
        float airResistance = 0.5f * airDensity * frontalArea * dragCoefficient * speed * speed;

        float totalDecelerationForce = frictionForce + airResistance;
        float deceleration = totalDecelerationForce / rb.mass;

        Vector3 decelerationVector = -rb.velocity.normalized * deceleration;

        // İvme kuvveti uygulayarak yavaşlatma
        rb.AddForce(decelerationVector, ForceMode.Acceleration);

        // Hava direncini hesaplayarak drag değerini ayarlama
        rb.drag = dragScale * airResistance / speed;
    }
}
