using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillInputEvent : InputEvent
{
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
        EnemyKill();
        return eKill;
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
        if (eKill)
        {
            foreach (Transform transform in this.obj.transform)
            {
                transform.GetComponent<Enemy>().Dead();
            }
            eKill = false;
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
