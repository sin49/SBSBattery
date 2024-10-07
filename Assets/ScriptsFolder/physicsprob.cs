using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsprob : MonoBehaviour,environmentObject
{
    [Header("Áú·®")]
    public float mass;
    Rigidbody rb;
    Vector3 EnvironmentForce;

    public void AddEnviromentPower(Vector3 power)
    {
        EnvironmentForce = power;
    }

   
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(rb==null)
        {
            rb=gameObject.AddComponent<Rigidbody>();

        }
        rb.mass = mass;
       
        gameObject.layer = 14;
    }
    private void Update()
    {
        rb.mass = mass;

        if (EnvironmentForce != Vector3.zero)
        {
            rb.AddForce(EnvironmentForce, ForceMode.VelocityChange);
            EnvironmentForce = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }
}
