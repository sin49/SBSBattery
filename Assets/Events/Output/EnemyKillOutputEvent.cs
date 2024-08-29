using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyKillOutputEvent : OutputEvent
{
    [Header("���͵��� ������ �������ִ� ������Ʈ")]
    public GameObject obj;

    [Header("������Ʈ �׷�")]
    public List<GameObject> objGroup = new List<GameObject>();
    public bool reduceNum, increaseNum;
    int pointNum;

    public override void output()
    {
        EnemyKill();
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

    public void EnemyKill()
    {
        if (obj.transform.childCount > 0)
        {
            foreach (var child in objGroup)
            {
                child.GetComponent<Enemy>().Dead();
            }
        }
        else
            Debug.Log("EnemyKillOutputEvent\n������ ������Ʈ �ȿ� ���Ͱ� �������� �ʽ��ϴ�");
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
        if (objGroup.Count > 0)
        {
            if (pointNum >= objGroup.Count)
                pointNum = 0;
            else if (pointNum < 0)
                pointNum = objGroup.Count - 1;

            obj = objGroup[pointNum];
        }
    }
}
