using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnOutputEvent : OutputEvent
{
    public int index;
    public Transform spawnPoint;
    public override void output()
    {
        Debug.Log("�÷��̾ ���� �ε��� ����");
        base.output();
    }
}
