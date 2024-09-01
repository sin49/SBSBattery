using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : InteractiveObject
{
    public bool onLadder;
    [Header("사다리 밀착 테스트용 값")]
    public float value;
    [Header("상호작용 UI가 표시될 트랜스폼")]
    public Transform upInteractPoint;
    public Transform downInteractPoint;
    public Transform resultPoint;

    public Transform upStartPoint, downStartPoint;
    float posY;

    public void LadderActive()
    {
        if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentPlayer != null)
        {
            Vector3 fixPos = transform.position + transform.forward * value;
            //Vector3 playerPos = PlayerHandler.instance.CurrentPlayer.transform.position;
            PlayerHandler.instance.CurrentPlayer.transform.position = new(fixPos.x, posY, fixPos.z);
            SetRotation();
        }
    }

    public void SetRotation()
    {
        PlayerHandler.instance.CurrentPlayer.transform.GetChild(0).rotation = Quaternion.Euler(0, -(180f - transform.eulerAngles.y), 0);
    }

    public override void Active(direction direct)
    {
        base.Active(direct);
        LadderActive();
        PlayerHandler.instance.ladderInteract = true;
        PlayerHandler.instance.CurrentPlayer.GetComponent<Rigidbody>().useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHandler.instance.ladderCheck = true;
            if (PlayerHandler.instance.CurrentPlayer.transform.position.y > transform.position.y)
            {
                resultPoint = upInteractPoint;
                posY = upStartPoint.position.y;
            }
            else
            {
                resultPoint = downInteractPoint;
                posY = downStartPoint.position.y;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHandler.instance.ladderCheck = false;
        }
    }
}
