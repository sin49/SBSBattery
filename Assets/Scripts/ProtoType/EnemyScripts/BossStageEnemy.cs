using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BossStageEnemy : Character, DamagedByPAttack
{
    public Transform target;
    [Header("애니메이션 관련")]
    public Animator animator;
    public Material idleMat;
    public Material hittedMat;
    public Renderer skinRenderer;

    public BossStageMeleeAttack meleeAttack;
    public GameObject deathEffect;
    [Header("스탯")]
    public float hpMax;
    public float hp;
    public float damage;
    public float moveSpeed;
    public float rotateSpeed;
    [Header("공격 딜레이")]
    public float attackTimer;
    public float attackTimerMax;
    public float attackDelay;
    public float knockbackForce;

    public bool onAttack;
    public bool activeAttack;
    [HideInInspector]
    public bool completeSpawn;
    [HideInInspector]
    public bool attackRange;
    bool tracking;
    bool isMove;

    [Header("스폰 포물선 관련")]
    Vector3 targetPos, rotPos;
    Vector3 vec;
    public Vector3 distanceValue;    
    public float disToTarget;
    public Vector3 min, max;
    public Vector3 forceInfo;

    public float testValue;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        hp = hpMax;
        attackTimer = attackTimerMax;
        if (PlayerHandler.instance.CurrentPlayer != null)
        {
            target = PlayerHandler.instance.CurrentPlayer.transform;
        }
        RandomDistanceValue();
        SetTargetPoint();
    }
    
    public void RandomDistanceValue()
    {
        distanceValue.x = Random.Range(min.x, max.x);
        distanceValue.y = Random.Range(min.y, max.y);
        distanceValue.z = Random.Range(min.z, max.z);
    }

    public void SetTargetPoint()
    {
        InitRotation();
        SpawnJump(distanceValue);        
    }

    public void InitRotation()
    {
        rotPos = transform.position + distanceValue;
        vec = rotPos - transform.position;
        Vector3 normalVec = vec.normalized;
        Vector3 angleAxis = Vector3.Cross(transform.forward, normalVec);
        float angle = Mathf.Acos(Vector3.Dot(transform.forward, normalVec)) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, angleAxis);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);        
    }

    public void SpawnJump(Vector3 value)
    {
        float v_y = Mathf.Sqrt(2 * -Physics.gravity.y * value.y);

        float v_x = value.x * v_y / (2 * value.y);

        float v_z = value.z * v_y / (2 * value.y);

        Vector3 force = rb.mass * (new Vector3(v_x, v_y, v_z) - rb.velocity);
        forceInfo = force;
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void Update()
    {
        ReadyAttackTime();

        if (!completeSpawn)
        {
            vec = rotPos - transform.position;            
            disToTarget = vec.magnitude;
            if (disToTarget < 0.1f)
            {
                completeSpawn = true;
                //rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
    }

    public void FixedUpdate()
    {
        if (animator != null)
        {
            if (!onAttack && completeSpawn && !attackRange)
            {
                isMove = true;
            }
            else
            {
                isMove = false;
            }

            animator.SetBool("isMove", isMove);
        }

        if (!onAttack && completeSpawn && !attackRange)
        {
            Move();            
        }
    }

    public void ReadyAttackTime()
    {
        if (onAttack && !activeAttack)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                activeAttack = true;
                Attack();
            }
        }
    }

    public void InitAttackCoolTime()
    {
        onAttack = false;
        activeAttack = false;
        attackTimer = attackTimerMax;
    }

    public override void Attack()
    {
        animator.Play("EnemyAttack");
        meleeAttack.MeleeAttack();
        StartCoroutine(WaitAttackDelay());
    }

    IEnumerator WaitAttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        InitAttackCoolTime();
    }

    public override void Damaged(float damage)
    {
        if (!completeSpawn)
            completeSpawn = true;
        hp -= damage;
        if (hp <= 0)
        {
            Dead();
        }

        if (animator != null)
        {
            animator.SetTrigger("isHitted");
            activeAttack = true;
            attackTimer = attackTimerMax;
            Material[] materials = skinRenderer.materials;
            materials[1] = hittedMat;
            skinRenderer.materials = materials;
        }
        rb.velocity = Vector3.zero;
        rb.AddForce(-transform.forward * knockbackForce, ForceMode.Force);
    }

    public override void Dead()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public override void Move()
    {
        if (target != null)
        {
            Vector3 vec = target.position - transform.position;
            vec.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(vec);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, rotateSpeed * Time.deltaTime);

            testValue = Quaternion.Angle(transform.rotation, lookRot);
            rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
            /*if (testValue < 0.5f)
            {
                rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
            }*/
        }
    }    
}
