using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class objectcollisionevent : InputEvent
{
    public GameObject targetobject;
    public Collider targetcollision;
    bool tf;

    private void Awake()
    {
        isCollisionEventTrigger tmp;
        if (targetcollision.gameObject.GetComponent<isCollisionEventTrigger>())
        {
            tmp = targetcollision.gameObject.GetComponent<isCollisionEventTrigger>();
        }
        else
        {
            tmp = targetcollision.gameObject.AddComponent<isCollisionEventTrigger>();
        }
        tmp.registerEvent(OnCollisionEnterEvent, OnCollisionExitEvent);
    }
    void OnCollisionEnterEvent(Collision other)
    {
        if (targetobject == other.gameObject)
        {
            
                tf = true;
            
        }
      
    }
    void OnCollisionExitEvent(Collision other)
    {
        if (targetobject == other.gameObject)
        {

            tf = false;

        }
    }


    public override bool input(object o = null)
    {
        return tf;
    }

    public override void initialize()
    {
        tf = false;
    }
}
