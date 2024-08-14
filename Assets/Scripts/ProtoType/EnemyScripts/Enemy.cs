
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
    //public Rigidbody enemyRb; // 적 리지드바디
    public GameObject attackCollider; // 적의 공격 콜라이더 오브젝트    
    public ParticleSystem deadEffect;
    bool posRetry;
    [Header("적 애니메이션 관련")]
    public Animator animaor;
    public Material idleMat;
    public Material hittedMat;
    public Renderer skinRenderer;
    public ParticleSystem moveEffect;
    [HideInInspector]
    public bool isMove;
    [Header("플레이어 탐색 큐브 조정(드로우 기즈모)")]
    public Vector3 searchCubeRange; // 플레이어 인지 범위를 Cube 사이즈로 설정
    public Vector3 searchCubePos; // Cube 위치 조정
    [Header("플레이어 탐색 범위 조정(콜라이더)")]
    public GameObject searchCollider; // 탐지 범위 콜라이더
    public Vector3 searchColliderRange;
    public Vector3 searchColliderPos;
    public bool searchPlayer;
    [Header("추격 범위 ")]
    public float trackingDistance;
    public float disToPlayer;
    [Header("정찰 이동관련(정찰 그룹, 정찰목표값, 정찰 대기시간)")]
    public Vector3[] patrolGroup; // 0번째: 왼쪽, 1번째: 오른쪽
    public Vector3 targetPatrol; // 정찰 목표지점-> patrolGroup에서 지정
    public float patrolWaitTime; // 정찰 대기시간
    [Header("좌우 정찰 위치, 정찰거리값, 플레이어 추적타임")]
    public float leftPatrolRange; // 좌측 정찰 범위
    public float rightPatrolRange; // 우측 정찰 범위
    public float patrolDistance; // 정찰 거리
    public float trackingTime;
    Vector3 leftPatrol, rightPatrol;
    [HideInInspector]
    public bool onPatrol;
    [Header("그려질 정찰 큐브 사이즈 결정")]
    public float yWidth;
    public float zWidth;
    Vector3 center;
    [Header("공격 활성화 콜라이더 큐브 조정")]
    public GameObject rangeCollider; // 공격 범위 콜라이더 오브젝트
    public Vector3 rangePos;
    public Vector3 rangeSize;

    [Header("적 공격딜레이 관련(보류중)")]
    public float attackTimer; // 공격 대기시간
    public float attackInitCoolTime; // 공격 대기시간 초기화 변수
    public float attackDelay; // 공격 후 딜레이

    [HideInInspector]
    public bool callCheck;
    [HideInInspector]
    public bool onAttack; // 공격 활성화 여부 (공격 범위 내에 플레이어를 인식했을 때 true 변환)
    [HideInInspector]
    public bool activeAttack; // 공격 가능한 상태인지 체크
    [HideInInspector]
    public bool checkPlayer; // 범위 내 플레이어 체크    

    [Header("목표 회전을 테스트하기 위한 변수")]
    public Transform target; // 추적할 타겟
    public bool tracking; // 추적 활성화 체크
    public Vector3 testTarget; // 타겟을 바라보는 시점을 테스트하기 위한 임시 변수
    float rotationY; // 로테이션 값을 이해하기 위한 테스트 변수
    float notMinusRotation;
    float eulerAnglesY; // 오일러값 확인 테스트        
    [HideInInspector]
    public float rotationSpeed; // 자연스러운 회전을 찾기 위한 테스트 

    [Header("기절상태")]
    [HideInInspector]
    public bool onStun;
    public bool reachCheck;
    bool complete;
    public bool attackRange;

   protected override void Awake()
    {

        base.Awake();
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
        if (!onStun)
        {
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

    #region 피격함수
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

    #region 이동함수
    public override void Move()
    {

        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {

            if (tracking)
            {
                if (!activeAttack && !onAttack && !attackRange)
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

    #region 추격
    public void TrackingMove()
    {
        testTarget = target.position - transform.position;
        //var vector = testTarget;
        testTarget.y = 0;        

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), eStat.rotationSpeed * Time.deltaTime);
        /*rotationY = transform.localRotation.y;
        notMinusRotation = 360 - rotationY;
        eulerAnglesY = transform.eulerAngles.y;*/

        if (SetRotation())
        {            
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
        }
        /*var a = new Vector3(vector.x, vector.y);
        float f = testTarget.z - transform.position.z; // -> 절대값을 하여 z값이 n보다 크면 false로 빠져나가도록 
        f = Mathf.Abs(f);
        disToPlayer = a.magnitude;*/
        disToPlayer = testTarget.magnitude;

        if (disToPlayer > trackingDistance /*|| f > 6*/)
        {
            searchPlayer = false;
            target = null;
            onPatrol = true;
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
    [Header("#로테이션레벨(기본적으로 85)")]
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
        //Debug.Log($"체크가 되는 거냐? {complete = completeRot}\n로테이션앵글:{transform.eulerAngles.y}");
        //Debug.Log(completeRot);
        return completeRot;
    }
    #endregion

    #region 정찰
    public void Patrol()
    {        

        //Debug.Log("추적하고있지 않다면 주변을 정찰합니다");
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
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, trackingDistance); 
        }
        //Gizmos.DrawWireSphere(patrolGroup[0], 10f);
        //Gizmos.DrawWireSphere(patrolGroup[1], 10f);
        //Gizmos.DrawWireCube(transform.position + searchCubePos, searchCubeRange * 2f);
        //Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    #endregion

    #endregion

    #region 사망함수
    public override void Dead()
    {
        eStat.eState = EnemyState.dead;
        PlayerHandler.instance.CurrentPlayer.dmCollider.OtherCheck(this.gameObject);
        Instantiate(deadEffect,transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
    #endregion

    #region 공격함수
    public override void Attack()
    {
        if(animaor != null)
            animaor.Play("EnemyAttack");
    }

    // 공격 준비시간
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

    // 공격 초기화
    public void InitAttackCoolTime()
    {        
        onAttack = false;
        activeAttack = false;
        attackTimer = eStat.initattackCoolTime;
    }
    #endregion

}
