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
    public override void output()
    {
        Debug.Log("�÷��̾ ���� �ε��� ����");
        base.output();
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
}
