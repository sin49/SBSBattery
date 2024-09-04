using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnInputEvent : InputEvent
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public List<Transform> spawnGroup = new List<Transform>();

    public bool reduceNum, increaseNum;

    public bool eSpawn;
    int pointNum;
    public override void initialize()
    {
        eSpawn = false;
    }

    public override bool input(object o)
    {
        EnemySpawn();
        return eSpawn;
    }
    private void Update()
    {
        ChangePoint();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            ChangePoint();
        }
    }

    public void EnemySpawn()
    {
        if (eSpawn)
        {
            var obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            obj.transform.SetParent(spawnPoint);
            eSpawn = false;
        }
    }

    public void ChangePoint()
    {
        if (reduceNum)
        {
            reduceNum = false;
            pointNum--;
            InitPoint();
        }

        if (increaseNum)
        {
            increaseNum = false;
            pointNum++;
            InitPoint();
        }

    }

    public void InitPoint()
    {
        if (spawnGroup.Count > 0)
        {
            if (pointNum >= spawnGroup.Count)
                pointNum = 0;
            else if (pointNum < 0)
                pointNum = spawnGroup.Count - 1;
            spawnPoint = spawnGroup[pointNum];
        }
    }
}
