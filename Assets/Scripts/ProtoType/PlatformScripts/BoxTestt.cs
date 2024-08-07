using UnityEngine;

public class BoxTestt : MonoBehaviour
{
    public EnemyStat eStat;

    public Animator animator;

    public Rigidbody enemyRb; // �� ������ٵ�
    public GameObject attackCollider; // ���� ���� �ݶ��̴� ������Ʈ
    public GameObject rangeCollider; // ���� ���� �ݶ��̴� ������Ʈ
    public GameObject enemy; // ������ �� ������
    public GameObject breakBox; // �μ��� �ڽ� ��ƼŬ ������Ʈ

    //public float searchRange; // �÷��̾� ���� ����
    //public float attackRange; // ���� ���� ����
    [Header("�÷��̾� Ž�� ť�� ����")]
    public Vector3 searchCubeRange; // �÷��̾� ���� ������ Cube ������� ����
    public Vector3 searchCubePos; // Cube ��ġ ����

    [Header("�÷��̾� �߰� ����")]
    public float attackTimer; // ���� ���ð�
    public float attackInitCoolTime; // ���� ���ð� �ʱ�ȭ ����
    public float attackDelay; // ���� �� ������
    [Header("���� ������Ʈ ���� ���� �ݶ��̴����� ������")]
    public bool onAttack; // ���� Ȱ��ȭ ���� (���� ���� ���� �÷��̾ �ν����� �� true ��ȯ)
    public bool activeAttack; // ���� ������ �������� üũ
    public bool checkPlayer; // ���� �� �÷��̾� üũ    

    [Header("��ǥ ȸ���� �׽�Ʈ�ϱ� ���� ����")]
    public Transform target; // ������ Ÿ��
    public bool tracking; // ���� Ȱ��ȭ üũ
    public Vector3 testTarget; // Ÿ���� �ٶ󺸴� ������ �׽�Ʈ�ϱ� ���� �ӽ� ����
    public float rotationY; // �����̼� ���� �����ϱ� ���� �׽�Ʈ ����
    public float notMinusRotation;
    public float eulerAnglesY; // ���Ϸ��� Ȯ�� �׽�Ʈ
    public float rotationSpeed; // �ڿ������� ȸ���� ã�� ���� �׽�Ʈ 

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
            Debug.Log("�ൿ �Ҵ�");
            StartCoroutine(WaitStunTime());
        }*/
    }

    private void Update()
    {
        ReadyAttackTime();


    }

    // �θ��� Enemy���� ���?
    // �ڽ��� �پ��� �� ������Ʈ ��ũ��Ʈ���� ���? 
    private void FixedUpdate()
    {
        Move();
    }    

    #region �ǰ��Լ�
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

    #region �̵��Լ�
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

    #region �߰�
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
        //Debug.Log($"üũ�� �Ǵ� �ų�? {complete = completeRot}\n�����̼Ǿޱ�:{transform.eulerAngles.y}");
        return completeRot;
    }
    #endregion

    #region ����
    public void Patrol()
    {
        //Debug.Log("�����ϰ����� �ʴٸ� �ֺ��� �����մϴ�");
        //Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange);
        Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                //Debug.Log($"{target} �����ض�");
                target = colliders[i].transform;
                //checkPlayer = true;
                tracking = true;

                //animator.SetBool("Tracking", tracking);
            }
            /*else
            {
                //Debug.Log("�÷��̾� ������������");
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

    #region ����Լ�
    public void Dead()
    {
        //rangeCollider.SetActive(false);
        gameObject.SetActive(false);
    }
    #endregion

    #region �����Լ�
    public void Attack()
    {
        //���� �ݶ��̴� ������Ʈ Ȱ��ȭ
        attackCollider.SetActive(true);        
        /*
         * ���� �ݶ��̴� ������Ʈ�� 0.2�� �Ŀ� ��Ȱ��ȭ�� ����
         * activeAttack �ο� ������ false��ȯ �� ���� Ÿ�̸� �ʱ�ȭ      
        */        
    }

    // ���� �غ�
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

    // ���� �ʱ�ȭ
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
    /*public int breakCount; // ���� ��� �������ں��� �� ưư�� ������Ʈ�� ��� ������ ������ �ν����� ����
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
