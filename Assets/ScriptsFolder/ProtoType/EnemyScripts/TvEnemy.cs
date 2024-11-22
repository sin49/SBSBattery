using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TvColor { white, red, blue}

public class TvEnemy : MonoBehaviour
{
    public TvColor tvColor = TvColor.white;

    [Header("Tv 오브젝트 관련")]
    public bool checkTv; // Tv오브젝트를 추격하고 근접했을 때 true(Tv 인식 이후 목표 지점으로 도달했을 때)
    public bool activeTv; // Tv 오브젝트가 활성화 되었을 때 true (활성화 시점)
    [Header("#티비 인식 및 이동 정지를 위한 광선 변수")]
    public float rayRange; // 레이캐스트 길이 조절
    public float rayHeight; // 레이캐스트 높이 조절
    public Rigidbody rb;
    bool isRotate, move;
    public CharacterSoundPlayer soundplayer;
    public Animator animaor;
    public Transform target;
    bool tracking;
    public Vector3 testTarget;
    float movespeed=2f;
    [Header("목표물과의 거리 계산")] public float distance;
    protected  void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        soundplayer = this.GetComponent<CharacterSoundPlayer>();
        animaor = transform.GetChild(0).GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            Move();
        }

        animaor.SetBool("isMove", move);
    }

    /*private void FixedUpdate()
    {
        Move();

        TrackingCheck();
    }*/
    #region CCTV이동
    float dis;
    public void TrackingCheck()
    {
        Debug.DrawRay(transform.position + Vector3.up * rayHeight, transform.forward * rayRange, Color.magenta, 0.1f);

        Vector3 distance = target.position - transform.position;

        dis = distance.magnitude;

        if (dis <= this.distance)
        {
            checkTv = true;
            tracking = false;
            rb.constraints = RigidbodyConstraints.FreezePosition |
                RigidbodyConstraints.FreezeRotation;
            target = null;
            move = false;
            if (target == null)
                Debug.Log("목표물에 도착해서 null로 변경된 상태임");
        }
        else
        {
            if (!isRotate)
                move = true;
        }

        //hits = Physics.RaycastAll(transform.position + Vector3.up * rayHeight, transform.forward, rayRange);        
        //if (hits != null && hits.Length > 0)
        //{
        //    Debug.Log("콜라이더 받아오고 있음");
        //    for (int i = 0; i < hits.Length; i++)
        //    {
        //        Debug.Log($"호출되고 있지? {hits[i].collider.tag}");
        //        if (hits[i].collider.gameObject.CompareTag("GameController"))
        //        {
        //            Debug.Log("리모컨 상호작용 오브젝트 확인");
        //            RemoteTV TV;
        //            SignalTv sTV;
        //            if (hits[i].collider != null &&
        //                hits[i].collider.TryGetComponent<RemoteTV>(out TV))
        //            {
        //                Debug.Log("TV찾음");
        //                if (TV.onActive)
        //                {
        //                    checkTv = true;
        //                    rb.constraints = RigidbodyConstraints.FreezePosition |
        //                RigidbodyConstraints.FreezeRotation;
        //                    tracking = false;
        //                    Debug.Log("활성화된 TV 찾음");
        //                    target = null;
        //                    if (target == null)
        //                        Debug.Log("타겟이 null로 변경됨");
        //                }
        //            }
        //            else if (hits[i].collider != null && hits[i].collider.TryGetComponent<SignalTv>(out sTV))
        //            {
        //                if (sTV.done)
        //                {
        //                    checkTv = true;
        //                    tracking = false;
        //                }
        //            }
        //            else
        //                Debug.Log("뭔지 모르겠는데?");
        //        }
        //    }
        //}
    }

    public void Move()
    {
      
            if (tracking && activeTv)
            {
                if (!checkTv)
                {
                    testTarget = target.position - transform.position;
                    testTarget.y = 0;

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), 8.0f * Time.deltaTime);

                    if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(testTarget)) < 0.01f)
                    {
                        isRotate = false;
                        rb.MovePosition(transform.position + transform.forward * Time.deltaTime* movespeed);
                 if(soundplayer!=null)
                        soundplayer.PlayMoveSound();
                    }
                    else
                    {
                        isRotate = true;
                        move = false;
                    }
                    animaor.SetBool("isRotate", isRotate);
                }
            }
        
        TrackingCheck();
    }
    #endregion
   

    /*public override void Dead()
    {
        return;
    }*/

 

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

            RemoteTV tv = obj.GetComponentInChildren<RemoteTV>();

            if (tv.onActive && tv.tvColor == tvColor)
            {
                target = tv.transform;
                activeTv = true;
                tracking = true;

                Debug.Log("발견");
            }
            //if (obj.GetComponentInChildren<RemoteTV>() != null)
            //{
            //    RemoteTV TV = obj.GetComponentInChildren<RemoteTV>();
            //    if (TV.onActive && TV.tvColor == tvColor)
            //    {
            //        target = other.transform;
            //        activeTv = true;
            //        tracking = true;

            //        /*rb.constraints = RigidbodyConstraints.FreezeRotation |
            //            RigidbodyConstraints.FreezePositionY;*/
            //        Debug.Log("발견");
            //    }
            //}
            //else if (obj.GetComponentInChildren<SignalTv>() != null)
            //{
            //    SignalTv sTV = obj.GetComponentInChildren<SignalTv>();
            //    if (sTV.done && sTV.tvColor == tvColor)
            //    {
            //        target = sTV.gameObject.transform;
            //        activeTv = true;
            //        tracking = true;
            //    }
            //    Debug.Log($"인지한 오브젝트:{sTV.gameObject}");
            //}

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
