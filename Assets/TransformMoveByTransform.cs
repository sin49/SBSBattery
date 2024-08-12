using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMoveByTransform : MonoBehaviour
{
    public Transform PositionA;
    public Transform PositionB;
    bool OnA;
    // Update is called once per frame
    void Update()
    {
        Vector3 v;
        if (OnA)
        {
            v = PositionB.transform.position - transform.position;

        }
        else
        {
            v = PositionA.transform.position - transform.position;
            
        }
        if (v.magnitude < 1)
            OnA = !OnA;
        v = v.normalized;
        transform.Translate(v * Time.deltaTime * 3);
    }
}
