using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAreaOutputEvent : OutputEvent
{
    public Collider killCollider;
    public override void output()
    {
        EnemyKillOnArea();
        base.output();
    }

    public void EnemyKillOnArea()
    {
        if (killCollider != null)
        {
            if (killCollider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = killCollider.gameObject.GetComponent<Enemy>();
                enemy.Dead();
            }
        }
    }
}
