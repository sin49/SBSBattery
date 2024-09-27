using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingtest : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    Transform targettransform;
    void Start()
    {
        targettransform = point1;
        transform.LookAt(targettransform);
    }

    // Update is called once per frame
    void Update()
    {
        if ((targettransform.position - transform.position).magnitude < 2) {
            if (targettransform == null || targettransform == point2)
            {
                targettransform = point1;
                transform.LookAt(targettransform);
            }

            else
            {
                targettransform = point2;
                targettransform.LookAt(targettransform);
            }
           
                }
     
        transform.Translate((Vector3.forward) * 3 * Time.deltaTime);
    }
}
