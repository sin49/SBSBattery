using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnOutputEvent : OutputEvent
{
    public int index;
    public Transform spawnPoint;
    public override void output()
    {
        Debug.Log("플레이어에 의한 인덱스 몬스터");
        base.output();
    }
}
