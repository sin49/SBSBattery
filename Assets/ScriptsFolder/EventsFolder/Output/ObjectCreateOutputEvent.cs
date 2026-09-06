using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreateOutputEvent : OutputEvent
{
    public GameObject Object;

    public Transform targettransform;

    public Transform targettransformparent;
    public override void output()
    {
        GameObject instance;
        if (targettransformparent != null)
            instance = Instantiate(Object, targettransformparent);
        else
            instance = Instantiate(Object);
        instance.transform.position = targettransform.position;
        instance.transform.rotation = targettransform.rotation;
       base.output();
    }

}
