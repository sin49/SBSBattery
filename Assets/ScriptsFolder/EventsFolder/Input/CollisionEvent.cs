using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CollisionEvent :  InputEvent
{
    public int index;
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
        if (index == 1)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                tf = true;
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                tf = true;
                //여기에 적 스크립트를 받아서 인덱스 체크하는 그런거 있어야 함
            }
        }
    }
     void OnCollisionExitEvent(Collision other)
    {
        if (index == 1)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                tf = false;
            }
 
        }
        else
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                tf = false;
                //여기에 적 스크립트를 받아서 인덱스 체크하는 그런거 있어야 함
            }
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
