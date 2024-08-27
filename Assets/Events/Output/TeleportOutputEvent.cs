using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOutputEvent : OutputEvent
{
    public Transform teleportPoint;
    public Transform point;
    public List<Transform> pointGroup = new List<Transform>();

    [Header("pointGroup")]
    public bool previousPointNum;
    public bool nextPointNum;
    [Header("이전/다음 텔포 트랜스폼")]
    public bool previousTelpoNum;
    public bool nextTelpoNum;

    int pointNum, telpoNum;

    public override void output()
    {
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
    public void PlayerTeleport()
    {
        if (teleportPoint != null)
        {
            if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer != null)
            {
                PlayerHandler.instance.CurrentPlayer.transform.position = teleportPoint.position;
            }
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

    public void InitPoint()
    {
        if (pointGroup.Count > 0)
        {
            if (pointNum >= pointGroup.Count)
                pointNum = 0;
            else if (pointNum < 0)
                pointNum = pointGroup.Count - 1;

            point = pointGroup[pointNum];
        }
    }
    public void InitTeleport()
    {
        if (pointGroup.Count > 0)
        {
            if (telpoNum >= pointGroup.Count)
                telpoNum = 0;
            else if (telpoNum < 0)
                telpoNum = pointGroup.Count - 1;

            teleportPoint = pointGroup[telpoNum];
        }
    }
}
