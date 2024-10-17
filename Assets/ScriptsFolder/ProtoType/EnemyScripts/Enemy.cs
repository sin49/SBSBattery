using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //public Rigidbody enemyRb; // �� ������ٵ�
    public GameObject attackCollider; // ���� ���� �ݶ��̴� ������Ʈ    
    public ParticleSystem deadEffect;
    bool posRetry;
    [Header("�� �ִϸ��̼� ����")]
    public Animator animaor;
    
    
    public ParticleSystem moveEffect;
    public Vector3 environmentforce;
    [HideInInspector]
    public bool isMove;

    public EnemyTrackingAndPatrol tap;
    public EnemySearchCollider searchCollider;
    public EnemyMaterialAndEffect mae;
    
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
   public EnemyAttackHandler actionhandler;

    public bool callCheck;
    public bool rotCheck;
    
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
            if(searchCollider.onPatrol)
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
        searchCollider.onPatrol = true;
        tap.SetPoint(transform);
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
                if (tracking && !onAttack && !attackRange && searchCollider.searchPlayer)
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
                tap.disToPlayer = testTarget.magnitude;
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
                        if (tap.disToPlayer < disToWall)
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
                            if (searchCollider.searchPlayer && !searchCollider.onPatrol)
                            {
                                searchCollider.searchPlayer = false;
                                searchCollider.onPatrol = true;
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
            wallCheck = true;
        else
            wallCheck = false;
    }
   
    public Vector3 direction;//������
    //public bool �ൿ����;//
 
    void move()
    {
     
    }
    #region �ǰ��Լ�
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
                if(soundplayer!=null)
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
    #region �ǰ� �ڷ�ƾ
    public virtual IEnumerator HittedEnd()
    {
        if (mae != null)
        {
            StartEmmissionHitMat();
        }
        
        yield return new WaitForSeconds(0.5f);
        
        if (mae != null)
        {
            EndEmmissionHitMat();
        }

        if (animaor.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Damaged_ToBack"))
        {
            while (animaor.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            Debug.Log("�ǰ� �ڷ�ƾ ����");
        }

        if (mae != null)
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
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = mae.backMat;
        materials[1] = mae.hittedMat;
        mae.skinRenderer.materials = materials;
    }

    public virtual void EndHitMat()
    {
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = mae.backMat;
        materials[1] = mae.idleMat;
        mae.skinRenderer.materials = materials;

        Debug.Log("�⺻ ���׸���� ����");
    }

    #endregion

    [Header("������������ ����� ������ ������Ʈ")]
    public GameObject flatObject;
    public float flatScaleY;
    public float flatTime=0;
    public float timer;
    [HideInInspector] public bool onFlat;
    Vector3 originScale;
    Vector3 flatScale;
    //�����ϰ� �Ǵ� �Լ�
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

        if (mae != null)
        {
            Material[] materials = mae.skinRenderer.materials;
            materials[1] = mae.hittedMat;
            mae.skinRenderer.materials = materials;
        }
    }

    public virtual void RollBackFormFlatState()
    {
        onStun = false;
        onFlat = false;
        timer = 0;
        if (mae != null)
        {
            Material[] materials = mae.skinRenderer.materials;
            materials[1] = mae.idleMat;
            mae.skinRenderer.materials = materials;
        }

        flatObject.transform.localScale = originScale;
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
  protected  bool onmove;
    #region �̵��Լ�
  
    
    public override void Move()
    {

        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {

            if (tracking)
            {
                if (!activeAttack && !onAttack)
                {
                    if (movepattern == EnemyMovePattern.patrol)
                    {
                        if (patrolType == PatrolType.movePatrol && searchCollider.onPatrol)
                        {

                            PatrolTracking();
                        }
                    }
                    if (searchCollider.searchPlayer)
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
    #region �߰�
    public virtual void TrackingMove()
    {
        testTarget = target.position - transform.position;
        //var vector = testTarget;
        testTarget.y = 0;
        tap.disToPlayer = testTarget.magnitude;

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
        float f = testTarget.z - transform.position.z; // -> ���밪�� �Ͽ� z���� n���� ũ�� false�� ������������ 
        f = Mathf.Abs(f);
        disToPlayer = a.magnitude;*/
        if (!callCheck)
        {
            if (tap.disToPlayer > tap.trackingDistance /*|| f > 6*/)
            {
                searchCollider.searchPlayer = false;
                target = null;
                searchCollider.onPatrol = true;
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
    
        testTarget = tap.targetPatrol - transform.position;
        testTarget.y = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(testTarget), eStat.rotationSpeed * Time.deltaTime);

        if (SetRotation())
        {
          
            enemymovepattern();
            PlayMoveSound();
        }
        if (testTarget.magnitude < tap.patrolDistance)
        {
            tracking = false;
            StartCoroutine(InitPatrolTarget());
        }
    
    }

    bool setPatrol;

  protected  IEnumerator InitPatrolTarget()
    {
        yield return new WaitForSeconds(tap.patrolWaitTime);        
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
        if (tap.patrolGroup.Length >=2)
        {
            tap.patrolGroup[0].x = tap.leftPatrol.x - tap.leftPatrolRange;
            tap.patrolGroup[0].y = transform.position.y;
            tap.patrolGroup[0].z = transform.position.z;

            tap.patrolGroup[1].x = tap.rightPatrol.x + tap.rightPatrolRange;
            tap.patrolGroup[1].y = transform.position.y;
            tap.patrolGroup[1].z = transform.position.z;
        }
    }

    public bool SetPatrolTarget()
    {
        int randomTarget = Random.Range(0, tap.patrolGroup.Length);
        if (tap.patrolGroup.Length >= 2)
        {
            if (tap.targetPatrol == tap.patrolGroup[randomTarget])
            {
                setPatrol = true;
            }
            else
            {
                tap.targetPatrol = tap.patrolGroup[randomTarget];
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
    [Header("#�����̼Ƿ���(�⺻������ 85)")]
    public float rotLevel;
    public float testAngle;
    public virtual bool SetRotation()
    {
        bool completeRot = false;
        if (target != null && !searchCollider.onPatrol)
        {
            Vector3 targetTothis = target.position - transform.position;
            targetTothis.y = 0;
            Quaternion q = Quaternion.LookRotation(targetTothis);
            testAngle = Quaternion.Angle(transform.rotation, q);
            if (testAngle < 45f)
                completeRot = true;
        }
        else if (searchCollider.onPatrol)
        {
            Vector3 patrolTothis = tap.targetPatrol - transform.position;
            patrolTothis.y = 0;
            Quaternion q = Quaternion.LookRotation(patrolTothis);
            testAngle = Quaternion.Angle(transform.rotation, q);
            if (testAngle < 1.5f)
                completeRot = true;
        }
        return completeRot;

    }
    #endregion

    #region ����

    private void OnDrawGizmos()
    {
        if (!eStat)
        {
            eStat = GetComponent<EnemyStat>();
        }

        if (patrolType == PatrolType.movePatrol)
        {
            if (tap.patrolGroup.Length >= 2)
            {
                center = (tap.patrolGroup[0] + tap.patrolGroup[1]) / 2; //s
                float xPoint = tap.patrolGroup[1].x - tap.patrolGroup[0].x;
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
                p1.x = p1.x - tap.leftPatrolRange*2;
                p2.x = p2.x + tap.rightPatrolRange*2;
                center = (p1 + p2) / 2;
                float xPoint = p2.x - p1.x;
                Vector3 size = new(xPoint, yWidth, zWidth);

                if (CharColliderColor.instance != null)
                    Gizmos.color = CharColliderColor.instance.patrolRange;
                else
                    Gizmos.color = Color.red;
                Gizmos.DrawWireCube(center, size);
                tap.targetPatrol = p2;

                ForwardWallRayCheck();
                UpWallRayCheck();               
                WallCheckResult();
            }
            if (CharColliderColor.instance != null)
                Gizmos.color = CharColliderColor.instance.trackingRange;
            else
                Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, tap.trackingDistance);           
        }        
    }

    #endregion

    #endregion

    #region ����Լ�
    public override void Dead()
    {
        base.Dead();
        eStat.eState = EnemyState.dead;
        if (PlayerHandler.instance != null)
            PlayerHandler.instance.CurrentPlayer.wallcheck = false;

        if (mae != null)
        {
            if (TryGetComponent<RagdolEnemy>(out RagdolEnemy enemy))
            {
                if (enemy.isRagdoll)
                {
                    enemy.RagdolDeadEffect(mae.deadEffect);
                }
                else
                {
                    Instantiate(mae.deadEffect, transform.position, Quaternion.identity);
                }
            }
            else
            Instantiate(mae.deadEffect, transform.position, Quaternion.identity);
        }
        if (transform.parent.gameObject.TryGetComponent<EnemyEnable>(out EnemyEnable enable))
        {
            transform.parent.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
    #endregion

    #region �����Լ�

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

    public void AddEnviromentPower(Vector3 power)
    {
        environmentforce = power;
    }
    #endregion

}
