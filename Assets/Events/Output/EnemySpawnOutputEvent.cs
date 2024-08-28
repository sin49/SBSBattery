using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnOutputEvent : OutputEvent
{
    [Header("인덱스 구현은 하지 않아서 불편하시더라도 직접 할당 부탁드립니다\n빠른 시일 내 구현해볼게요")]
    public int index;
    [Header("몬스터 오브젝트 할당")]
    public GameObject enemy;
    [Header("좌표 오브젝트 할당")]
    public Transform spawnPoint;
    public override void output()
    {
        Debug.Log("플레이어에 의한 인덱스 몬스터");
        base.output();
    }

    public void EnemySpawn()
    {
        if (enemy != null && spawnPoint != null)
        {
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        }
        else
            Debug.Log("몹 스폰 출력 이벤트에서 몹 혹은 좌표 지정되지 않음");
    }
}
