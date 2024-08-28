using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class characterhitevent : InputEvent
{
    public bool hit;
    public Character character;
    [Header("¾ÆÁ÷ ÀÎµ¦½º Áö¿ø ¾È µÊ")]
    public int index;
    public override bool input(object o = null)
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

    public override void initialize()
    {
        hit = false;   
    }
}
