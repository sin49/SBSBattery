using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnInputEvent : InputEvent
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public bool eSpawn;
    public override void initialize()
    {
        eSpawn = false;
    }

    public override bool input(object o)
    {
        return eSpawn;
    }

    public void EnemySpawn()
    {
        if (eSpawn)
        {
            var obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            obj.transform.SetParent(spawnPoint);
        }
    }
}
