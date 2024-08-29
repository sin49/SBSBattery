using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : InteractiveObject
{
    public bool onLadder;
    [Header("사다리 밀착 테스트용 값")]
    public float value;

    public void LadderActive()
    {
        if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer != null)
        {
            Vector3 fixPos = transform.position + transform.forward * value;
            Vector3 playerPos = PlayerHandler.instance.CurrentPlayer.transform.position;
            PlayerHandler.instance.CurrentPlayer.transform.position = new(fixPos.x, playerPos.y, fixPos.z);
            PlayerHandler.instance.CurrentPlayer.transform.GetChild(0).rotation = Quaternion.Euler(0, -transform.eulerAngles.y, 0);
        }
    }

    public override void Active(direction direct)
    {
        base.Active(direct);
        LadderActive();
        PlayerHandler.instance.ladderInteract = true;
        PlayerHandler.instance.CurrentPlayer.GetComponent<Rigidbody>().useGravity = false;
    }
}
