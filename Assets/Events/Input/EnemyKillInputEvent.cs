using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillInputEvent : InputEvent
{
    public GameObject enemy;

    public GameObject obj;
    public List<GameObject> objGroup = new List<GameObject>();
    public bool eKill;
    public bool reduceNum, increaseNum;

    int pointNum;

    public override void initialize()
    {
        eKill = false;
    }

    public override bool input(object o)
    {        
        return eKill;
    }

    private void Update()
    {
        ChangePoint();
        EnemyKill();
    }

    public void EnemyKill()
    {
        if (enemy != null)
        {
            if (enemy.GetComponent<Enemy>().eStat.eState == EnemyState.dead)
            {
                eKill = true;
            }
        }
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
