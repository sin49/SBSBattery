using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CctvHeadRotate : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed;
    Enemy enemy;
    public Vector3 vec;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            vec = target.position - enemy.transform.position;
            var b = Quaternion.LookRotation(vec);
           
                var a = Quaternion.RotateTowards(transform.rotation, b, rotateSpeed * Time.deltaTime);


            var c
                   = Quaternion.Euler(0, 0, a.eulerAngles.z);


            transform.rotation = c;
                          
        }
    }
}
