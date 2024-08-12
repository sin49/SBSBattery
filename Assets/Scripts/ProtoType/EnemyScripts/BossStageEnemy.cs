using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageEnemy : Character, DamagedByPAttack
{
    public Transform target;

    public Animator animator;
    public Material idleMat;
    public Material hittedMat;

    public float hpMax;
    public float hp;
    public float damage;
    public float moveSpeed;

    public float attackTimer;
    public float attackTimerMax;
    public bool onAttack;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        attackTimer = attackTimerMax;
    }

    private void Update()
    {
        
    }

    public void FixedUpdate()
    {
        Move();
    }

    public void ReayAttackTime()
    {
        if (onAttack)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void InitAttackCoolTime()
    {
        onAttack = false;
        attackTimer = attackTimerMax;
    }

    public override void Attack()
    {

    }

    public override void Damaged(float damage)
    {

    }

    public override void Dead()
    {
        throw new System.NotImplementedException();
    }

    public override void Move()
    {

    }
}
