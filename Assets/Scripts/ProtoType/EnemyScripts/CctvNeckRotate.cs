using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CctvNeckRotate : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed;
    Enemy enemy;

    public float angleValue;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
  
        if (target != null) {
         
            Vector3 vec = (target.position - transform.position).normalized;
            var a = Quaternion.LookRotation(vec);
            //a.y += 90;
            //a.z -= 90;
            var b = Quaternion.RotateTowards(transform.rotation, a, rotateSpeed * Time.fixedDeltaTime);



            var c = Quaternion.Euler(0, b.eulerAngles.y, 90);
            //c.x -= 90;
            //c.y -= 90;
            //c.z -= 90;
            transform.rotation = c;


            angleValue = Quaternion.Angle(target.rotation, transform.rotation);
        }
    }
}
