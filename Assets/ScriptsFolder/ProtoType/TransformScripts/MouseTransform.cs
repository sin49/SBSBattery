using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTransform : Player
{    
    public MouseFormCursor cursor;
    public GameObject secondForm;
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

    public override void PlayerJumpEvent()
    {
        if(!activeCursor)
        base.PlayerJumpEvent();
    }

    public void CursorActive()
    {
        if (cursor != null)
        {
            cursor.gameObject.SetActive(true);
        }

        if (secondForm != null)
        {
            activeCursor = true;
            SecondFormActive();
        }

        
    }

    public void CursorDeactive()
    {
        if (cursor != null)
        {
            cursor.gameObject.SetActive(false);
        }

        if (secondForm != null)
        {
            SecondFormDeactive();
        }
    }

    public void SecondFormActive()
    {
        Destroy(Instantiate(changeEffect, transform.position, Quaternion.identity), 2f);
        for (int i = 0; i < Humonoidanimator.transform.childCount; i++)
        {
            Humonoidanimator.transform.GetChild(i).gameObject.SetActive(false);
        }
        secondForm.SetActive(true);
    }

    public void SecondFormDeactive()
    {
        Destroy(Instantiate(changeEffect, transform.position, Quaternion.identity), 2f);
        for (int i = 0; i < Humonoidanimator.transform.childCount; i++)
        {
            Humonoidanimator.transform.GetChild(i).gameObject.SetActive(true);
        }
        secondForm.SetActive(false);
    }

    public override void Skill1()
    {
        Debug.Log("마우스 스킬 구현해야됨");
    }
}
