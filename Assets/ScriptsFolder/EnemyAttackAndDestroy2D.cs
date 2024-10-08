using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAndDestroy2D : EnemyAttackFor2D
{
    public GameObject DestroyEffect;
    protected override void Attack()
    {
        base.Attack();
        if(DestroyEffect!=null)
            Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        Destroy(transform.parent. gameObject);
    }
}
