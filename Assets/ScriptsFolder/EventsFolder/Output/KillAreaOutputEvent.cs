using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAreaOutputEvent : OutputEvent
{
    public KillAreaCollider killCollider;
    public override void output()
    {
        EnemyKillOnArea();
        base.output();
    }

    public void EnemyKillOnArea()
    {
        if (killCollider != null)
        {
            killCollider.EnemyAllDie();
        }
    }
}
