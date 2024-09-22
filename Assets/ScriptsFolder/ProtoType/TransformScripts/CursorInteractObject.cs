using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;

public class CursorInteractObject : MonoBehaviour
{
    public bool caught, thrown;

    public Collider cursorTargetCollider;
    SphereCollider sphere;
    BoxCollider box;

    private void Awake()
    {
        cursorTargetCollider = GetComponent<Collider>();
        if (cursorTargetCollider is SphereCollider)
        {
            sphere = GetComponent<SphereCollider>();
        }
        else if (cursorTargetCollider is BoxCollider)
        {
            box = GetComponent<BoxCollider>();
        }

    }

    public float ColliderEndPoint()
    {
        Vector3 fPoint = Vector3.zero;
        float size=0;
        if (cursorTargetCollider is SphereCollider)
        {
            size = sphere.radius;

            fPoint = transform.forward * size;
        }
        else if (cursorTargetCollider is BoxCollider)
        {
            fPoint = transform.right * (box.size.x / 2);
            size = box.size.x / 2;
        }
        Debug.Log(fPoint);
        return size;
    }
}
