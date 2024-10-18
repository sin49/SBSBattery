using System;
using UnityEngine;

public enum AttackType { melee, range}

public enum PlayerState { idle, hitted, dead, attack }
public enum EnemyState { idle, patrol, tracking, hitted, attack, dead }
public enum PatrolType { none, movePatrol }
public enum State { none, wet}

public class CharacterStat : MonoBehaviour
{
    [Header("캐릭터 능력치")]
    public AttackType attackType;
    [HideInInspector]
    public State characterState;

    [Header("체력 초기치")]
    public float initMaxHP ;
    [Header("이동속도 초기치")]
    public float initMoveSpeed;
    [HideInInspector]
    public float HPBonus;
    [HideInInspector]
    public float MoveSpeedBonus; 
    public float hpMax { get { return initMaxHP + HPBonus; } }
    [Header("#기본적인 스탯")]
    public float atk; // 공격력
    public float moveSpeed { get { return initMoveSpeed + MoveSpeedBonus; } }
    //[Header("캐릭터 회전 속도")]
    //public float rotationSpeed; // 캐릭터의 방향 전환 속도
    [HideInInspector]
    public float attackSpeed;
    [HideInInspector]
    public float attackCoolTimebonus;

    [Header("#공격 활성화 시 준비시간#")]
    public float initattackCoolTime;
    public float attackCoolTime { get { if (initattackCoolTime <= attackCoolTimebonus) return 0.1f; return initattackCoolTime - attackCoolTime; } } // 공격 딜레이
    [Header("공격 후 대기시간")]
    public float attackDelay; // 공격 후 딜레이


    [HideInInspector]
    public bool canMove; // 이동 가능 여부 체크
    [HideInInspector]
    public bool canAttack; // 공격 가능 여부 체크
}
