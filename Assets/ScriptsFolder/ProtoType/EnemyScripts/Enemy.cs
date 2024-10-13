

using System.Collections;
using System.Linq;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public enum EnemyMovePattern { stop,patrol}
public interface DamagedByPAttack
{
    public void Damaged(float f);
}


public class Enemy: Character,DamagedByPAttack,environmentObject
{

    public GameObject EnemyHitCol2D;

    public Color testcolor;
    public EnemyMovePattern movepattern;
    public EnemyStat eStat;
    public PatrolType patrolType;
    //public Rigidbody enemyRb; // 적 리지드바디
    public GameObject attackCollider; // 적의 공격 콜라이더 오브젝트    
    public ParticleSystem deadEffect;
    bool posRetry;
    [Header("적 애니메이션 관련")]
    public Animator animaor;
    [Header("기본 머티리얼")]
    public Material idleMat;
    public Material backMat;
    public Material headMat;
    public Material hittedMat;
    public Renderer skinRenderer;
    
    public ParticleSystem moveEffect;
    public Vector3 environmentforce;
    [HideInInspector]
    public bool isMove;
    [Header("#플레이어 탐색 큐브 조정#\n(현재 CCTV 몬스터에서만)")]
    [Tooltip("범위")] public Vector3 searchCubeRange; // 플레이어 인지 범위를 Cube 사이즈로 설정
    [Tooltip("위치")] public Vector3 searchCubePos; // Cube 위치 조정       

    [Header("#공격 활성화 콜라이더 큐브 조정#")]
    [Tooltip("활성화 콜라이더")] public GameObject rangeCollider; // 공격 범위 콜라이더 오브젝트
    [Tooltip("활성화 범위")] public Vector3 rangeSize;
    [Tooltip("활성화 위치")] public Vector3 rangePos;    

    [Header("#플레이어 탐색 큐브 조정(콜라이더)#")]
    [Tooltip("탐색 콜라이더")] public GameObject searchCollider; // 탐지 범위 콜라이더
    [Tooltip("탐색 범위")] public Vector3 searchColliderRange;
    [Tooltip("탐색 위치")] public Vector3 searchColliderPos;
    public bool searchPlayer;

    [Header("#추격 범위#")]
    [Tooltip("탐색 후 추격 유지 범위")]public float trackingDistance;
    [Tooltip("설정 X")] public float disToPlayer;

    [Header("#정찰 이동관련(정찰 그룹, 정찰목표값, 정찰 대기시간)#")]
    public Transform[] PatrolTransform;
    [Tooltip("설정 X")]public Vector3[] patrolGroup; // 0번째: 왼쪽, 1번째: 오른쪽
    [Tooltip("설정 X")]public Vector3 targetPatrol; // 정찰 목표지점-> patrolGroup에서 지정
    [Tooltip("정찰 대기시간")]public float patrolWaitTime; // 정찰 대기시간

    [Header("#정찰 범위 관련#")]
    [Tooltip("왼쪽 정찰 범위")]public float leftPatrolRange; // 좌측 정찰 범위
    [Tooltip("오른쪽 정찰 범위")]public float rightPatrolRange; // 우측 정찰 범위
    [Tooltip("정찰 거리(설정 안해도됨)")]public float patrolDistance; // 정찰 거리
    
   protected Vector3 leftPatrol, rightPatrol;
    
    public bool onPatrol;
    [Header("#그려질 정찰 큐브 사이즈 결정#")]
    [Tooltip("붉은색 높이")] public float yWidth;
    [Tooltip("붉은색 z축 넓이")] public float zWidth;
    Vector3 center;

    [Header("벽 체크 레이캐스트")]
    [Tooltip("벽 체크 Ray의 높이")] public float wallRayHeight;
    [Tooltip("정면 Ray 길이")] public float wallRayLength;
    [Tooltip("위쪽 Ray 길이")] public float wallRayUpLength;    
    
