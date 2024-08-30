using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyRemoveOutputEvent : OutputEvent
{
    public GameObject selectEnemy;
    public Transform ePoint;
    public List<Transform> ePointGroup = new List<Transform>();
    public List<GameObject> eGroup = new List<GameObject>();

    [Header("��ǥ �׷� �������� �ѱ��")]
    public bool previousPointNum;
    [Header("��ǥ �׷� ���������� �ѱ��")]
    public bool nextPointNum;
    [Header("���� ���� �� ����")]
    public bool previousEnemyNum;
    public bool nextEnemyNum;

    int pointNum, enemyNum;

    public override void output()
    {
        EnemySelectAndDelete();
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
    // ������ ���͸� ����
    public void EnemySelectAndDelete()
    {
        if (selectEnemy != null)
        {
            if (Application.isPlaying)            
                Destroy(selectEnemy);            
            else
                DestroyImmediate(selectEnemy);

            eGroup.RemoveAt(enemyNum);

            if (eGroup.Count > 0)
            {
                enemyNum = 0;
                selectEnemy = eGroup[enemyNum];
            }
            else
            {
                selectEnemy = null;
            }
        }
        else
        {
            Debug.Log("�� ���� ����̺�Ʈ�� ���� �����Ǿ����� �ʽ��ϴ�.");
        }
    }
    //���� ��ǥ�� �����ϴ� �Լ�
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

            ePoint = ePointGroup[pointNum];
            if (ePoint.childCount > 0)
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
        if (eGroup.Count < ePoint.childCount)
        {
            for (int i = 0; i < ePoint.childCount; i++)
            {
                eGroup.Add(ePoint.GetChild(i).gameObject);
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
}
