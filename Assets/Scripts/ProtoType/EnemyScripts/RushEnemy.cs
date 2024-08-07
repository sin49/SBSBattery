using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : Enemy
{
    [Header("돌진 캐릭터 테스트 변수")]
    public float rushForce; // 돌진 속도

    public override void Attack()
    {
        //공격 콜라이더 오브젝트 활성화
        attackCollider.SetActive(true);
        //앞으로 돌진
        enemyRb.AddForce(transform.forward * rushForce, ForceMode.Impulse);

        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, attackDelay);
    }
}