    public Collider forwardWall;
    public Collider upWall;
    public float disToWall;    
    public bool wallCheck;
    bool forwardCheck, upCheck;
    
    public float attackTimer; // 공격 대기시간
    //public float attackInitCoolTime; // 공격 대기시간 초기화 변수
    [HideInInspector]
    public float attackDelay; // 공격 후 딜레이
   public EnemyAttackHandler actionhandler;

    public bool callCheck;
    public bool rotCheck;
    
    public bool onAttack; // 공격 활성화 여부 (공격 범위 내에 플레이어를 인식했을 때 true 변환)
    
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
    /*[HideInInspector]*/
    public bool onStun;
    public bool reachCheck;
    bool complete;
    public bool attackRange;

    public bool viewActive;

    private void OnEnable()
    {
        StartCoroutine(InitPatrolTarget());
    }

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
        actionhandler = GetComponent<EnemyAttackHandler>();
        if (actionhandler != null)
            actionhandler.e = this;

        if (flatObject != null)
        {
            originScale = flatObject.transform.localScale;
            flatScale = new(flatObject.transform.localScale.x, flatScaleY, flatObject.transform.localScale.z);
        }
    }

    public void InitPatrolPoint()
    {
        onPatrol = true;
        if (PatrolTransform.Length==0)
        {
            patrolGroup = new Vector3[2];
            patrolGroup[0] = new(transform.position.x - leftPatrolRange, transform.position.y, transform.position.z);
            patrolGroup[1] = new(transform.position.x + rightPatrolRange, transform.position.y, transform.position.z);
            leftPatrol = patrolGroup[0];
            rightPatrol = patrolGroup[1];
        }
        else
        {
            patrolGroup = new Vector3[PatrolTransform.Length];
          for(int n=0; n < PatrolTransform.Length; n++)
            {
                patrolGroup[n] = PatrolTransform[n].position;
            }
            leftPatrol = patrolGroup[0];
            rightPatrol = patrolGroup[1];
        }
    }    

    private void Start()
    {                
        attackTimer = eStat.initattackCoolTime;

        /*if (onStun)
        {         
            StartCoroutine(WaitStunTime());
        }*/
    }

    private void Update()
    {
        if (!onStun)
        {

            ReadyAttackTime();

            if (rangeCollider != null)
            {
                rangeCollider.GetComponent<BoxCollider>().center = rangePos;
                rangeCollider.GetComponent<BoxCollider>().size = rangeSize;
            }
        }
        else
        {
            if (onFlat)
            {
                if (timer < flatTime)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    RollBackFormFlatState();
                }
            }

        }


    }

    protected virtual void MoveAnimationPlay()
    {
        if (animaor != null)
        {
            animaor.SetBool("isMove", isMove);
        }
    }

    private void FixedUpdate()
    {
        /*if (searchPlayer)
            DistanceToPlayer();*/

        if (!onStun)
        {
            if (!attackRange)
                Move();

            if (movepattern == EnemyMovePattern.stop)
            {
                if (tracking && !onAttack && !attackRange && searchPlayer)
                {
                    isMove = true;
                }
                else
                {
                    isMove = false;
                }
            }
            else
            {
                if (tracking && !onAttack && !attackRange)
                {
                    isMove = true;
                }
                else
                {
                    isMove = false;
                }
            }
            MoveAnimationPlay();
        }
        ForwardWallRayCheck();
        UpWallRayCheck();
        WallCheckResult();
        if(environmentforce
            !=Vector3.zero)
        {
            rb.AddForce(environmentforce, ForceMode.VelocityChange);
            environmentforce = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
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
                //Debug.Log("Forward탐지");
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
        /*RaycastHit[] forwardHits = Physics.RaycastAll(transform.position + Vector3.up * wallRayHeight, transform.forward, wallRayLength);
        for (int i = 0; i < forwardHits.Length; i++)
        {
            if (forwardHits[i].collider.CompareTag("Ground"))
            {
                forwardWall = forwardHits[i].collider;
                isWall = true;
                Debug.Log("Forward탐지");
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
                            isWall = false; // 플레이어와의 거리가 벽과의 거리보다 가까울 경우
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
                //Debug.Log("Up탐지");
            }

            if (upWall != null)
            {
                if (target != null && PlayerHandler.instance != null)
                {
                    if (PlayerHandler.instance.CurrentPlayer != null && target.gameObject == PlayerHandler.instance.CurrentPlayer.gameObject)
                    {
                        if (target.transform.position.y < upWall.transform.position.y)
                        {
                            //Debug.Log("플레이어는 천장에 있지 않음");
                            isWall = false; //플레이어 y축이 위쪽 바닥의 y축 보다 값이 작으면 false
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
                Debug.Log("Up탐지");
            }

            if (upWall != null)
            {
                if (target != null && PlayerHandler.instance != null)
                {
                    if (PlayerHandler.instance.CurrentPlayer != null && target.gameObject == PlayerHandler.instance.CurrentPlayer.gameObject)
                    {
                        if (target.transform.position.y < upWall.transform.position.y)
                        {
                            isWall = false; //플레이어 y축이 위쪽 바닥의 y축 보다 값이 작으면 false
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

    #region 피격함수
    public virtual void HittedRotate()
    {
        if (target != null)
        {
            if (PlayerHandler.instance.CurrentPlayer != null && target.gameObject == PlayerHandler.instance.CurrentPlayer.gameObject)
            {
                target = PlayerHandler.instance.CurrentPlayer.transform;

                Vector3 pos = target.position - transform.position;
                pos.y = 0;
                transform.rotation = Quaternion.LookRotation(pos);
            }
        }
        else
        {
            target = PlayerHandler.instance.CurrentPlayer.transform;

            Vector3 pos = target.position - transform.position;
            pos.y = 0;
            transform.rotation = Quaternion.LookRotation(pos);
        }
        rb.AddForce(-transform.forward * 3f, ForceMode.Impulse);
    }
    public override void Damaged(float damage)
    {
        base.Damaged(damage);
        eStat.hp -= damage;
        if (eStat.hp <= 0)
        {
            eStat.hp = 0;

            Dead();
        }
        else
        {
            HittedRotate();
            StopCoroutine("HittedEnd");
            if (!onStun)
            {
                rb.velocity = Vector3.zero;
                if (attackCollider != null)
                    attackCollider.SetActive(false);

                soundplayer.PlayHittedSound();
                if (animaor != null)
                {
                    animaor.SetTrigger("isHitted");
                    activeAttack = true;
                    attackTimer = eStat.initattackCoolTime;
                }
                //InitAttackCoolTime();                
                StartCoroutine("HittedEnd");
            }
        }
    }
    #region 피격 코루틴
    public virtual IEnumerator HittedEnd()
    {
        if (skinRenderer != null)
        {
            StartEmmissionHitMat();
        }
        
        yield return new WaitForSeconds(0.5f);
        
        if (skinRenderer != null)
        {
            EndEmmissionHitMat();
        }

        if (animaor.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Damaged_ToBack"))
        {
            while (animaor.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            Debug.Log("피격 코루틴 복귀");
        }

        if (skinRenderer != null)
        {
            EndHitMat();
        }

        activeAttack = false;
    }

    public virtual void StartEmmissionHitMat()
    {
        
    }

    public virtual void EndEmmissionHitMat()
    {
        Material[] materials = skinRenderer.materials;
        materials[0] = backMat;
        materials[1] = hittedMat;
        skinRenderer.materials = materials;
    }

    public virtual void EndHitMat()
    {
        Material[] materials = skinRenderer.materials;
        materials[0] = backMat;
        materials[1] = idleMat;
        skinRenderer.materials = materials;

        Debug.Log("기본 머테리얼로 복귀");
    }

    #endregion

    [Header("납작해지도록 적용될 스케일 오브젝트")]
    public GameObject flatObject;
    public float flatScaleY;
    public float flatTime=0;
    public float timer;
    [HideInInspector] public bool onFlat;
    Vector3 originScale;
    Vector3 flatScale;
    //납작하게 되는 함수
    public virtual void FlatByIronDwonAttack(float flat)
    {
        if (flatObject == null)
            return;

        onStun = true;
        onFlat = true;
        flatObject.transform.localScale = flatScale;
        flatTime = flat;
        attackRange = false;
        Debug.Log(flat);

        if (skinRenderer != null)
        {
            Material[] materials = skinRenderer.materials;
            materials[1] = hittedMat;
            skinRenderer.materials = materials;
        }
    }

    public virtual void RollBackFormFlatState()
    {
        onStun = false;
        onFlat = false;
        timer = 0;
        if (skinRenderer != null)
        {
            Material[] materials = skinRenderer.materials;
            materials[1] = idleMat;
            skinRenderer.materials = materials;
        }

        flatObject.transform.localScale = originScale;
    }

    /*IEnumerator RollBackFromFlatState(float flat)
    {
        float timer = 0;
        Debug.Log("납작 코루틴 실행됨");
        onStun = true;
        attackRange = false;
        flatObject.transform.localScale = flatScale;
        if (skinRenderer != null)
        {
            Material[] materials = skinRenderer.materials;
            materials[1] = hittedMat;
            skinRenderer.materials = materials;
        }

        while (timer < flatTime)
        {
            timer += Time.deltaTime;
            Debug.Log($"{timer}, {flat}");
            yield return null;
        }
        
        flatObject.transform.localScale = originScale;
        Debug.Log("원래 스케일로 돌아와!");
        if (skinRenderer != null)
        {
            Material[] materials = skinRenderer.materials;
            materials[1] = idleMat;
            skinRenderer.materials = materials;
        }
        onStun = false;
    }*/

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
  protected  bool onmove;
    #region 이동함수
    public override void Move()
    {
  
        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {
           
            if (tracking)
            {
                if (!activeAttack && !onAttack)
                {
                    if (movepattern == EnemyMovePattern. patrol)
                    {
                        if (patrolType == PatrolType.movePatrol && onPatrol)
                        {
                        
                            PatrolTracking();
                        }
                    }
                    if(searchPlayer)
                        TrackingMove();
                }
            }

            /*if (!callCheck)
                Patrol();*/

        }        
    }
    protected virtual void PlayMoveSound()
    {
        if (soundplayer != null)
            soundplayer.PlayMoveSound();
    }
    #region 추격
    public virtual void TrackingMove()
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
            enemymovepattern();
            PlayMoveSound();
        }
        /*var a = new Vector3(vector.x, vector.y);
        float f = testTarget.z - transform.position.z; // -> 절대값을 하여 z값이 n보다 크면 false로 빠져나가도록 
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
    public virtual void enemymovepattern()
    {
        if(rb != null)
        rb.MovePosition(environmentforce+transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
    }
    public virtual void PatrolTracking()
    {
    
        testTarget = targetPatrol - transform.position;
        testTarget.y = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), eStat.rotationSpeed * Time.deltaTime);

        if (SetRotation())
        {
          
            enemymovepattern();
            PlayMoveSound();
        }
        if (testTarget.magnitude < patrolDistance)
        {
            tracking = false;
            StartCoroutine(InitPatrolTarget());
        }
    
    }

    bool setPatrol;

  protected  IEnumerator InitPatrolTarget()
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
       
    public virtual void PatrolChange()
    {
        if (patrolGroup.Length >=2)
        {
            patrolGroup[0].x = leftPatrol.x - leftPatrolRange;
            patrolGroup[0].y = transform.position.y;
            patrolGroup[0].z = transform.position.z;

            patrolGroup[1].x = rightPatrol.x + rightPatrolRange;
            patrolGroup[1].y = transform.position.y;
            patrolGroup[1].z = transform.position.z;
        }
    }

    public bool SetPatrolTarget()
    {
        int randomTarget = Random.Range(0, patrolGroup.Length);
        if (patrolGroup.Length >= 2)
        {
            if (targetPatrol == patrolGroup[randomTarget])
            {
                setPatrol = true;
            }
            else
            {
                targetPatrol = patrolGroup[randomTarget];
                setPatrol = false;
            }
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
    public float testAngle;
    public virtual bool SetRotation()
    {
        bool completeRot = false;
        if (target != null && !onPatrol)
        {
            Vector3 targetTothis = target.position - transform.position;
            targetTothis.y = 0;
            Quaternion q = Quaternion.LookRotation(targetTothis);
            testAngle = Quaternion.Angle(transform.rotation, q);
            if (testAngle < 45f)
                completeRot = true;
        }
        else if (onPatrol)
        {
            Vector3 patrolTothis = targetPatrol - transform.position;
            patrolTothis.y = 0;
            Quaternion q = Quaternion.LookRotation(patrolTothis);
            testAngle = Quaternion.Angle(transform.rotation, q);
            if (testAngle < 1.5f)
                completeRot = true;
        }
        /*eulerAnglesY = transform.eulerAngles.y;
        if (transform.eulerAngles.y >= rotLevel && transform.eulerAngles.y <= 10 + rotLevel)
        {
            completeRot = true;
        }
        else if (transform.eulerAngles.y >= 175 -rotLevel && transform.eulerAngles.y <= 190 -rotLevel ||
            transform.eulerAngles.y >= 350 - rotLevel && transform.eulerAngles.y <= 360 - rotLevel)
        { 
            completeRot = true;
        }*/
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
        if (!eStat)
        {
            eStat = GetComponent<EnemyStat>();
        }

        if (patrolType == PatrolType.movePatrol)
        {
            if (patrolGroup.Length >= 2)
            {
                center = (patrolGroup[0] + patrolGroup[1]) / 2; //s
                float xPoint = patrolGroup[1].x - patrolGroup[0].x;
                Vector3 size = new(xPoint, yWidth, zWidth);
                if (CharColliderColor.instance != null)
                    Gizmos.color = CharColliderColor.instance.patrolRange;
                else
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

                if (CharColliderColor.instance != null)
                    Gizmos.color = CharColliderColor.instance.patrolRange;
                else
                    Gizmos.color = Color.red;
                Gizmos.DrawWireCube(center, size);
                targetPatrol = p2;

                ForwardWallRayCheck();
                UpWallRayCheck();               
                WallCheckResult();
            }
            if (CharColliderColor.instance != null)
                Gizmos.color = CharColliderColor.instance.trackingRange;
            else
                Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, trackingDistance);
            //Gizmos.color = Color.yellow;

            //Gizmos.DrawWireSphere(transform.position, trackingDistance); 
        }

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
        base.Dead();
        eStat.eState = EnemyState.dead;
        if (PlayerHandler.instance != null)
            PlayerHandler.instance.CurrentPlayer.wallcheck = false;

        if (deadEffect != null)
        {
            if (TryGetComponent<RagdolEnemy>(out RagdolEnemy enemy))
            {
                if (enemy.isRagdoll)
                {
                    enemy.RagdolDeadEffect(deadEffect);
                }
                else
                {
                    Instantiate(deadEffect, transform.position, Quaternion.identity);
                }
            }
            else
            Instantiate(deadEffect, transform.position, Quaternion.identity);
        }
        if (transform.parent.gameObject.TryGetComponent<EnemyEnable>(out EnemyEnable enable))
        {
            transform.parent.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    #endregion

    #region 공격함수

    protected virtual void PlayAttackSound()
    {
        if (soundplayer != null)
            soundplayer.PlayAttackAudio();
    }
    public override void Attack()
    {
        base.Attack();
        if(animaor != null)
            animaor.Play("EnemyAttack");
        PlayAttackSound();
        if (actionhandler!=null)
        actionhandler.invokemainaction();
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

    public void AddEnviromentPower(Vector3 power)
    {
        environmentforce = power;
    }
    #endregion

}
