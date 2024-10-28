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

    bool isRotate;

    public Transform target;
    bool tracking;
    public Vector3 testTarget;

    protected override void Awake()
    {
        base.Awake();        
    }

    /*private void FixedUpdate()
    {
        Move();

        TrackingCheck();
    }*/
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
                SignalTv sTV;
                if (hits[i].collider != null &&
                    hits[i].collider.TryGetComponent<RemoteTV>(out TV))
                {                    
                    if (TV.onActive)
                    {
                        checkTv = true;
                        rb.constraints = RigidbodyConstraints.FreezePosition |
                    RigidbodyConstraints.FreezeRotation;
                        tracking = false;
                    }
                }
                else if(hits[i].collider != null && hits[i].collider.TryGetComponent<SignalTv>(out sTV))
                {
                    if (sTV.done)
                    {
                        checkTv = true;
                        tracking = false;
                    }
                }
            }
        }
    }

    public override void Move()
    {
        if (!die || !hitted)
        {
            if (tracking && activeTv)
            {
                if (!activeAttack && !checkTv )
                {
                    testTarget = target.position - transform.position;
                    testTarget.y = 0;

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), 4.0f * Time.deltaTime);

                    if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(testTarget)) < 0.8f)
                    {
                        isRotate = false;
                        rb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
                 if(soundplayer!=null)
                        soundplayer.PlayMoveSound();
                    }
                    else
                    {
                        isRotate = true;
                    }
                    animaor.SetBool("isRotate", isRotate);
                }
            }
        }
        TrackingCheck();
    }
    #endregion
    public override void Attack()
    {
        return;
    }

    /*public override void Dead()
    {
        return;
    }*/

    public override void Damaged(float damage)
    {
        base.Damaged(damage);
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
            Debug.Log("TV 활성화 콜라이더 감지?");
            GameObject obj = other.transform.parent.gameObject;
            if (obj.GetComponentInChildren<RemoteTV>() != null)
            {
                RemoteTV TV = obj.GetComponentInChildren<RemoteTV>();
                if (TV.onActive && TV.tvColor == tvColor)
                {
                    target = other.transform;
                    activeTv = true;
                    tracking = true;

                    /*rb.constraints = RigidbodyConstraints.FreezeRotation |
                        RigidbodyConstraints.FreezePositionY;*/
                }
            }
            else if (obj.GetComponentInChildren<SignalTv>() != null)
            {
                SignalTv sTV = obj.GetComponentInChildren<SignalTv>();
                if (sTV.done && sTV.tvColor == tvColor)
                {
                    target = sTV.gameObject.transform;
                    activeTv = true;
                    tracking = true;
                }
                Debug.Log($"인지한 오브젝트:{sTV.gameObject}");
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!activeTv)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
    }
}
