using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionisTriggerEvent : MonoBehaviour,InputEvent
{
    public int index;//인덱스말고 enum이 나을듯?
    bool tf;
   
    private void OnTriggerEnter(Collider other)
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
    private void OnTriggerExit(Collider other)
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
