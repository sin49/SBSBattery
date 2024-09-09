using System;
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

    [Header("스폰 위치가 되는 트랜스폼 오브젝트")]
    public List<Transform> spawnPosGroup;

    [Header("시그널을 주는 오브젝트 할당\n여기서는 떨어지는 셔터를\n활성화 시키는 스위치가 될거임")]
    public signalSender signalSender;
    [Header("추가되어있는\n몬스터 킬 입력 이벤트를 넣어주세요")]
    public EnemyKillInputEvent eKillEvent;
    [Header("스폰되는 시간 간격")]
    public float spawnTime;

    public override void output()
    {
        Debug.Log("플레이어에 의한 인덱스 몬스터");
        base.output();
        if (signalSender.gameObject != null && eKillEvent.gameObject != null)
            StartCoroutine(SpawnCondition());
        else
            EnemySpawn();
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

    IEnumerator SpawnCondition()
    {        
        while (!signalSender.active)
        {
            for (int i = 0; i < spawnPosGroup.Count; i++)
            {
                var monster = Instantiate(enemy, spawnPosGroup[i].position, Quaternion.identity);
                if (eKillEvent != null)
                    eKillEvent.AddEnemyDeadEvent(monster);
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
