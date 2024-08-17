
using System.Collections;
using System.Linq;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public interface DamagedByPAttack
{
    public void Damaged(float f);
}


public class Enemy: Character,DamagedByPAttack
{
   

    public EnemyStat eStat;
    public PatrolType patrolType;
    //public Rigidbody enemyRb; // �� ������ٵ�
    public GameObject attackCollider; // ���� ���� �ݶ��̴� ������Ʈ    
    public ParticleSystem deadEffect;
    bool posRetry;
    [Header("�� �ִϸ��̼� ����")]
    public Animator animaor;
    public Material idleMat;
    public Material hittedMat;
    public Renderer skinRenderer;
    public ParticleSystem moveEffect;
    [HideInInspector]
    public bool isMove;
    [Header("#�÷��̾� Ž�� ť�� ����#\n(���� CCTV ���Ϳ�����)")]
    [Tooltip("����")] public Vector3 searchCubeRange; // �÷��̾� ���� ������ Cube ������� ����
    [Tooltip("��ġ")] public Vector3 searchCubePos; // Cube ��ġ ����       

    [Header("#���� Ȱ��ȭ �ݶ��̴� ť�� ����#")]
    [Tooltip("Ȱ��ȭ �ݶ��̴�")] public GameObject rangeCollider; // ���� ���� �ݶ��̴� ������Ʈ
    [Tooltip("Ȱ��ȭ ����")] public Vector3 rangeSize;
    [Tooltip("Ȱ��ȭ ��ġ")] public Vector3 rangePos;    

    [Header("#�÷��̾� Ž�� ť�� ����(�ݶ��̴�)#")]
    [Tooltip("Ž�� �ݶ��̴�")] public GameObject searchCollider; // Ž�� ���� �ݶ��̴�
    [Tooltip("Ž�� ����")] public Vector3 searchColliderRange;
    [Tooltip("Ž�� ��ġ")] public Vector3 searchColliderPos;
    public bool searchPlayer;

    [Header("#�߰� ����#")]
    [Tooltip("Ž�� �� �߰� ���� ����")]public float trackingDistance;
    [Tooltip("���� X")] public float disToPlayer;

    [Header("#���� �̵�����(���� �׷�, ������ǥ��, ���� ���ð�)#")]
    [Tooltip("���� X")]public Vector3[] patrolGroup; // 0��°: ����, 1��°: ������
    [Tooltip("���� X")]public Vector3 targetPatrol; // ���� ��ǥ����-> patrolGroup���� ����
    [Tooltip("���� ���ð�")]public float patrolWaitTime; // ���� ���ð�

    [Header("#���� ���� ����#")]
    [Tooltip("���� ���� ����")]public float leftPatrolRange; // ���� ���� ����
    [Tooltip("������ ���� ����")]public float rightPatrolRange; // ���� ���� ����
    [Tooltip("���� �Ÿ�(���� ���ص���)")]public float patrolDistance; // ���� �Ÿ�
    
    Vector3 leftPatrol, rightPatrol;
    
    public bool onPatrol;
    [Header("#�׷��� ���� ť�� ������ ����#")]
    [Tooltip("������ ����")] public float yWidth;
    [Tooltip("������ z�� ����")] public float zWidth;
    Vector3 center;

    [Header("�� üũ ����ĳ��Ʈ")]
    [Tooltip("�� üũ Ray�� ����")] public float wallRayHeight;
    [Tooltip("���� Ray ����")] public float wallRayLength;
    [Tooltip("���� Ray ����")] public float wallRayUpLength;    
    
    public Collider forwardWall;
    public Collider upWall;
    public float disToWall;    
    public bool wallCheck;
    bool forwardCheck, upCheck;
    
    public float attackTimer; // ���� ���ð�
    //public float attackInitCoolTime; // ���� ���ð� �ʱ�ȭ ����
    [HideInInspector]
    public float attackDelay; // ���� �� ������

    
    public bool callCheck;
    
    public bool onAttack; // ���� Ȱ��ȭ ���� (���� ���� ���� �÷��̾ �ν����� �� true ��ȯ)
    
    public bool activeAttack; // ���� ������ �������� üũ
    [HideInInspector]
    public bool checkPlayer; // ���� �� �÷��̾� üũ    

    [Header("��ǥ ȸ���� �׽�Ʈ�ϱ� ���� ����")]
    public Transform target; // ������ Ÿ��
    public bool tracking; // ���� Ȱ��ȭ üũ
    public Vector3 testTarget; // Ÿ���� �ٶ󺸴� ������ �׽�Ʈ�ϱ� ���� �ӽ� ����
    float rotationY; // �����̼� ���� �����ϱ� ���� �׽�Ʈ ����
    float notMinusRotation;
    float eulerAnglesY; // ���Ϸ��� Ȯ�� �׽�Ʈ        
    [HideInInspector]
    public float rotationSpeed; // �ڿ������� ȸ���� ã�� ���� �׽�Ʈ 

