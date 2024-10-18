using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrackingAndPatrol : MonoBehaviour
{

    public EnemyMaterialAndEffect mae;

    [Header("#���� Ȱ��ȭ �ݶ��̴� ť�� ����#")]
    [Tooltip("Ȱ��ȭ �ݶ��̴�")] public GameObject rangeCollider; // ���� ���� �ݶ��̴� ������Ʈ
    [Tooltip("Ȱ��ȭ ����")] public Vector3 rangeSize;
    [Tooltip("Ȱ��ȭ ��ġ")] public Vector3 rangePos;

    [Header("#�÷��̾� Ž�� ť�� ����(�ݶ��̴�)#")]
    [Tooltip("Ž�� �ݶ��̴�")] public GameObject searchCollider; // Ž�� ���� �ݶ��̴�
    [Tooltip("Ž�� ����")] public Vector3 searchColliderRange;
    [Tooltip("Ž�� ��ġ")] public Vector3 searchColliderPos;


    public EnemySearchCollider searchCollider_;
    public AttackColliderRange attackColliderRange;

    //public Transform target; // ������ Ÿ��
    public bool tracking; // ���� Ȱ��ȭ üũ
    public Vector3 testTarget; // Ÿ���� �ٶ󺸴� ������ �׽�Ʈ�ϱ� ���� �ӽ� ����

    [Header("#�߰� ����#")]
    [Tooltip("Ž�� �� �߰� ���� ����")] public float trackingDistance;
    [Tooltip("���� X")] public float disToPlayer;

    [Header("#���� �̵�����(���� �׷�, ������ǥ��, ���� ���ð�)#")]
    //[Header("��ũ��Ʈ �󿡼� ���� ���� �����ϰ� �Ǿ�����")]public Vector3[] patrolGroup; // 0��°: ����, 1��°: ������
    public Vector3 firstPoint;
    public Vector3 secondPoint;
    [Header("��ũ��Ʈ �󿡼� ���� ������")] public Vector3 targetPatrol; // ���� ��ǥ����-> patrolGroup���� ����
    [Tooltip("���� ���ð�")] public float patrolWaitTime; // ���� ���ð�

    [Header("#���� ���� ����#")]
    [Tooltip("���� ���� ����")] public float leftPatrolRange; // ���� ���� ����
    [Tooltip("������ ���� ����")] public float rightPatrolRange; // ���� ���� ����
    [Header("���� �Ÿ�(���� ���ص���)")] public float patrolDistance; // ���� �Ÿ�

    [HideInInspector] public Vector3 leftPatrol, rightPatrol;

    [Header("#�׷��� ���� ť�� ������ ����#")]
    [Tooltip("������ ����")] public float yWidth;
    [Tooltip("������ z�� ����")] public float zWidth;
    [HideInInspector] public Vector3 center;

    [Header("�� üũ ����ĳ��Ʈ")]
    [Tooltip("�� üũ Ray�� ����")] public float wallRayHeight;
    [Tooltip("���� Ray ����")] public float wallRayLength;
    [Tooltip("���� Ray ����")] public float wallRayUpLength;

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
        //LookAt�� �ھƹ����ϱ� �� �������� �ٶ󺸰� ����Ƣ�� ���󶧹��� LookRotation�ھҽ��ϴ�.
        
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

    #region �� üũ
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
                //Debug.Log("ForwardŽ��");
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
                            isWall = false; // �÷��̾���� �Ÿ��� ������ �Ÿ����� ����� ���
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
                //Debug.Log("UpŽ��");
            }

            if (upWall != null)
            {
                if (PlayerHandler.instance != null)
                {
                    if (PlayerHandler.instance.CurrentPlayer != null)
                    {
                        if (transform.position.y < upWall.transform.position.y)
                        {
                            //Debug.Log("�÷��̾�� õ�忡 ���� ����");
                            isWall = false; //�÷��̾� y���� ���� �ٴ��� y�� ���� ���� ������ false
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
    #region �߰�

   public bool PlayerDetected;

    public void TrackingMove_()
    {
        Debug.Log("�÷��̾� �߰�");
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
        Debug.Log("�÷��̾� �߰�");
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
        Debug.Log("���� ����Ʈ �߰�");
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
    // PatrolChange()�� SetPatrolTarget()�� ���� ��Ű�� ����Ͽ� �� ���͸� ��Ȯ�ϰ� �����Ͽ� ������Դϴ�.
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
