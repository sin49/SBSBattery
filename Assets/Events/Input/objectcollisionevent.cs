using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectcollisionevent : MonoBehaviour
{
    public GameObject targetobject;
    public Collision targetcollision;
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


    public bool input(object o = null)
    {
        return tf;
    }
}
