using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : Enemy
{
    [Header("���� ĳ���� �׽�Ʈ ����")]
    public float rushForce; // ���� �ӵ�

    public override void Attack()
    {
        //���� �ݶ��̴� ������Ʈ Ȱ��ȭ
        attackCollider.SetActive(true);
        //������ ����
        enemyRb.AddForce(transform.forward * rushForce, ForceMode.Impulse);

        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, attackDelay);
    }
}
