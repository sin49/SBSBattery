using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HouseholdIronTransform : Player
{
    [Header("돌진 준비시간")]
    public float rushReady;
    [Header("돌진 이동속도")]
    public float rushSpeed;
    float saveSpeed;
    [Header("돌진 지속시간")]
    public float rushTimeMax;
    float rushTimer;
    [HideInInspector] public bool canRushAttack; // 돌진 공격을 받는 주기?를 위한 변수
    bool onRush, startRush, endRush;
    [Header("돌진 재사용 대기시간")]
    public float rushCoolTime;
    float rushCoolTimer;
    [Header("돌진 공격 피해량")]
    public float rushDamage;
    [Header("돌진 공격 주기")]
    public float rushAtkCycle, rushAtkColliderTimeMax;
    float cycleTimer, rushAtkColliderTimer;
    [Header("방향 전환 딜레이 시간")]
    public float rotateTime;
    float rotateTimer;

    public float rushRay;

    bool readyRush, downEnd;

    [Header("특수 능력 관련 변수")]
    public float downAtkSpeed;
    public float ironFlyTime;
    public float downAtkEndTime;
    float downAtkEndTimer;
    public float ironDownDamage;

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
        downAtkEndTimer = downAtkEndTime;
    }

    private void Update()
    {
        BaseBufferTimer();
        CheckRushTime();
        if (downEnd)
        {
            if (downAtkEndTimer > 0)
                downAtkEndTimer -= Time.deltaTime;
            else
            {
                downEnd = false;
                downAtkEndTimer = downAtkEndTime;
                PlayerHandler.instance.CantHandle = false;
            }
        }        
    }

    public override void DownAttack()
    {        
        if (!downAttack)
        {
            downAttack = true;
            StartCoroutine(IronDownAttack());
        }
    }
    IEnumerator IronDownAttack()
    {
        playerRb.useGravity = false;
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(transform.up * 2.5f, ForceMode.Impulse);

        yield return new WaitForSeconds(ironFlyTime);

        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(-transform.up * downAtkSpeed, ForceMode.Impulse);
        downAttackCollider.SetActive(true);
        playerRb.useGravity = true;
    }

    public void CheckRushTime()
    {
        if (onRush)
        {
            //돌진 지속 시간
            if (rushTimer > 0)
            {
                rushTimer -= Time.deltaTime;
            }
            else
            {
                RushEnd();
                rushTimer = rushTimeMax;
            }

            //돌진 공격 주기?
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
        PlayerHandler.instance.CantHandle = true;
        Humonoidanimator.SetTrigger("RushStart");
        onRush = true;
        readyRush = true;
    }

    public void RushEnd()
    {
        PlayerHandler.instance.CantHandle = true;
        Humonoidanimator.SetTrigger("RushEnd");
        onRush = false;
        readyRush = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && downEnd)
        {
            PlayerHandler.instance.CantHandle = true;
        }
    }
}
