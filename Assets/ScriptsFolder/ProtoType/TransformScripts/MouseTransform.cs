using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTransform : Player
{
    public GameObject cursor;

    protected override void Awake()
    {
        base.Awake();
        InitMouseForm();
    }

    public void InitMouseForm()
    {
        if(cursor !=null)
            cursor.SetActive(false);
    }

}
