using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnOutputEvent : OutputEvent
{
    [Header("�ε��� ������ ���� �ʾƼ� �����Ͻô��� ���� �Ҵ� ��Ź�帳�ϴ�\n���� ���� �� �����غ��Կ�")]
    public int index;
    [Header("���� ������Ʈ �Ҵ�")]
    public GameObject enemy;
    [Header("��ǥ ������Ʈ �Ҵ�")]
    public Transform spawnPoint;

    [Header("���� ��ġ�� �Ǵ� Ʈ������ ������Ʈ")]
    public List<Transform> spawnPosGroup;

    [Header("�ñ׳��� �ִ� ������Ʈ �Ҵ�\n���⼭�� �������� ���͸�\nȰ��ȭ ��Ű�� ����ġ�� �ɰ���")]
    public signalSender signalSender;
    [Header("�߰��Ǿ��ִ�\n���� ų �Է� �̺�Ʈ�� �־��ּ���")]
    public EnemyKillInputEvent eKillEvent;
    [Header("�����Ǵ� �ð� ����")]
    public float spawnTime;

    public override void output()
    {
        Debug.Log("�÷��̾ ���� �ε��� ����");
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
            Debug.Log("�� ���� ��� �̺�Ʈ���� �� Ȥ�� ��ǥ �������� ����");
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
