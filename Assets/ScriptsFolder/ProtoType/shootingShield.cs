using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class shootingShield : MonoBehaviour
{
    public Transform Shielder;

    public Vector3 Target;
    public float rotationspeed = 2.0f;
    public float orbitradius = 0.5f;



    Vector3 OrbitPos;
    void Start()
    {
        OrbitPos = (transform.position - Shielder.position).normalized * orbitradius
            + Shielder.position;
        transform.position = OrbitPos;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetdir = Target - Shielder.position;
        targetdir.z = 0;
        targetdir.x *= -1;
        Vector3 neworbitpos = Vector3.RotateTowards(OrbitPos - Shielder.position,
            targetdir, rotationspeed * Time.deltaTime, 0).normalized 
            * orbitradius + Shielder.position;
        transform.position = neworbitpos;
        OrbitPos = neworbitpos;
      //  Vector3 lookdir = Target.position - transform.position;
      //  //lookdir.x = 0;
      //  lookdir.y = 0;
      //  // Vector3 test = new Vector3(0, 0, 180);
      //  //transform.LookAt(test);
      //  Quaternion targetrot = Quaternion.LookRotation(Vector3.forward, lookdir);
      //var a = Quaternion.RotateTowards(transform.rotation, targetrot, rotationspeed * Time.deltaTime );
      //  transform.rotation = Quaternion.Euler(0, 0, a.eulerAngles.z);
    }

   
}
