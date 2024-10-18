using System;
using UnityEngine;

public enum AttackType { melee, range}

public enum PlayerState { idle, hitted, dead, attack }
public enum EnemyState { idle, patrol, tracking, hitted, attack, dead }
public enum PatrolType { none, movePatrol }
public enum State { none, wet}

public class CharacterStat : MonoBehaviour
{
    [Header("ĳ���� �ɷ�ġ")]
    public AttackType attackType;
    [HideInInspector]
    public State characterState;

    [Header("ü�� �ʱ�ġ")]
    public float initMaxHP ;
    [Header("�̵��ӵ� �ʱ�ġ")]
    public float initMoveSpeed;
    [HideInInspector]
    public float HPBonus;
    [HideInInspector]
    public float MoveSpeedBonus; 
    public float hpMax { get { return initMaxHP + HPBonus; } }
    [Header("#�⺻���� ����")]
    public float atk; // ���ݷ�
    public float moveSpeed { get { return initMoveSpeed + MoveSpeedBonus; } }
    //[Header("ĳ���� ȸ�� �ӵ�")]
    //public float rotationSpeed; // ĳ������ ���� ��ȯ �ӵ�
    [HideInInspector]
    public float attackSpeed;
    [HideInInspector]
    public float attackCoolTimebonus;

    [Header("#���� Ȱ��ȭ �� �غ�ð�#")]
    public float initattackCoolTime;
    public float attackCoolTime { get { if (initattackCoolTime <= attackCoolTimebonus) return 0.1f; return initattackCoolTime - attackCoolTime; } } // ���� ������
    [Header("���� �� ���ð�")]
    public float attackDelay; // ���� �� ������


    [HideInInspector]
    public bool canMove; // �̵� ���� ���� üũ
    [HideInInspector]
    public bool canAttack; // ���� ���� ���� üũ
}
