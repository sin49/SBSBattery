using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTransform : Player
{    
    public GameObject cursor;
    bool activeCursor;


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

    public override void Attack()
    {
        if (attackInputValue < 1)
        {
            if (!cursor)
                CursorActive();
            else
                CursorDeactive();
        }
    }

    public void CursorActive()
    {
        if (cursor != null)
        {
            cursor.SetActive(true);
        }
    }

    public void CursorDeactive()
    {
        if (cursor != null)
        {
            cursor.SetActive(false);
        }
    }
}
