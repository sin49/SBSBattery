using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
        soundplayer_=GetComponent<MouseSoundPlayer>();
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
                    //CursorInteraction();
                    CursorFormDeactive();
                }
            }
        }
    }

    public override void Move()
    {
        base.Move();
    }
    bool init;
    direction dir = direction.none;
    directionZ dirZ = directionZ.none;
    public override void rotate(float hori, float vert)
    {
        base.rotate(hori, vert);
        if (hori != 0 || vert != 0)
        {
            if (dir != direction || dirZ != directionz)
            {
                dir = direction;
                dirZ = directionz;
                cursor.InitCursorPos();
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
    public MouseSoundPlayer soundplayer_;
    public void SecondFormActive()
    {
        Destroy(Instantiate(changeEffect, transform.position, Quaternion.identity), 2f);
        for (int i = 0; i < Humonoidanimator.transform.childCount; i++)
        {
            Humonoidanimator.transform.GetChild(i).gameObject.SetActive(false);
        }
        cursor.InitCursorPos();
        secondForm.SetActive(true);
        soundplayer_.FormChangePlay();
    }

    public void SecondFormDeactive()
    {
        if (cursor.onCatch)
            cursor.InteractTypeCheck();

        activeCursor = false;
        Destroy(Instantiate(changeEffect, transform.position, Quaternion.identity), 2f);
        for (int i = 0; i < Humonoidanimator.transform.childCount; i++)
        {
            Humonoidanimator.transform.GetChild(i).gameObject.SetActive(true);
        }
        secondForm.SetActive(false);
        cursor.InitCursorPos();
    }

    public override void Skill1()
    {
        Debug.Log("마우스 스킬 구현해야됨");
    }

    public override bool FormCheck()
    {
        if (cursor.onCatch)
            return true;
        else
            return false;
    }
}
