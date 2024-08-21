using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionisTriggerEvent : MonoBehaviour,InputEvent
{
    public int index;//�ε������� enum�� ������?
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
                //���⿡ �� ��ũ��Ʈ�� �޾Ƽ� �ε��� üũ�ϴ� �׷��� �־�� ��
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
                //���⿡ �� ��ũ��Ʈ�� �޾Ƽ� �ε��� üũ�ϴ� �׷��� �־�� ��
            }
        }
    }

    public bool input(object o=null)
    {
        return tf;
    }
}
