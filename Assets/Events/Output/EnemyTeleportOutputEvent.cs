using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleportOutputEvent : OutputEvent
{
    [Header("������ ���� ������Ʈ**�ʼ�**")]
    public GameObject selectEnemy;
    [Header("������ �ڷ���Ʈ**�ʼ�**")]
    public Transform teleportPoint;
    [Header("���õ� ��ǥ")]
    public Transform ePoint;
    [Header("��ǥ �׷�")]
    public List<Transform> ePointGroup = new List<Transform>();
    [Header("���� �׷�")]
    public List<GameObject> eGroup = new List<GameObject>();

    [Header("ePointGroup ���� ��ȣ")] public bool previousPointNum;
    [Header("ePointGroup ���� ��ȣ")] public bool nextPointNum;

    [Header("eGroup ���� ��ȣ")]public bool previousEnemyNum;
    [Header("eGroup ���� ��ȣ")]public bool nextEnemyNum;

    [Header("���� ���� Ʈ������")]public bool previousTelpoNum;
    [Header("���� ���� Ʈ������")]public bool nextTelpoNum;

    int pointNum, enemyNum, telpoNum;

    public override void output()
    {
        EnemyTeleport();
        base.output();
    }

    private void Update()
    {
        ChangePoint();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            ChangePoint();
    }

    public void EnemyTeleport()
    {
        if (selectEnemy != null && teleportPoint.gameObject != null)
        {
            selectEnemy.transform.position = teleportPoint.position;
            selectEnemy.transform.parent = teleportPoint;
        }
        else
        {
            Debug.Log("������ �� Ȥ�� �ڷ���Ʈ ������ �����Ǿ����� �ʽ��ϴ�.");
        }
    }

    public void ChangePoint()
    {
        if (previousPointNum)
        {
            previousPointNum = false;
            pointNum--;
            InitPoint();
        }
        
        if (nextPointNum)
        {
            nextPointNum = false;
            pointNum++;
            InitPoint();
        }

        if (previousEnemyNum)
        {
            previousEnemyNum = false;
            enemyNum--;
            InitEnemy();
        }
        if (nextEnemyNum)
        {
            nextEnemyNum = false;
            enemyNum++;
            InitEnemy();
        }


        if (previousTelpoNum)
        {
            previousTelpoNum = false;
            telpoNum--;
            InitTeleport();
        }

        if (nextTelpoNum)
        {
            nextTelpoNum = false;
            telpoNum++;
            InitTeleport();
        }
    }

    //���� ��ǥ �ʱ�ȭ
    public void InitPoint()
    {
        if (ePointGroup.Count > 0)
        {
            if (pointNum >= ePointGroup.Count)
            {
                pointNum = 0;
            }
            else if (pointNum < 0)
            {
                pointNum = ePointGroup.Count - 1;
            }

            teleportPoint = ePointGroup[pointNum];
            telpoNum = pointNum;
            ePoint = ePointGroup[pointNum];
            if (teleportPoint.childCount > 0)
            {
                InitEnemyWithPoint();
            }
            else
            {
                eGroup.Clear();
                selectEnemy = null;
                Debug.Log("���Ͱ� �������� ����");
            }
        }
    }
    //���� ��ǥ�� �Բ� ���� ���͵� �ʱ�ȭ
    public void InitEnemyWithPoint()
    {
        eGroup.Clear();
        enemyNum = 0;
        if (eGroup.Count < teleportPoint.childCount)
        {
            for (int i = 0; i < teleportPoint.childCount; i++)
            {
                eGroup.Add(teleportPoint.GetChild(i).gameObject);
            }
            selectEnemy = eGroup[enemyNum];
        }
    }
    //���� ���Ϳ� ���� ����Ʈ �ʱ�ȭ
    public void InitEnemy()
    {
        if (eGroup.Count > 0)
        {
            if (enemyNum >= eGroup.Count)
            {
                enemyNum = 0;
            }
            else if (enemyNum < 0)
            {
                enemyNum = eGroup.Count - 1;
            }

            selectEnemy = eGroup[enemyNum];
        }
    }
    //�ڷ���Ʈ �ʱ�ȭ
    public void InitTeleport()
    {
        if (ePointGroup.Count > 0)
        {
            if (telpoNum >= ePointGroup.Count)
            {
                telpoNum = 0;
            }
            else if (telpoNum < 0)
            {
                telpoNum = ePointGroup.Count - 1;
            }            
        }

        teleportPoint = ePointGroup[telpoNum];
    }
}
