using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleportOutputEvent : OutputEvent
{
    public GameObject selectEnemy;
    public Transform teleportPoint;
    public List<Transform> telpoGroup = new List<Transform>();

    public bool reduceNum, increaseNum;
    int pointNum;

    public override void output()
    {
        EnemyTeleport();
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
        if (selectEnemy != null && teleportPoint != null)
        {
            selectEnemy.transform.position = teleportPoint.position;            
        }
        else
        {
            Debug.Log("지정된 적, 텔레포트 지점이 설정되어있지 않습니다.");
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
        if (telpoGroup.Count > 0)
        {
            if (pointNum >= telpoGroup.Count)
                pointNum = 0;
            else if (pointNum < 0)
                pointNum = telpoGroup.Count - 1;

            teleportPoint = telpoGroup[pointNum];
        }
    }
}
