using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleportOutputEvent : OutputEvent
{
    public GameObject selectEnemy;
    public Transform teleportPoint;
    public override void output()
    {
        throw new System.NotImplementedException();
    }

    public void EnemyTeleport()
    {
        if (selectEnemy != null && teleportPoint != null)
        {
            selectEnemy.transform.position = teleportPoint.position;
        }
    }
}
