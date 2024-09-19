using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTransform : Player
{    
    public MouseFormCursor cursor;
    bool activeCursor;


    protected override void Awake()
    {
        base.Awake();
        InitMouseForm();
    }

    public void InitMouseForm()
    {
        cursor = GetComponentInChildren<MouseFormCursor>();
    }

    public override void Attack()
    {
        if (attackInputValue < 1 && attackBufferTimer > 0)
        {            
            attackInputValue = 1;
            attackBufferTimer = 0;
            if (!activeCursor)
                CursorActive();
            else
                CursorDeactive();
        }
    }

    public void CursorActive()
    {
        if (cursor != null)
        {
            cursor.gameObject.SetActive(true);
        }
    }

    public void CursorDeactive()
    {
        if (cursor != null)
        {
            cursor.gameObject.SetActive(false);
        }
    }

    public override void Skill1()
    {
        Debug.Log("마우스 스킬 구현해야됨");
    }
}
