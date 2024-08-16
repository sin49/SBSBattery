using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Crane : RemoteObject
{
    public GameObject MoveObject;
    public float CraneSpeed;

    public Transform ActiveTransform;
Vector3 DeActiveTransform;
    Vector3 MoveVector;
    protected override void Awake()
    {
        DeActiveTransform = MoveObject.transform.position;
    }
    public override void Active()
    {
        Debug.Log("active()");
        if (onActive)
        {
            Deactive();
            return;
        }
        onActive = true;
        base.Active();
    }

    public override void Deactive()
    {
        Debug.Log("Deactive()");
        onActive = false;
        base.Deactive();
    }
    public abstract Vector3 GetMoveVector(Vector3 Target, Vector3 origin);
    public abstract void MoveCrane(Vector3 vector, Vector3 Target, Transform origin);


        private void FixedUpdate()
    {
        if (onActive)
        {
    
            MoveVector = GetMoveVector(ActiveTransform.position, MoveObject.transform.position);
            MoveCrane(MoveVector, ActiveTransform.position, MoveObject.transform);
        }
        else
        {

            MoveVector = GetMoveVector(DeActiveTransform, MoveObject.transform.position);
            MoveCrane(MoveVector, DeActiveTransform, MoveObject.transform);
        }
    }

}
