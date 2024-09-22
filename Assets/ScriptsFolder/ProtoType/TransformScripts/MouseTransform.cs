using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTransform : Player
{
    [Header("마우스 커서 부모")] public GameObject cursorParent;
    [Header("마우스 커서")] public MouseFormCursor cursor;
    [Header("마우스 2형태")]public GameObject secondForm;
    bool activeCursor, readyCursor;

    public float cursorCoolTime;
    float cursorCoolTimer;

    protected override void Awake()
    {
        base.Awake();
        InitMouseForm();
    }

    public void InitMouseForm()
    {
        cursor = GetComponentInChildren<MouseFormCursor>(true);
    }

    public override void Attack()
    {
        if (attackInputValue < 1 && !downAttack)
        {
            if (attackBufferTimer > 0)
            {                
                attackBufferTimer = 0;
                attackInputValue = 1;
                if (!activeCursor)
                {
                    CursorFormActive();
                }
                else
                {
                    CursorInteraction();
                }
            }
        }
    }

    public override void PlayerJumpEvent()
    {
        if(!activeCursor)
        base.PlayerJumpEvent();
    }

    public void CursorFormActive()
    {
        readyCursor = true;

        if (cursorParent != null)
        {
            cursorParent.gameObject.SetActive(true);
        }

        if (secondForm != null)
        {
            activeCursor = true;
            SecondFormActive();
        }

        
    }

    public void CursorInteraction()
    {
        if (!readyCursor)
        {
            CursorActive();
        }
        else
        {
            CursorDeactive();
        }
    }

    public void CursorActive()
    {
        cursorParent.gameObject.SetActive(true);
        readyCursor = true;
    }

    public void CursorDeactive()
    {
        if (cursor.onCatch)
            cursor.InteractTypeCheck();
        cursorParent.gameObject.SetActive(false);
        readyCursor = false;
    }

    public void CursorFormDeactive()
    {
        if (cursorParent != null)
        {
            cursorParent.gameObject.SetActive(false);
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
        activeCursor = false;
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
