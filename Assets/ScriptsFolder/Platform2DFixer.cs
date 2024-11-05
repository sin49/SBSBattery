using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2DFixer : MonoBehaviour
{
    bool ChangeComplete;
    bool oncollide;
    private void Start()
    {
        PlayerHandler.instance.registerCameraChangeAction(FixPlatform);
    }
    public void FixPlatform()
    {
        if (!ChangeComplete&&oncollide&&!PlayerHandler.instance.Changing&& !(
              (int)PlayerStat.instance.MoveState >= 4)&&!PlayerHandler.instance.CantHandle&&!PlayerHandler.instance.CurrentPlayer.cantmove)
        {

            Transform player = PlayerHandler.instance.CurrentPlayer.transform;
            player.position = new Vector3(player.position.x, player.position.y, this.transform.position.z);
            ChangeComplete = true;
        }
        else
        {
            ChangeComplete = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ChangeComplete = false;
        oncollide = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            // 이 부분을 추가했음 뭔가 이상한 문제 생기면 지우샘
            if (PlayerHandler.instance.CurrentPlayer.transform.position.y > this.transform.position.y)
                oncollide = true;
                FixPlatform();
        
        }
    }
}