    [Header("��������")]
    [HideInInspector]
    public bool onStun;
    public bool reachCheck;
    bool complete;
    public bool attackRange;

   protected override void Awake()
    {

        base.Awake();
        if (attackCollider != null)
            attackCollider.SetActive(false);
        eStat = GetComponent<EnemyStat>();
        

        if (patrolType == PatrolType.movePatrol)
        {
            InitPatrolPoint();
            if(onPatrol)
                StartCoroutine(InitPatrolTarget());
        }
    }

    public void InitPatrolPoint()
    {
        onPatrol = true;
        patrolGroup = new Vector3[2];
        patrolGroup[0] = new(transform.position.x - leftPatrolRange, transform.position.y, transform.position.z);
        patrolGroup[1] = new(transform.position.x + rightPatrolRange, transform.position.y, transform.position.z);
        leftPatrol = patrolGroup[0];
        rightPatrol = patrolGroup[1];
    }    

    private void Start()
    {                
        attackTimer = eStat.initattackCoolTime;
        
        if (onStun)
        {         
            StartCoroutine(WaitStunTime());
        }
    }

    private void Update()
    {
        ReadyAttackTime();

        if (rangeCollider != null)
        {
            rangeCollider.GetComponent<BoxCollider>().center = rangePos;
            rangeCollider.GetComponent<BoxCollider>().size = rangeSize;
        }

    }
    
    private void FixedUpdate()
    {
        /*if (searchPlayer)
            DistanceToPlayer();*/

        if (!onStun)
        {
            if(!attackRange)
                Move();
        }

        if (tracking && !onAttack && !attackRange)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }

