using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsprob : MonoBehaviour
{
    [Header("Áú·®")]
    public float mass;
    Rigidbody rb;
    Vector3 EnvironmentForce;

    public void getenvironmentforce(Vector3 v)
    {
        EnvironmentForce = v;
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

        rb.AddForce(EnvironmentForce,ForceMode.VelocityChange);
        EnvironmentForce = Vector3.zero;
    }
}
