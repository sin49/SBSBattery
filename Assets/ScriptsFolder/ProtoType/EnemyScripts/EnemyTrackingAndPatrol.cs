using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyTrackingAndPatrol : MonoBehaviour
{

    public EnemyMaterialAndEffect mae;

    [Header("#���� Ȱ��ȭ �ݶ��̴� ť�� ����#")]
    [Header("Ȱ��ȭ �ݶ��̴�")] public GameObject rangeCollider; // ���� ���� �ݶ��̴� ������Ʈ

    [Header("Ȱ��ȭ ����")]
    [Range(0, 10)] public float rangeSizeX;
    [Range(0, 10)] public float rangeSizeY;
    [Range(0, 10)] public float rangeSizeZ;

    [Header("Ȱ��ȭ ��ġ")]
    [Range(0, 30)] public float rangePosX;
    [Range(0, 30)] public float rangePosY; 
    [Range(0, 30)] public float rangePosZ;

    [Header("#�÷��̾� Ž�� ť�� ����(�ݶ��̴�)#")]
    [Header("Ž�� �ݶ��̴�")] public GameObject searchCollider; // Ž�� ���� �ݶ��̴�

    [Header("Ž�� ����")]
    [Range(0, 10)] public float searchSizeX;
    [Range(0, 10)] public float searchSizeY;
    [Range(0, 10)] public float searchSizeZ;

    [Header("Ž�� ��ġ")]
    [Range(0, 30)] public float searchPosX;
    [Range(0, 30)] public float searchPosY;
    [Range(0, 30)] public float searchPosZ;


    [Header("Ž���ݶ��̴�")]public EnemySearchCollider searchCollider_;
    [Header("���� Ȱ��ȭ �ݶ��̴�")]public AttackColliderRange attackColliderRange;

    //public Transform target; // ������ Ÿ��
    public bool tracking; // ���� Ȱ��ȭ üũ
    public Vector3 testTarget; // Ÿ���� �ٶ󺸴� ������ �׽�Ʈ�ϱ� ���� �ӽ� ����

    [Header("#�߰� ����#")]
    [Header("Ž�� �� �߰� ���� ����")] public float trackingDistance;
    [Tooltip("���� X")] public float disToPlayer;

    [Header("#���� �̵�����(���� �׷�, ������ǥ��, ���� ���ð�)#")]
    //[Header("��ũ��Ʈ �󿡼� ���� ���� �����ϰ� �Ǿ�����")]public Vector3[] patrolGroup; // 0��°: ����, 1��°: ������
    public Vector3 firstPoint;
    public Vector3 secondPoint;
    [Header("��ũ��Ʈ �󿡼� ���� ������")] public Vector3 targetPatrol; // ���� ��ǥ����-> patrolGroup���� ����
    [Tooltip("���� ���ð�")]
    [Range(0, 1)]public float patrolWaitTime; // ���� ���ð�

    [Header("#���� ���� ����#")]
    [Header("���� ���� ����")] 
    [Range(0, 5)]public float leftPatrolRange; // ���� ���� ����
    [Header("������ ���� ����")]
    [Range(0, 5)] public float rightPatrolRange; // ���� ���� ����
    [Header("���� �Ÿ�(�ּ� 0.1)")]
    [Range(0, 5)] public float patrolDistance; // ���� �Ÿ�

    [HideInInspector] public Vector3 leftPatrol, rightPatrol;    
    [HideInInspector] public Vector3 center;

    [Header("�� üũ ����ĳ��Ʈ")]
    [Header("�� üũ Ray�� ����")] [Range(0,10)] public float wallRayHeight;
    [Header("���� Ray ����")] [Range(0, 10)] public float wallRayLength;
    [Header("���� Ray ����")] [Range(0, 10)] public float wallRayUpLength;
    [Header("���� Ray ����")] [Range(0, 10)] public float wallRayBackLength;
    Collider forwardWall;
    Collider upWall;
    Collider backWall;
    float disToWall;
    [HideInInspector] public bool wallCheck;
    bool forwardCheck, upCheck, backCheck;
     
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
    }

    private void Update()
    {
        if (rangeCollider != null)
        {
            rangeCollider.GetComponent<BoxCollider>().center = new(rangePosX, rangePosY, rangePosZ);
            rangeCollider.GetComponent<BoxCollider>().size = new(rangeSizeX, rangeSizeY, rangeSizeZ);
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
            searchCollider.GetComponent<BoxCollider>().size = new(searchSizeX, searchSizeY, searchSizeZ);
            searchCollider.GetComponent<BoxCollider>().center = new(searchPosX, searchPosY, searchPosZ);

            if (searchCollider.transform.childCount != 0)
            {
                searchCollider.transform.GetChild(0).localScale = new(searchSizeX, searchSizeY, searchSizeZ);
                searchCollider.transform.GetChild(0).localPosition = new(searchPosX, searchPosY, searchPosZ);
            }
        }


        if (rangeCollider != null)
        {
            rangeCollider.GetComponent<BoxCollider>().size = new(rangeSizeX, rangeSizeY, rangeSizeZ);
            rangeCollider.GetComponent<BoxCollider>().center = new(rangePosX, rangePosY, rangePosZ);

            if (rangeCollider.transform.childCount != 0)
            {
                rangeCollider.transform.GetChild(0).localScale = new(rangeSizeX, rangeSizeY, rangeSizeZ);
                rangeCollider.transform.GetChild(0).localPosition = new(rangePosX, rangePosY, rangePosZ);
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

    public void BackWallRayCheck()
    {
        bool isWall = false;
        upWall = null;
        Debug.DrawRay(transform.position + Vector3.up * wallRayHeight, -transform.forward * wallRayBackLength, Color.magenta, 0.02f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * wallRayHeight, -transform.forward, out hit, wallRayBackLength, LayerMask.GetMask("Platform")))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                backWall = hit.collider;
                isWall = true;
                //Debug.Log("UpŽ��");
            }

            if (backWall != null)
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

        if (backWall == null)
        {
            isWall = false;
        }

        backCheck = isWall;
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
            StartCoroutine(InitPatrolTarget());
        }
        return testTarget;

    }

    bool setPatrol;
    // PatrolChange()�� SetPatrolTarget()�� ���� ��Ű�� ����Ͽ� �� ���͸� ��Ȯ�ϰ� �����Ͽ� ������Դϴ�.
    public IEnumerator InitPatrolTarget()
    {
        tracking = false;

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
