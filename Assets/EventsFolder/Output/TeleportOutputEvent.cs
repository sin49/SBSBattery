using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOutputEvent : OutputEvent
{
    public Transform teleportPoint;
    public List<Transform> pointGroup = new List<Transform>();

    [Header("����/���� ���� Ʈ������")]
    public bool previousTelpoNum;
    public bool nextTelpoNum;

    int telpoNum;

    public override void output()
    {
        PlayerTeleport();
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
        Debug.Log("�Է� �̺�Ʈ�� ���� ȣ���");
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
