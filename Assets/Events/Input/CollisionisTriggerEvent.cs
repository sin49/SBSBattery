using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class CollisionisTriggerEvent : InputEvent
{
    public int index;//�ε������� enum�� ������?
    public Collider targetcollider;

    bool tf;
    private void Awake()
    {
        isTriggerEventHandler b = null;
        if (targetcollider.GetComponent<isTriggerEventHandler>())
        {
            b = targetcollider.GetComponent<isTriggerEventHandler>();

        }
        else
        {
            b = targetcollider.AddComponent<isTriggerEventHandler>();
        }

        b.registerEvent(TriggerEnterEvent, TriggerExitEvent);
    }
   
    void TriggerEnterEvent(Collider other)
    {
        if (index == 1)
        {
            if (other.CompareTag("Player"))
            {
                tf = true;
            }
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                tf = true;
                //���⿡ �� ��ũ��Ʈ�� �޾Ƽ� �ε��� üũ�ϴ� �׷��� �־�� ��
            }
        }
    }
    void TriggerExitEvent(Collider other)
    {
        if (index == 1)
        {
            if (other.CompareTag("Player"))
            {
                tf = false;
            }
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                tf = false;
                //���⿡ �� ��ũ��Ʈ�� �޾Ƽ� �ε��� üũ�ϴ� �׷��� �־�� ��
            }
        }
    }
   
    public override bool input(object o=null)
    {
        return tf;
    }

    public override void initialize()
    {
        tf = false;
    }
}
