using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrackingAndPatrol : MonoBehaviour
{

    public EnemyMaterialAndEffect mae;

    [Header("#공격 활성화 콜라이더 큐브 조정#")]
    [Tooltip("활성화 콜라이더")] public GameObject rangeCollider; // 공격 범위 콜라이더 오브젝트
    [Tooltip("활성화 범위")] public Vector3 rangeSize;
    [Tooltip("활성화 위치")] public Vector3 rangePos;

    [Header("#플레이어 탐색 큐브 조정(콜라이더)#")]
    [Tooltip("탐색 콜라이더")] public GameObject searchCollider; // 탐지 범위 콜라이더
    [Tooltip("탐색 범위")] public Vector3 searchColliderRange;
    [Tooltip("탐색 위치")] public Vector3 searchColliderPos;


    public EnemySearchCollider searchCollider_;
    public AttackColliderRange attackColliderRange;

    //public Transform target; // 추적할 타겟
    public bool tracking; // 추적 활성화 체크
    public Vector3 testTarget; // 타겟을 바라보는 시점을 테스트하기 위한 임시 변수

    [Header("#추격 범위#")]
    [Tooltip("탐색 후 추격 유지 범위")] public float trackingDistance;
    [Tooltip("설정 X")] public float disToPlayer;

    [Header("#정찰 이동관련(정찰 그룹, 정찰목표값, 정찰 대기시간)#")]
    //[Header("스크립트 상에서 값을 임의 결정하게 되어있음")]public Vector3[] patrolGroup; // 0번째: 왼쪽, 1번째: 오른쪽
    public Vector3 firstPoint;
    public Vector3 secondPoint;
    [Header("스크립트 상에서 값이 결정됨")] public Vector3 targetPatrol; // 정찰 목표지점-> patrolGroup에서 지정
    [Tooltip("정찰 대기시간")] public float patrolWaitTime; // 정찰 대기시간

    [Header("#정찰 범위 관련#")]
    [Tooltip("왼쪽 정찰 범위")] public float leftPatrolRange; // 좌측 정찰 범위
    [Tooltip("오른쪽 정찰 범위")] public float rightPatrolRange; // 우측 정찰 범위
    [Header("정찰 거리(설정 안해도됨)")] public float patrolDistance; // 정찰 거리

    [HideInInspector] public Vector3 leftPatrol, rightPatrol;

    [Header("#그려질 정찰 큐브 사이즈 결정#")]
    [Tooltip("붉은색 높이")] public float yWidth;
    [Tooltip("붉은색 z축 넓이")] public float zWidth;
    [HideInInspector] public Vector3 center;

    [Header("벽 체크 레이캐스트")]
    [Tooltip("벽 체크 Ray의 높이")] public float wallRayHeight;
    [Tooltip("정면 Ray 길이")] public float wallRayLength;
    [Tooltip("위쪽 Ray 길이")] public float wallRayUpLength;

    Collider forwardWall;
    Collider upWall;
    float disToWall;
    public bool wallCheck;
    bool forwardCheck, upCheck;
    public PatrolType patrolType;
    public void InitPatrolPoint()
    {

        SetPoint();
    }
    public Vector3 GetTarget()
    {

        if (PlayerDetected)
        {
            TrackingMove();
        }
        else
        {
         return   PatrolTracking();
        }

        return testTarget;
        //LookAt을 박아버리니까 위 방향으로 바라보고 통통튀는 현상때문에 LookRotation박았습니다.
        
    }

    private void Awake()
    {
    
        InitPatrolPoint();
        if (patrolType == PatrolType.movePatrol)
        {
            //InitPatrolPoint();
            if (!PlayerDetected)
                StartCoroutine(InitPatrolTarget());
        }


    }

    private void Update()
    {
        if (rangeCollider != null)
        {
            rangeCollider.GetComponent<BoxCollider>().center = rangePos;
            rangeCollider.GetComponent<BoxCollider>().size = rangeSize;
        }
       
    }
    private void FixedUpdate()
    {
        
    }


    public void SetPoint()
    {
        firstPoint = new(transform.position.x - leftPatrolRange, transform.position.y, transform.position.z);
        secondPoint = new(transform.position.x + rightPatrolRange, transform.position.y, transform.position.z);

        leftPatrol = firstPoint;
        rightPatrol = secondPoint;
       
    }

    private void OnDrawGizmos()
    {

        if (searchCollider != null)
        {
            searchCollider.GetComponent<BoxCollider>().size = searchColliderRange;
            searchCollider.GetComponent<BoxCollider>().center = searchColliderPos;

            if (searchCollider.transform.childCount != 0)
            {
                searchCollider.transform.GetChild(0).localScale = searchColliderRange;
                searchCollider.transform.GetChild(0).localPosition = searchColliderPos;
            }
        }


        if (rangeCollider != null)
        {
            rangeCollider.GetComponent<BoxCollider>().size = rangeSize;
            rangeCollider.GetComponent<BoxCollider>().center = rangePos;

            if (rangeCollider.transform.childCount != 0)
            {
                rangeCollider.transform.GetChild(0).localScale = rangeSize;
                rangeCollider.transform.GetChild(0).localPosition = rangePos;
            }

        }
    }

    #region 벽 체크
    public void ForwardWallRayCheck()
    {
        bool isWall = false;
        forwardWall = null;
        Debug.DrawRay(transform.position + Vector3.up * wallRayHeight, transform.forward * wallRayLength, Color.magenta, 0.02f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * wallRayHeight, transform.forward, out hit, wallRayLength, LayerMask.GetMask("Platform")))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                forwardWall = hit.collider;
                isWall = true;
                //Debug.Log("Forward탐지");
            }

            if (forwardWall != null)
            {
                Vector3 targetWall = forwardWall.transform.position - transform.position;
                disToWall = targetWall.magnitude;
                if (PlayerHandler.instance != null)
                {
                    if (PlayerHandler.instance.CurrentPlayer != null)
                    {
                        if (disToPlayer < disToWall)
                        {
                            isWall = false; // 플레이어와의 거리가 벽과의 거리보다 가까울 경우
                        }
                        else
                        {
                            isWall = true;
                        }
                    }
                }
            }
        }

        if (forwardWall == null)
        {
            isWall = false;
        }

        forwardCheck = isWall;
    }

    public void UpWallRayCheck()
    {
        bool isWall = false;
        upWall = null;
        Debug.DrawRay(transform.position + Vector3.up * wallRayHeight, transform.up * wallRayUpLength, Color.magenta, 0.02f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * wallRayHeight, transform.up, out hit, wallRayUpLength, LayerMask.GetMask("Platform")))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                upWall = hit.collider;
                isWall = true;
                //Debug.Log("Up탐지");
            }

            if (upWall != null)
            {
                if (PlayerHandler.instance != null)
                {
                    if (PlayerHandler.instance.CurrentPlayer != null)
                    {
                        if (transform.position.y < upWall.transform.position.y)
                        {
                            //Debug.Log("플레이어는 천장에 있지 않음");
                            isWall = false; //플레이어 y축이 위쪽 바닥의 y축 보다 값이 작으면 false
                        }
                        else
                        {
                            isWall = true;
                            if (PlayerDetected)
                            {
                                PlayerDetected = false;
                      
                            }
                        }
                    }
                }
            }
        }

        if (upWall == null)
        {
            isWall = false;
        }

        upCheck = isWall;
    }

    public void WallCheckResult()
    {
        if (forwardCheck || upCheck || forwardCheck && upCheck)
        {
            YesWallCheck();
        }
        else
        {
            NoWallCheck();
        }
    }

    public void YesWallCheck()
    {
        wallCheck = true;

    }

    public void NoWallCheck()
    {
        wallCheck = false;

    }
    #endregion
    #region 추격

   public bool PlayerDetected;

    public void TrackingMove_()
    {
        Debug.Log("플레이어 추격");
        testTarget = PlayerHandler.instance.CurrentPlayer.transform.position - transform.position;
        testTarget.y = 0;
        disToPlayer = testTarget.magnitude;

       
        if (disToPlayer > trackingDistance /*|| f > 6*/)
        {
            PlayerDetected = false;

        }
    }
    public virtual Vector3 TrackingMove()
    {
        Debug.Log("플레이어 추격");
        testTarget = PlayerHandler.instance.CurrentPlayer.transform.position - transform.position;
        testTarget.y = 0;
        disToPlayer = testTarget.magnitude;

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), eStat.rotationSpeed * Time.deltaTime);

        //if (SetRotation())
        //{
        //    //enemymovepattern();

        //}

        if (disToPlayer > trackingDistance /*|| f > 6*/)
        {
            PlayerDetected = false;
            
        }
        return testTarget;
    }

    public virtual Vector3 PatrolTracking()
    {
        Debug.Log("정찰 포인트 추격");
       Vector3 testTarget = targetPatrol - transform.position;
        testTarget.y = 0;

       
        if (testTarget.magnitude < patrolDistance)
        {
            tracking = false;
            StartCoroutine(InitPatrolTarget());
        }
        return testTarget;

    }

    bool setPatrol;
    // PatrolChange()랑 SetPatrolTarget()은 제외 시키고 요약하여 두 벡터만 정확하게 선언하여 사용중입니다.
    public IEnumerator InitPatrolTarget()
    {
        yield return new WaitForSeconds(patrolWaitTime);

        if (targetPatrol == firstPoint)
            targetPatrol = secondPoint;
        else
            targetPatrol = firstPoint;        

        tracking = true;
    }

    //public virtual void PatrolChange()
    //{

    //    if (patrolGroup.Length >= 2)
    //    {
    //        patrolGroup[0].x = leftPatrol.x - leftPatrolRange;
    //        patrolGroup[0].y = transform.position.y;
    //        patrolGroup[0].z = transform.position.z;

    //        patrolGroup[1].x = rightPatrol.x + rightPatrolRange;
    //        patrolGroup[1].y = transform.position.y;
    //        patrolGroup[1].z = transform.position.z;
    //    }
    //}

    //public bool SetPatrolTarget()
    //{
    //    int randomTarget = Random.Range(0, patrolGroup.Length);
    //    if (patrolGroup.Length >= 2)
    //    {
    //        if (targetPatrol == patrolGroup[randomTarget])
    //        {
    //            setPatrol = true;
    //        }
    //        else
    //        {
    //            targetPatrol = patrolGroup[randomTarget];
    //            setPatrol = false;
    //        }
    //    }

    //    return setPatrol;
    //}

    //public virtual bool SetRotation()
    //{
    //    bool completeRot = false;
    //    if (PlayerHandler.instance != null && !mae.searchCollider.onPatrol)
    //    {
    //        Vector3 targetTothis = PlayerHandler.instance.CurrentPlayer.transform.position - transform.position;
    //        targetTothis.y = 0;
    //        Quaternion q = Quaternion.LookRotation(targetTothis);
    //        float testAngle = Quaternion.Angle(transform.rotation, q);
    //        if (testAngle < 45f)
    //            completeRot = true;
    //    }
    //    else if (mae.searchCollider.onPatrol)
    //    {
    //        Vector3 patrolTothis = targetPatrol - transform.position;
    //        patrolTothis.y = 0;
    //        Quaternion q = Quaternion.LookRotation(patrolTothis);
    //        float testAngle = Quaternion.Angle(transform.rotation, q);
    //        if (testAngle < 1.5f)
    //            completeRot = true;
    //    }
    //    return completeRot;

    //}
    #endregion
}
