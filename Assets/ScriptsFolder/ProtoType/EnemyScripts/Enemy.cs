using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyMovePattern { stop,patrol}
public enum EnemyMoveType {none,basic,jump }

public enum EnemyAttackType { none,melee,range,breath,rush}
public interface DamagedByPAttack
{
    public void Damaged(float f);
}


public class Enemy: Character,DamagedByPAttack,environmentObject
{
    [Header("식별번호")] public int PriorityNumber;




    [Header("몬스터가 피격당하는 2D 콜라이더")]public GameObject EnemyHitCol2D;

   
    public EnemyStat eStat;
 
    //public Rigidbody enemyRb; // 적 리지드바디
    [Header("몬스터 공격 콜라이더(원거리일 경우 위치로 발사)")]public GameObject attackCollider; // 적의 공격 콜라이더 오브젝트        
  
    bool posRetry;
    [Header("적 애니메이션 관련")]
    public Animator animaor;


    [HideInInspector] public Vector3 environmentforce;
    [HideInInspector] public bool isMove;

    [Header("추적/탐색 관련 스크립트")]public EnemyTrackingAndPatrol tap;    
    [Header("기본/피격 머티리얼 관련 스크립트")]public EnemyMaterialAndEffect mae;
    [Header("몬스터 접촉 데미지 스크립트")]public ReachAttack reachAttack;

    
    [HideInInspector]public float attackTimer; // 공격 대기시간
                                               //public float attackInitCoolTime; // 공격 대기시간 초기화 변수
                                               //[Header("몬스터 공격 핸들러?")]public EnemyAttackHandler actionhandler;    
    
    public NormalEnemyAction MoveAction;
    public NormalEnemyAction AttackAction;


    [HideInInspector] public bool activeAttack; // 공격 가능한 상태인지 체크            

    [Header("기절상태")]
    [HideInInspector]public  bool onStun;
    protected bool die, hitted;
    
    private void OnEnable()
    {
        //StartCoroutine(InitPatrolTarget());
        initializeEnemy();
    }

    protected override void Awake()
    {

        base.Awake();
        if (attackCollider != null)
            attackCollider.SetActive(false);
        eStat = GetComponent<EnemyStat>();
        if(tap.attackColliderRange!=null)
        tap.attackColliderRange.enemy = this;
        tap.attackColliderRange.tap = tap;
        if (tap.searchCollider_ != null)
            tap.searchCollider_.tap = tap;

       

        if (flatObject != null)
        {
            originScale = flatObject.transform.localScale;
            flatScale = new(flatObject.transform.localScale.x, flatScaleY, flatObject.transform.localScale.z);
        }


    }

