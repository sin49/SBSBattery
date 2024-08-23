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
                //���⿡ �� ��ũ��Ʈ�� �޾Ƽ� �ε��� üũ�ϴ� �׷��� �־�� ��
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
                //���⿡ �� ��ũ��Ʈ�� �޾Ƽ� �ε��� üũ�ϴ� �׷��� �־�� ��
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
