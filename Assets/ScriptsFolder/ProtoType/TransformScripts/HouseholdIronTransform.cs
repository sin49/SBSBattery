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
    public float rushTimeMax, rushAtkColliderTimeMax;
    [HideInInspector] public bool canRushAttack;
    bool onRush, startRush, endRush;
    float rushTimer, rushAtkColliderTimer;
    [Header("���� ���� ���ð�")]
    public float rushCoolTime;
    float coolTimer;
    [Header("���� ���� ���ط�")]
    public float rushDamage;
    [Header("���� ���� �ֱ�")]
    public float rushAtkCycle;
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

    private void Update()
    {
        BaseBufferTimer();

        if (rushTimer > 0)
        {
            rushTimer += Time.deltaTime;
        }
        else
        {
            canRushAttack = true;
            rushTimer = rushTimeMax;
        }

        if (canRushAttack && rushAtkColliderTimer > 0)
        {
            rushAtkColliderTimer -= Time.deltaTime;
        }
        else
        {
            canRushAttack = false;
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
    }
}