    public void TestAttack()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            base.Attack();
            if (animaor != null)
                animaor.Play("EnemyAttack");
            PlayAttackSound();
            AttackAction.Invoke(this.gameObject.transform);
        }
    }


    //public void InitPatrolPoint()
    //{
    //    mae.searchCollider.onPatrol = true;
    //    tap.SetPoint(transform);
    //}    
    public void getstatusfromtable()
    {
        if (ETableManager.instance != null)
        {
            var datas = ETableManager.instance.returnenemydata(PriorityNumber);
            enemystattest s = datas;

            switch (s.attackstateID)
            {
                case 1:
                    AttackAction = gameObject.AddComponent<EnemyAction_Swing>();
                    break;
                case 2:
                    AttackAction = gameObject.AddComponent<EnemyAction_Throwing>();
                    break;
                case 3:
                    var obj = gameObject.AddComponent<Enemy_Action_rush>();
                    var data = ETableManager.instance.enemyattacks[3];
                    obj.rushtime = data.SpecialVaule[0];
                    obj.rushspeed = data.SpecialVaule[1];
                    obj.PlayerForce = data.SpecialVaule[2];
                    AttackAction = obj;
                    break;
                case 4:
                    var obj2 = gameObject.AddComponent<EnemyAction_breath>();
                    var data2 = ETableManager.instance.enemyattacks[4];
                    obj2.breathtime = data2.SpecialVaule[0];
                    obj2.breathspreadmaxtime = data2.SpecialVaule[1];
                    obj2.breathendtime = data2.SpecialVaule[2];
                    AttackAction = obj2;
                    break;
                default:

                    break;

            }


            AttackAction.register(this);
            switch (s.movestateid)
            {
                case 0:
                    MoveAction = null;
                    MoveAction.register(this);
                    break;
                case 1:
                    MoveAction = gameObject.AddComponent<ENemy_Action_BasicMove>();
                    MoveAction.register(this);
                    break;
                case 2:
                    MoveAction = gameObject.AddComponent<EnemyAction_jumpMove>();
                    MoveAction.register(this);
                    break;

            }
            eStat.initMaxHP = s.hp; eStat.initMoveSpeed = s.movespeed;
            eStat.initattackCoolTime = s.initattackdelay;
            eStat.attackDelay = s.afterattackdelay;
        }

    }
   void initializeEnemy()
    {
        eStat.hp = eStat.hpMax;
        die = false;
        hitted = false;
        activeAttack = false;
        
    }
    private void Start()
    {                
        attackTimer = eStat.initattackCoolTime;
        getstatusfromtable();

        if (reachAttack != null)
            reachAttack.SetDamage(eStat.atk);
        /*if (onStun)
        {         
            StartCoroutine(WaitStunTime());
        }*/
    }

    private void Update()
    {

        TestAttack();
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
    bool CanAttack;
    
    private void FixedUpdate()
    {
        /*if (searchPlayer)
            DistanceToPlayer();*/

        if (die || hitted)
            return;
            if (!onStun)
        {


            //if (!mae.attackColliderRange.attackRange)
            if (!CanAttack)
                Move();
        }
        tap.ForwardWallRayCheck();
        tap.UpWallRayCheck();
        tap.WallCheckResult();
        if(environmentforce
            !=Vector3.zero)
        {
            rb.AddForce(environmentforce, ForceMode.VelocityChange);
            environmentforce = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }

    //void DistanceToPlayer()
    //{
    //    if (target != null && PlayerHandler.instance.CurrentPlayer != null)
    //    {
    //        if (target == PlayerHandler.instance.CurrentPlayer)
    //        {
    //            testTarget = target.position - transform.position;
    //            testTarget.y = 0;
    //            tap.disToPlayer = testTarget.magnitude;
    //        }
    //    }
    //}

    public Vector3 direction;//목적지
    //public bool 행동여부;//
 
    

    public void EnemyAI()
    {
        if (tap.tracking)
        {

        }
    }

    #region 피격함수
    public virtual void HittedRotate()
    {

            if (PlayerHandler.instance.CurrentPlayer != null)
            {
               Vector3 target = PlayerHandler.instance.CurrentPlayer.transform.position;

                Vector3 pos = target - transform.position;
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
            HittedAttackEvent();
        }
    }

    public void HittedAttackEvent()
    {
        StopCoroutine("HittedEnd");
        if (!onStun)
        {
            rb.velocity = Vector3.zero;
            if (attackCollider != null)
                attackCollider.SetActive(false);
            if (soundplayer != null)
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

    #region 피격 코루틴
    public IEnumerator HittedEnd()
    {
        if (mae !=null)
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
            Debug.Log("피격 코루틴 복귀");
        }

        if (mae != null)
        {
            EndHitMat();
        }

        activeAttack = false;
    }

    //머티리얼 관련해서 현재는 가상함수처리하여 각 몬스터마다
    //오버라이드한 함수로 머티리얼 처리하고 있습니다
    //materialAndPatrol에서 각 몬스터에 맞게 처리하는 함수 생각중입니다.
    public virtual void StartEmmissionHitMat()
    {
        mae.StartEmmissionHitMat();
    }

    public virtual void EndEmmissionHitMat()
    {
        //Material[] materials = mae.skinRenderer.materials;
        //materials[0] = mae.backMat;
        //materials[1] = mae.hittedMat;
        //mae.skinRenderer.materials = materials;
        mae.EndEmmissionHitMat();
    }

    public virtual void EndHitMat()
    {
        //Material[] materials = mae.skinRenderer.materials;
        //materials[0] = mae.backMat;
        //materials[1] = mae.idleMat;
        //mae.skinRenderer.materials = materials;

        //Debug.Log("기본 머테리얼로 복귀");
        mae.EndHitMat();
    }

    #endregion

    #region 납작하게하는 기능 관련
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

        StartStun();

        onFlat = true;
        flatObject.transform.localScale = flatScale;
        flatTime = flat;

        Debug.Log(flat);

        if (mae != null && mae.skinRenderer != null)
        {
            Material[] materials = mae.skinRenderer.materials;
            materials[1] = mae.hittedMat;
            mae.skinRenderer.materials = materials;
        }
    }

    public virtual void RollBackFormFlatState()
    {
        EndStun();


        onFlat = false;
        timer = 0;
        if (mae != null && mae.skinRenderer != null)
        {
            Material[] materials = mae.skinRenderer.materials;
            materials[1] = mae.idleMat;
            mae.skinRenderer.materials = materials;
        }

        flatObject.transform.localScale = originScale;
    }    

    public void StartStun() // 납작해지게 하는 다리미 내려찍기에 맞았을 때 호출되는 함수
    {
        onStun = true;
     
        if(reachAttack!=null)
        reachAttack.onStun = false;
    }

    public void EndStun()
    {
        onStun = false;
   
        if(reachAttack!=null)
        reachAttack.onStun = false;
    }
    #endregion

    //IEnumerator HiiitedState()
    //{
    //    eStat.eState = EnemyState.hitted;
    //    yield return new WaitForSeconds(1f);
    //    if (!onAttack)
    //        eStat.eState = EnemyState.idle;
    //    else if(onAttack)
    //        eStat.eState = EnemyState.attack;
    //}
    #endregion
    protected bool onmove;
    #region 이동함수

   


    public Vector3 MoveDirection;


    public override void Move()
    {
        //if (movepattern == EnemyMovePattern.patrol)
        //{여기를 시스템화를 위한 밑작업으로 빼두기

         
                if ( !activeAttack && tap.tracking)
                {
            Vector3 target = tap.GetTarget();
                    transform.rotation = Quaternion.LookRotation(target);
            //enemymovepattern();
            MoveAction.Invoke();
                }

        //ismove = move 에니메이션 관련 변수->move쪽으로 옮기기
        //if (eStat. movepattern == EnemyMovePattern.stop)
        //{
        //    if (tap.tracking && !activeAttack && tap.PlayerDetected)
        //    {
        //        isMove = true;
        //    }
        //    else
        //    {
        //        isMove = false;
        //    }
        //}
        //else
        //{
            if (tap.tracking && !activeAttack)
            {
                isMove = true;
            }
            else
            {
                isMove = false;
            }
        //}
        MoveAnimationPlay();

        //}     
    }

 

    protected virtual void PlayMoveSound()
    {
        if (soundplayer != null)
            soundplayer.PlayMoveSound();
    }

    public virtual void enemymovepattern()
    {
        if (rb != null)
            rb.MovePosition(environmentforce + transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
    }

    #region DrawGizomo
    //정찰 포인트 그리는 건에 대해서는 
    //큐브에서 스피어 처리로 변경했습니다
    private void OnDrawGizmos()
    {
       

            if (tap.firstPoint != Vector3.zero && tap.secondPoint != Vector3.zero)
            {
                if (CharColliderColor.instance != null)
                    Gizmos.color = CharColliderColor.instance.patrolRange;
                else
                    Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(tap.firstPoint, 0.3f);
                Gizmos.DrawWireSphere(tap.secondPoint, 0.3f);
            }
            else
            {
                Vector3 p1 = transform.position;
                Vector3 p2 = transform.position;
                p1.x = p1.x - tap.leftPatrolRange * 2;
                p2.x = p2.x + tap.rightPatrolRange * 2;

                if (CharColliderColor.instance != null)
                    Gizmos.color = CharColliderColor.instance.patrolRange;
                else
                    Gizmos.color = Color.red;

                Gizmos.DrawWireSphere(p1, 0.3f);
                Gizmos.DrawWireSphere(p2, 0.3f);

                tap.ForwardWallRayCheck();
                tap.UpWallRayCheck();
                tap.WallCheckResult();
            }

            if (CharColliderColor.instance != null)
                Gizmos.color = CharColliderColor.instance.trackingRange;
            else
                Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, tap.trackingDistance);
        

/*        if (patrolType == PatrolType.movePatrol)
        {
            if (tap.patrolGroup.Length >= 2)
            {
                tap.center = (tap.firstPoint + tap.secondPoint) / 2; //s
                float xPoint = tap.patrolGroup[1].x - tap.patrolGroup[0].x;
                Vector3 size = new(xPoint, tap.yWidth, tap.zWidth);
                if (CharColliderColor.instance != null)
                    Gizmos.color = CharColliderColor.instance.patrolRange;
                else
                    Gizmos.color = Color.red;
                Gizmos.DrawWireCube(tap.center, size);
            }
            else
            {
                Vector3 p1 = transform.position;
                Vector3 p2 = transform.position;
                p1.x = p1.x - tap.leftPatrolRange * 2;
                p2.x = p2.x + tap.rightPatrolRange * 2;
                tap.center = (p1 + p2) / 2;
                float xPoint = p2.x - p1.x;
                Vector3 size = new(xPoint, tap.yWidth, tap.zWidth);

                if (CharColliderColor.instance != null)
                    Gizmos.color = CharColliderColor.instance.patrolRange;
                else
                    Gizmos.color = Color.red;
                Gizmos.DrawWireCube(tap.center, size);
                tap.targetPatrol = p2;

                tap.ForwardWallRayCheck();
                tap.UpWallRayCheck();
                tap.WallCheckResult();
            }
            if (CharColliderColor.instance != null)
                Gizmos.color = CharColliderColor.instance.trackingRange;
            else
                Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, tap.trackingDistance);
        }*/
    }

    #endregion

    #endregion

    #region 사망함수
    public override void Dead()
    {
        base.Dead();
        die = true;
        if (PlayerHandler.instance != null)
            PlayerHandler.instance.CurrentPlayer.wallcheck = false;

        if (mae != null && mae.deadEffect != null)
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
        AttackAction.Invoke(PlayerHandler.instance.CurrentPlayer.transform);
        DelayTime();
    }

    // 공격 준비시간
    public void ReadyAttackTime()
    {
        if (activeAttack && !die && !CanAttack)
        {            
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                CanAttack = true;
                activeAttack = false;
                Attack();
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
        CanAttack = false;
        attackTimer = eStat.initattackCoolTime;        
    }

    public void AddEnviromentPower(Vector3 power)
    {
        environmentforce = power;
    }
    #endregion

}
