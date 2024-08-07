using UnityEngine;

public class BoxTestt : MonoBehaviour
{
    public EnemyStat eStat;

    public Animator animator;

    public Rigidbody enemyRb; // 적 리지드바디
    public GameObject attackCollider; // 적의 공격 콜라이더 오브젝트
    public GameObject rangeCollider; // 공격 범위 콜라이더 오브젝트
    public GameObject enemy; // 생성될 적 프리팹
    public GameObject breakBox; // 부서진 박스 파티클 오브젝트

    //public float searchRange; // 플레이어 인지 범위
    //public float attackRange; // 공격 실행 범위
    [Header("플레이어 탐색 큐브 조정")]
    public Vector3 searchCubeRange; // 플레이어 인지 범위를 Cube 사이즈로 설정
    public Vector3 searchCubePos; // Cube 위치 조정

    [Header("플레이어 추격 관련")]
    public float attackTimer; // 공격 대기시간
    public float attackInitCoolTime; // 공격 대기시간 초기화 변수
    public float attackDelay; // 공격 후 딜레이
    [Header("하위 오브젝트 공격 범위 콜라이더에서 변경중")]
    public bool onAttack; // 공격 활성화 여부 (공격 범위 내에 플레이어를 인식했을 때 true 변환)
    public bool activeAttack; // 공격 가능한 상태인지 체크
    public bool checkPlayer; // 범위 내 플레이어 체크    

    [Header("목표 회전을 테스트하기 위한 변수")]
    public Transform target; // 추적할 타겟
    public bool tracking; // 추적 활성화 체크
    public Vector3 testTarget; // 타겟을 바라보는 시점을 테스트하기 위한 임시 변수
    public float rotationY; // 로테이션 값을 이해하기 위한 테스트 변수
    public float notMinusRotation;
    public float eulerAnglesY; // 오일러값 확인 테스트
    public float rotationSpeed; // 자연스러운 회전을 찾기 위한 테스트 

    public Vector3 rotPos;

    public float rotLevel;
    bool complete;

    private void Awake()
    {
        eStat = gameObject.AddComponent<EnemyStat>();
        //attackCollider.GetComponent<TrackingBoxAttack>().SetDamage(eStat.atk);
        attackCollider.SetActive(false);

        enemyRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        attackInitCoolTime = 3.5f;
        attackTimer = attackInitCoolTime;
        attackDelay = 1.5f;
        /*if (onStun)
        {
            Debug.Log("행동 불능");
            StartCoroutine(WaitStunTime());
        }*/
    }

    private void Update()
    {
        ReadyAttackTime();


    }

    // 부모인 Enemy에서 사용?
    // 자식인 다양한 적 오브젝트 스크립트에서 사용? 
    private void FixedUpdate()
    {
        Move();
    }    

    #region 피격함수
    public void Damaged(float damage)
    {
        eStat.hp -= damage;
        if (eStat.hp <= 0)
        {
            eStat.hp = 0;

            Dead();
        }
        enemyRb.AddForce(-transform.forward * 3f, ForceMode.Impulse);
        InitAttackCoolTime();
    }
    #endregion

    #region 이동함수
    public void Move()
    {

        if (eStat.eState != EnemyState.dead)
        {
            if (tracking)
            {
                if (!activeAttack && !onAttack)
                {
                    TrackingMove();
                }
            }

            Patrol();
        }
    }

