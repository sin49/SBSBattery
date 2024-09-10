using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HouseholdIronTransform : Player
{
    [Header("(B.J.H)돌진 준비시간")]
    public float rushReady;
    [Header("돌진 이동속도")]
    public float rushSpeed;
    float saveSpeed;
    [Header("돌진 지속시간")]
    public float rushTimeMax, rushAtkColliderTimeMax;
    [HideInInspector] public bool canRushAttack;
    bool onRush, startRush, endRush;
    float rushTimer, rushAtkColliderTimer;
    [Header("돌진 재사용 대기시간")]
    public float rushCoolTime;
    float coolTimer;
    [Header("돌진 공격 피해량")]
    public float rushDamage;
    [Header("돌진 공격 주기")]
    public float rushAtkCycle;
    [Header("방향 전환 딜레이 시간")]
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
