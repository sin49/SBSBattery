using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnOutputEvent : OutputEvent
{
    public int index;
    [Header("몬스터 오브젝트 할당")]
    public GameObject enemy;
    [Header("좌표 오브젝트 할당\n(대량 생산이 아닐 때)")]
    public Transform spawnPoint;

    [Header("스폰 위치가 되는 트랜스폼 오브젝트\n이건 이전에 쌓이는 TV 몹처럼 한 번에 대량 스폰할 좌표들 있을때")]
    public List<Transform> spawnPosGroup;
    
    [Header("몬스터 생성을 막을 신호를 받는 오브젝트 (셔터)")]
    public signalReceiver signalReceiver;
    [Header("추가되어있는\n몬스터 킬 입력 이벤트를 넣어주세요")]
    public EnemyKillInputEvent eKillEvent;
    [Header("스폰되는 시간 간격")]
    public float spawnTime;

    public override void output()
    {
        Debug.Log("플레이어에 의한 인덱스 몬스터");
        base.output();
        if (signalReceiver != null && spawnPosGroup.Count !=0)
            StartCoroutine(SpawnCondition());
        else if (signalReceiver!= null)
        {
            StartCoroutine(SpawnNoGroup());
        }
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
        while (!signalReceiver.active)
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

    IEnumerator SpawnNoGroup()
    {
        while (!signalReceiver.active)
        {
            var monster = Instantiate(enemy, spawnPoint.position, Quaternion.identity);
            if (eKillEvent != null)
                eKillEvent.AddEnemyDeadEvent(monster);

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
