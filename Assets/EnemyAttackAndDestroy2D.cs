using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAndDestroy2D : EnemyAttackFor2D
{
    protected override void Attack()
    {
        base.Attack();
        Destroy(transform.parent. gameObject);
    }
}
