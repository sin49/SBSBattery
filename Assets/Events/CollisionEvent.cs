using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    public int index;//�ε������� enum�� ������?
    bool tf;
    private void OnCollisionEnter(Collision other)
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
    private void OnCollisionExit(Collision other)
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


    public bool input(object o = null)
    {
        return tf;
    }
}
