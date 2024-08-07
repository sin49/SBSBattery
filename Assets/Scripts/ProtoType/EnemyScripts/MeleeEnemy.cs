using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{    
    public override void Attack()
    {
        attackCollider.SetActive(true);
        attackCollider.GetComponent<EnemyMeleeAttack>().AttackReady(this, eStat.attackDelay);
    }
}
