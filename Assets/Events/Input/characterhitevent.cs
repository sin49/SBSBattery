using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterhitevent : MonoBehaviour,InputEvent
{
    public bool hit;
    public Character character;
    [Header("���� �ε��� ���� �� ��")]
    public int index;
    public bool input(object o = null)
    {
        return hit;
    }
    private void Start()
    {
        character.registerhittedevent(hiteventinvoke);
    }
    void hiteventinvoke()
    {
        hit = true;
    }
}
