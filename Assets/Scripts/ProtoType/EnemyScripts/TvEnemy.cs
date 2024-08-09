using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TvColor { white, red, blue}

public class TvEnemy : Enemy
{
    public TvColor tvColor = TvColor.white;

    [Header("Tv 오브젝트 관련")]
    public bool checkTv; // Tv오브젝트를 추격하고 근접했을 때 true(Tv 인식 이후 목표 지점으로 도달했을 때)
    public bool activeTv; // Tv 오브젝트가 활성화 되었을 때 true (활성화 시점)
    [Header("#티비 인식 및 이동 정지를 위한 광선 변수")]
    public float rayRange; // 레이캐스트 길이 조절
    public float rayHeight; // 레이캐스트 높이 조절

    protected override void Awake()
    {
        base.Awake();        
    }

    private void FixedUpdate()
    {
        Move();

        TrackingCheck();
    }
    #region CCTV이동
    public void TrackingCheck()
    {
        Debug.DrawRay(transform.position + Vector3.up * rayHeight, transform.forward * rayRange, Color.magenta, 0.1f);

        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * rayHeight, transform.forward, rayRange);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("GameController"))
            {
                RemoteTV TV;
                if (hits[i].collider != null &&
                    hits[i].collider.TryGetComponent<RemoteTV>(out TV))
                {                    
                    if (TV.onActive)
                    {
                        checkTv = true;
                        rb.constraints = RigidbodyConstraints.FreezePosition |
                    RigidbodyConstraints.FreezeRotation;
                    }
                }
            }
        }
    }

    public override void Move()
    {
        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {
            if (tracking && activeTv)
            {
                if (!activeAttack && !checkTv && !onAttack)
                {
                    testTarget = target.position - transform.position;
                    testTarget.y = 0;

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), eStat.rotationSpeed * Time.deltaTime);

                    if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(testTarget)) < 0.8f)
                    {
                        rb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
                    }
                }
            }
        }
    }
    #endregion
    public override void Attack()
    {
        return;
    }

    public override void Dead()
    {
        return;
    }

    public override void Damaged(float damage)
    {
        return;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("GameController"))
    //    {
    //        RemoteTV TV = null;
    //        if (other.TryGetComponent<RemoteTV>(out TV) && !hitByPlayer)
    //        {
                

    //            if (TV.onActive && TV.tvColor == tvColor)
    //            {
    //                target = other.transform;
    //                tracking = true;
    //            }
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {

            RemoteTV TV = null;
            if (other.TryGetComponent<RemoteTV>(out TV))
            {
                

                if (TV.onActive && TV.tvColor == tvColor)
                {
                    target = other.transform;
                    activeTv = true;
                    tracking = true;

                    rb.constraints = RigidbodyConstraints.FreezeRotation |
                        RigidbodyConstraints.FreezePositionY;

                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!activeTv)
            {
                rb.constraints = RigidbodyConstraints.FreezePosition | 
                    RigidbodyConstraints.FreezeRotation;                 
            }
        }
    }
}