        if (animaor != null)
        {
            animaor.SetBool("isMove", isMove);
        }
        ForwardWallRayCheck();
        UpWallRayCheck();
        WallCheckResult();
    }

    void DistanceToPlayer()
    {
        if (target != null && PlayerHandler.instance.CurrentPlayer != null)
        {
            if (target == PlayerHandler.instance.CurrentPlayer)
            {
                testTarget = target.position - transform.position;
                testTarget.y = 0;
                disToPlayer = testTarget.magnitude;
            }
        }
    }

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
                if (target != null && PlayerHandler.instance != null)
                {
                    if (PlayerHandler.instance.CurrentPlayer != null && target.gameObject == PlayerHandler.instance.CurrentPlayer.gameObject)
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
        /*RaycastHit[] forwardHits = Physics.RaycastAll(transform.position + Vector3.up * wallRayHeight, transform.forward, wallRayLength);
        for (int i = 0; i < forwardHits.Length; i++)
        {
            if (forwardHits[i].collider.CompareTag("Ground"))
            {
                forwardWall = forwardHits[i].collider;
                isWall = true;
                Debug.Log("ForwardŽ��");
            }

            if (forwardWall != null)
            {
                Vector3 targetWall = forwardWall.transform.position - transform.position;
                disToWall = targetWall.magnitude;
                if (target != null && PlayerHandler.instance != null)
                {
                    if (PlayerHandler.instance.CurrentPlayer != null && target.gameObject == PlayerHandler.instance.CurrentPlayer.gameObject)
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
        }*/
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
                if (target != null && PlayerHandler.instance != null)
                {
                    if (PlayerHandler.instance.CurrentPlayer != null && target.gameObject == PlayerHandler.instance.CurrentPlayer.gameObject)
                    {
                        if (target.transform.position.y < upWall.transform.position.y)
                        {
                            //Debug.Log("�÷��̾�� õ�忡 ���� ����");
                            isWall = false; //�÷��̾� y���� ���� �ٴ��� y�� ���� ���� ������ false
                        }
                        else
                        {
                            isWall = true;
                            if (searchPlayer && !onPatrol)
                            {
                                searchPlayer = false;
                                onPatrol = true;
                            }
                        }
                    }
                }
            }
        }
        /*RaycastHit[] upHits = Physics.RaycastAll(transform.position + Vector3.up * wallRayHeight, transform.up, wallRayUpLength);
        for (int j = 0; j < upHits.Length; j++)
        {
            if (upHits[j].collider.CompareTag("Ground"))
            {
                upWall = upHits[j].collider;
                isWall = true;
                Debug.Log("UpŽ��");
            }

            if (upWall != null)
            {
                if (target != null && PlayerHandler.instance != null)
                {
                    if (PlayerHandler.instance.CurrentPlayer != null && target.gameObject == PlayerHandler.instance.CurrentPlayer.gameObject)
                    {
                        if (target.transform.position.y < upWall.transform.position.y)
                        {
                            isWall = false; //�÷��̾� y���� ���� �ٴ��� y�� ���� ���� ������ false
                        }
                        else
                        {
                            isWall = true;
                        }
                    }
                }
            }
        }*/

        if (upWall == null)
        {
            isWall = false;
        }

        upCheck = isWall;
    }

    public void WallCheckResult()
    {
        if (forwardCheck || upCheck || forwardCheck && upCheck)
            wallCheck = true;
        else
            wallCheck = false;
    }

    IEnumerator WaitStunTime()
    {
        eStat.onInvincible = true;
        transform.rotation = Quaternion.Euler(0, -90 * (int)PlayerStat.instance.direction, 0);
        rb.AddForce(-((transform.forward + transform.up)*5f), ForceMode.Impulse);

        yield return new WaitForSeconds(eStat.invincibleTimer);

        onStun = false;
        eStat.onInvincible = false;
    }

    #region �ǰ��Լ�
    public override void Damaged(float damage)
    {

        eStat.hp -= damage;
        if (eStat.hp <= 0)
        {
            eStat.hp = 0;

            Dead();
        }
        else
        {
            rb.velocity = Vector3.zero;
            attackCollider.SetActive(false);
            if (target != null)
            {
                if (target.position.x > transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                }
            }
            rb.AddForce(-transform.forward * 3f, ForceMode.Impulse);
            if (animaor != null)
            {
                animaor.SetTrigger("isHitted");
                activeAttack = true;
                attackTimer = eStat.initattackCoolTime;           
                if (skinRenderer != null)
                {
                    Material[] materials = skinRenderer.materials;
                    materials[1] = hittedMat;
                    skinRenderer.materials = materials;
                }
            }
            //InitAttackCoolTime();
        }
    }

    IEnumerator HiiitedState()
    {
        eStat.eState = EnemyState.hitted;
        yield return new WaitForSeconds(1f);
        if (!onAttack)
            eStat.eState = EnemyState.idle;
        else if(onAttack)
            eStat.eState = EnemyState.attack;
    }
    #endregion

    #region �̵��Լ�
    public override void Move()
    {

        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {

            if (tracking)
            {
                if (!activeAttack && !onAttack)
                {
                    if (patrolType == PatrolType.movePatrol && onPatrol)
                        PatrolTracking();
                    
                    if(searchPlayer)
                        TrackingMove();
                }
            }

            /*if (!callCheck)
                Patrol();*/

        }        
    }

    #region �߰�
    public void TrackingMove()
    {
        testTarget = target.position - transform.position;
        //var vector = testTarget;
        testTarget.y = 0;
        disToPlayer = testTarget.magnitude;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), eStat.rotationSpeed * Time.deltaTime);
        /*rotationY = transform.localRotation.y;
        notMinusRotation = 360 - rotationY;
        eulerAnglesY = transform.eulerAngles.y;*/

        if (SetRotation())
        {            
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
            if (soundplayer != null)
                soundplayer.PlayMoveSound();
        }
        /*var a = new Vector3(vector.x, vector.y);
        float f = testTarget.z - transform.position.z; // -> ���밪�� �Ͽ� z���� n���� ũ�� false�� ������������ 
        f = Mathf.Abs(f);
        disToPlayer = a.magnitude;*/
        if (!callCheck)
        {
            if (disToPlayer > trackingDistance /*|| f > 6*/)
            {
                searchPlayer = false;
                target = null;
                onPatrol = true;
            }
        }
    }

    public void PatrolTracking()
    {
        testTarget = targetPatrol - transform.position;
        testTarget.y = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), eStat.rotationSpeed * Time.deltaTime);

        if (SetRotation())
        {
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
            if (soundplayer != null)
                soundplayer.PlayMoveSound();
        }

        if (testTarget.magnitude < patrolDistance)
        {
            tracking = false;
            StartCoroutine(InitPatrolTarget());
        }
    
    }

    bool setPatrol;

    IEnumerator InitPatrolTarget()
    {
        yield return new WaitForSeconds(patrolWaitTime);        
        PatrolChange();
        

        setPatrol = true;
        while (setPatrol)
        {
            SetPatrolTarget();
            yield return null;
        }
      
        
        tracking = true;
    }    
       
    public void PatrolChange()
    {
        patrolGroup[0].x = leftPatrol.x - leftPatrolRange;
        patrolGroup[0].y = transform.position.y;
        patrolGroup[0].z = transform.position.z;

        patrolGroup[1].x =rightPatrol.x + rightPatrolRange;
        patrolGroup[1].y = transform.position.y;
        patrolGroup[1].z = transform.position.z;
    }

    public bool SetPatrolTarget()
    {
        int randomTarget = Random.Range(0, patrolGroup.Length);
        
        if (targetPatrol == patrolGroup[randomTarget])
        {
            setPatrol = true;
        }
        else
        {
            targetPatrol = patrolGroup[randomTarget]; ;
            setPatrol = false;
        }
        return setPatrol;
    }

    /*public void LookTarget()
    {
        if (target != null)
        {
            testTarget = target.position - transform.position;
            testTarget.y = 0;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), rotationSpeed * Time.deltaTime);
        }
    }*/
    [Header("#�����̼Ƿ���(�⺻������ 85)")]
    public float rotLevel;

    public bool SetRotation()
    {
        bool completeRot = false;

        if (/*transform.eulerAngles.y >= -10 && transform.eulerAngles.y <= 10*/transform.eulerAngles.y >= rotLevel && transform.eulerAngles.y <= 10 + rotLevel)
        {
            completeRot = true;
        }
        else if (transform.eulerAngles.y >= 175 -rotLevel && transform.eulerAngles.y <= 190 -rotLevel ||
            transform.eulerAngles.y >= 350 - rotLevel && transform.eulerAngles.y <= 360 - rotLevel)
        {
            completeRot = true;
        }
        //Debug.Log($"üũ�� �Ǵ� �ų�? {complete = completeRot}\n�����̼Ǿޱ�:{transform.eulerAngles.y}");
        //Debug.Log(completeRot);
        return completeRot;
    }
    #endregion

    #region ����
    public void Patrol()
    {        

        //Debug.Log("�����ϰ����� �ʴٸ� �ֺ��� �����մϴ�");
        //Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange);
        Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange, Quaternion.identity);
        bool playerCheck = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                if (!posRetry)
                {
                    posRetry = true;
                    transform.position = new(transform.position.x, transform.position.y, PlayerHandler.instance.CurrentPlayer.transform.localPosition.z);
                }

                target = colliders[i].transform;
                //checkPlayer = true;
                playerCheck = true;
            }

            tracking = playerCheck;

            /*if (!tracking && !onAttack && activeAttack)
            {
                LookTarget();
            }*/

        }
    }


    private void OnDrawGizmos()
    {
        if (patrolType == PatrolType.movePatrol)
        {
            if (patrolGroup.Length >= 2)
            {
                center = (patrolGroup[0] + patrolGroup[1]) / 2; //s
                float xPoint = patrolGroup[1].x - patrolGroup[0].x;
                Vector3 size = new(xPoint, yWidth, zWidth);
                Gizmos.color = Color.red;

                Gizmos.DrawWireCube(center, size);
            }
            else
            {
                Vector3 p1 = transform.position;
                Vector3 p2 = transform.position;
                p1.x = p1.x -leftPatrolRange*2;
                p2.x = p2.x + rightPatrolRange*2;
                center = (p1 + p2) / 2;
                float xPoint = p2.x - p1.x;
                Vector3 size = new(xPoint, yWidth, zWidth);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(center, size);
                targetPatrol = p2;

                ForwardWallRayCheck();
                UpWallRayCheck();               
                WallCheckResult();
            }
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, trackingDistance); 
        }

        if (searchCollider != null)
        {
            searchCollider.GetComponent<BoxCollider>().size = searchColliderRange;
            searchCollider.GetComponent<BoxCollider>().center = searchColliderPos;
        }


        if (rangeCollider != null)
        {
            rangeCollider.GetComponent<BoxCollider>().size = rangeSize;
            rangeCollider.GetComponent<BoxCollider>().center = rangePos;
        }


        //Gizmos.DrawWireSphere(patrolGroup[0], 10f);
        //Gizmos.DrawWireSphere(patrolGroup[1], 10f);
        //Gizmos.DrawWireCube(transform.position + searchCubePos, searchCubeRange * 2f);
        //Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    #endregion

    #endregion

    #region ����Լ�
    public override void Dead()
    {
        eStat.eState = EnemyState.dead;
        PlayerHandler.instance.CurrentPlayer.wallcheck = false;
       
        Instantiate(deadEffect,transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
    #endregion

    #region �����Լ�
    public override void Attack()
    {
        if(animaor != null)
            animaor.Play("EnemyAttack");
        if (soundplayer != null)
            soundplayer.PlayAttackAudio();
    }

    // ���� �غ�ð�
    public void ReadyAttackTime()
    {
        if (onAttack && eStat.eState != EnemyState.dead)
        {
            if(!activeAttack)
            {
                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                else
                {
                    activeAttack = true;
                    attackTimer = eStat.initattackCoolTime;
                    Attack();
                }
            }           
        }        
    }

    public void DelayTime()
    {
        StartCoroutine(WaitDelay());
    }

    IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(eStat.attackDelay);
        InitAttackCoolTime();

    }

    // ���� �ʱ�ȭ
    public void InitAttackCoolTime()
    {        
        onAttack = false;
        activeAttack = false;
        attackTimer = eStat.initattackCoolTime;
    }
    #endregion

}
