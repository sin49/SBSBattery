using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyKillOutputEvent : OutputEvent
{
    [Header("몬스터들을 하위로 가지고있는 오브젝트")]
    public GameObject obj;

    [Header("오브젝트 그룹")]
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
            Debug.Log("EnemyKillOutputEvent\n지정된 오브젝트 안에 몬스터가 존재하지 않습니다");
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
