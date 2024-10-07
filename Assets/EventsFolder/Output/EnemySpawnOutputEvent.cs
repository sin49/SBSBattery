using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnOutputEvent : OutputEvent
{
    public int index;
    [Header("���� ������Ʈ �Ҵ�")]
    public GameObject enemy;
    [Header("��ǥ ������Ʈ �Ҵ�\n(�뷮 ������ �ƴ� ��)")]
    public Transform spawnPoint;

    [Header("���� ��ġ�� �Ǵ� Ʈ������ ������Ʈ\n�̰� ������ ���̴� TV ��ó�� �� ���� �뷮 ������ ��ǥ�� ������")]
    public List<Transform> spawnPosGroup;
    
    [Header("���� ������ ���� ��ȣ�� �޴� ������Ʈ (����)")]
    public signalReceiver signalReceiver;
    [Header("�߰��Ǿ��ִ�\n���� ų �Է� �̺�Ʈ�� �־��ּ���")]
    public EnemyKillInputEvent eKillEvent;
    [Header("�����Ǵ� �ð� ����")]
    public float spawnTime;

    public override void output()
    {
        Debug.Log("�÷��̾ ���� �ε��� ����");
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
            Debug.Log("�� ���� ��� �̺�Ʈ���� �� Ȥ�� ��ǥ �������� ����");
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
