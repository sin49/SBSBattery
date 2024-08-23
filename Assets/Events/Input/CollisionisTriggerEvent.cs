using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionisTriggerEvent : MonoBehaviour,InputEvent
{
    public int index;//인덱스말고 enum이 나을듯?
    public Collider targetcollider;

    bool tf;
    private void Awake()
    {
     
            isTriggerEventHandler b=null;
            if (targetcollider.GetComponent<isTriggerEventHandler>())
            {
               b= targetcollider.GetComponent<isTriggerEventHandler>();
               
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
                //여기에 적 스크립트를 받아서 인덱스 체크하는 그런거 있어야 함
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
                //여기에 적 스크립트를 받아서 인덱스 체크하는 그런거 있어야 함
            }
        }
    }
   
    public bool input(object o=null)
    {
        return tf;
    }
}