    #region 추격
    public void TrackingMove()
    {
        animator.SetBool("Tracking", tracking);

        testTarget = target.position - transform.position;
        testTarget.y = 0;

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 10 * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), rotationSpeed * Time.deltaTime);
        rotationY = transform.localRotation.y;
        notMinusRotation = 360 - rotationY;
        eulerAnglesY = transform.eulerAngles.y;
        
        if (SetRotation())
        {
            enemyRb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
        }
    }

    public bool SetRotation()
    {
        bool completeRot = false;

        if (/*transform.eulerAngles.y >= -10 && transform.eulerAngles.y <= 10*/transform.eulerAngles.y >=5+rotLevel && transform.eulerAngles.y<=10+rotLevel)
        {
            completeRot = true;
        }
        else if (transform.eulerAngles.y >= 175 - rotLevel && transform.eulerAngles.y <= 190 - rotLevel ||
            transform.eulerAngles.y >= 350 - rotLevel && transform.eulerAngles.y <= 360 - rotLevel)
        {
            completeRot = true;
        }
        //Debug.Log($"체크가 되는 거냐? {complete = completeRot}\n로테이션앵글:{transform.eulerAngles.y}");
        return completeRot;
    }
    #endregion

    #region 정찰
    public void Patrol()
    {
        //Debug.Log("추적하고있지 않다면 주변을 정찰합니다");
        //Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange);
        Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                //Debug.Log($"{target} 추적해라");
                target = colliders[i].transform;
                //checkPlayer = true;
                tracking = true;

                //animator.SetBool("Tracking", tracking);
            }
            /*else
            {
                //Debug.Log("플레이어 추적하지마라");
                tracking = false;
                checkPlayer = false;                
            }*/
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + searchCubePos, searchCubeRange * 2f);
        //Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    #endregion

    #endregion

    #region 사망함수
    public void Dead()
    {
        //rangeCollider.SetActive(false);
        gameObject.SetActive(false);
    }
    #endregion

    #region 공격함수
    public void Attack()
    {
        //공격 콜라이더 오브젝트 활성화
        attackCollider.SetActive(true);        
        /*
         * 공격 콜라이더 오브젝트를 0.2초 후에 비활성화한 다음
         * activeAttack 부울 변수를 false변환 및 공격 타이머 초기화      
        */        
    }

    // 공격 준비
    public void ReadyAttackTime()
    {

        if (onAttack && eStat.eState != EnemyState.dead)
        {

            if (attackTimer > 0 && !activeAttack)
            {
                attackTimer -= Time.deltaTime;
            }
            else if (attackTimer <= 0)
            {
                activeAttack = true;
                attackTimer = attackInitCoolTime;
                Attack();
            }
        }
        /*else
        {
            InitAttackCoolTime();
        }*/
    }

    // 공격 초기화
    public void InitAttackCoolTime()
    {
        activeAttack = false;
        attackTimer = attackInitCoolTime;
        onAttack = false;
    }
    #endregion

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            RemoteObject o=null;
            if (other.TryGetComponent<RemoteObject>(out o))
            {
                if (other.GetComponent<RemoteObject>().CanControl)
                {
                    target = other.transform;
                    tracking = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("GameController"))
        {
            if (other.GetComponent<RemoteObject>().rType == RemoteType.tv && !hitByPlayer)
            {
                RemoteObject tv = other.GetComponent<RemoteObject>();

                if (tv.onActive)
                {
                    target = other.transform;
                    //activeTv = true;
                    tracking = true;
                }
            }
        }*/

        if (other.CompareTag("Player"))
        {
            target = other.transform;
            tracking = true;
        }

        if (other.CompareTag("PlayerAttack"))
        {

            eStat.eState = EnemyState.dead;

            rangeCollider.SetActive(false);
            //gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            Instantiate(breakBox, transform.position, Quaternion.identity).transform.parent = this.transform;
            Instantiate(enemy, transform.position, Quaternion.identity).GetComponent<Enemy>().onStun = true;
            gameObject.SetActive(false);
        }
    }

    /*IEnumerator Broken()
    {
        animator.SetTrigger("Broken");

        yield return new WaitForSeconds(4f);

        gameObject.SetActive(false);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            /*enemyRb.constraints = RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezeRotation;*/
        }
    }
    /*public int breakCount; // 예를 들어 나무상자보다 더 튼튼한 오브젝트인 경우 여러번 떄려야 부숴지게 설정
    public GameObject enemy;

    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            eStat.cState = CharacterState.dead;
            rangeCollider.SetActive(false);
            //gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(Broken());
            Instantiate(enemy, transform.position, Quaternion.identity).GetComponent<Enemy>().onStun = true;
        }
    }

    IEnumerator Broken()
    {
        animator.SetTrigger("Broken");

        yield return new WaitForSeconds(4f);

        gameObject.SetActive(false);
    }*/
}
