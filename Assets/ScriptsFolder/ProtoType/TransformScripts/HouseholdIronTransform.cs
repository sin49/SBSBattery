using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HouseholdIronTransform : Player
{
    [Header("(B.J.H)���� �غ�ð�")]
    public float rushReady;
    [Header("���� �̵��ӵ�")]
    public float rushSpeed;
    float saveSpeed;
    [Header("���� ���ӽð�")]
    public float rushTimeMax;
    float rushTimer;
    [HideInInspector] public bool canRushAttack; // ���� ������ �޴� �ֱ�?�� ���� ����
    bool onRush, startRush, endRush;
    [Header("���� ���� ���ð�")]
    public float rushCoolTime;
    float rushCoolTimer;
    [Header("���� ���� ���ط�")]
    public float rushDamage;
    [Header("���� ���� �ֱ�")]
    public float rushAtkCycle, rushAtkColliderTimeMax;
    float cycleTimer, rushAtkColliderTimer;
    [Header("���� ��ȯ ������ �ð�")]
    public float rotateTime;
    float rotateTimer;

    public float rushRay;

    bool readyRush;

    protected override void Awake()
    {
        base.Awake();
        saveSpeed = PlayerStat.instance.moveSpeed;
    }

    public void InitTimer()
    {
        rushCoolTimer = rushCoolTime;
        rotateTimer = rotateTime;
        cycleTimer = rushAtkCycle;
        rushAtkColliderTimer = rushAtkColliderTimeMax;
    }

    private void Update()
    {
        BaseBufferTimer();
        CheckRushTime();
    }
    
    public void CheckRushTime()
    {
        if (onRush)
        {
            //���� ���� �ð�
            if (rushTimer > 0)
            {
                rushTimer -= Time.deltaTime;
            }
            else
            {
                RushEnd();
                rushTimer = rushTimeMax;
            }

            //���� ���� �ֱ�?
            if (canRushAttack && rushAtkColliderTimer > 0)
            {
                rushAtkColliderTimer -= Time.deltaTime;
            }
            else
            {
                canRushAttack = false;
                rushAtkColliderTimer = rushAtkColliderTimeMax;
            }
        }

        if (!readyRush)
        {
            if (rushTimer > 0)
                rushTimer -= Time.deltaTime;
            else
            {
                readyRush = true;
                rushTimer = rushTimeMax;
            }
        }
    }

    public override void Attack()
    {
        if (PlayerHandler.instance.onAttack && attackInputValue < 1)
        {
            if (attackBufferTimer > 0 && readyRush)
            {
                if (!onRush)
                {
                    RushStart();
                }
                else
                {
                    RushEnd();
                }

                if (Humonoidanimator != null)
                {                    
                    Humonoidanimator.SetBool("Rush", onRush);
                }
            }
        }
    }

    public void RushStart()
    {
        Humonoidanimator.SetTrigger("RushStart");
        onRush = true;
    }

    public void RushEnd()
    {
        Humonoidanimator.SetTrigger("RushEnd");
        onRush = false;
        readyRush = false;

        PlayerHandler.instance.CantHandle = true;
    }
}
