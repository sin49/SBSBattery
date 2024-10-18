using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveOutputEvent : OutputEvent
{
    public Enemy enemyPrefab;
    public Transform movePoint;

    public override void output()
    {
        SelectEnemyMove();
        base.output();
    }

    public void SelectEnemyMove()
    {
        if (enemyPrefab != null & movePoint != null)
        {
            enemyPrefab.tap.targetPatrol = movePoint.position;
            enemyPrefab.tap.tracking = true;
        }
    }
}
