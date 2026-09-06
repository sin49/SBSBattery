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
        return eSpawn;
    }

    private void Start()
    {
        EnemySpawn();
    }

    private void Update()
    {
        ChangePoint();
    }

    public void EnemySpawn()
    {
        if (enemyPrefab != null && spawnPoint != null)
        {
            var obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            obj.transform.SetParent(spawnPoint);
            eSpawn = true;
        }
        else
        {
            Debug.Log("몬스터 스폰 입력 이벤트에 몬스터 혹은 좌표가 없습니다");
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
