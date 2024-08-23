using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectTriggerInputEvent : MonoBehaviour,InputEvent
{
    public GameObject targetObject;
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
        if (targetObject == other.gameObject)
        {
            tf = true;
        }
       
    }
    void TriggerExitEvent(Collider other)
    {
        if (targetObject == other.gameObject)
        {
            tf = false;
        }
    }

    public bool input(object o = null)
    {
        return tf;
    }
}
