using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    bool tracking;
    bool isMove;

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
    }

    private void Update()
    {
        ReadyAttackTime();
    }

    public void FixedUpdate()
    {
        if (animator != null)
        {
            if (!onAttack)
            {
                isMove = true;
            }
            else
            {
                isMove = false;
            }

            animator.SetBool("isMove", isMove);
        }

        if (!onAttack)
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

            if (vec.magnitude > 0.5f)
            {
                rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
            }
        }
    }
}
