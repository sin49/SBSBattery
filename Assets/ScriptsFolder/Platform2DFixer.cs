using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2DFixer : MonoBehaviour
{
    bool ChangeComplete;
    private void Start()
    {
        PlayerHandler.instance.registerCameraChangeAction(FixPlatform);
    }
    public void FixPlatform()
    {
        if (!(
              (int)PlayerStat.instance.MoveState >= 4))
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (ChangeComplete)
            {

                FixPlatform();
            }
        }
    }
}