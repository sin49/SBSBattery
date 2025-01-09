using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorResistrict : MonoBehaviour
{
    CursorInteractObject cursorPlatform;

    private void Awake()
    {
        cursorPlatform = GetComponentInParent<CursorInteractObject>();
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (cursorPlatform.CompareTag("CursorObject") && cursorPlatform.caught)
    //    {
    //        if (other.CompareTag("Ground") || other.CompareTag("InteractivePlatform"))
    //        {
    //            if (PlayerHandler.instance != null)
    //            {
    //                Player player = PlayerHandler.instance.CurrentPlayer;
    //                player.cantmove = true;
    //                player.playerRb.velocity = new(0, player.playerRb.velocity.y, 0);
    //            }
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Ground") || other.CompareTag("InteractivePlatform"))
    //    {
    //        if (PlayerHandler.instance != null)
    //        {
    //            Player player = PlayerHandler.instance.CurrentPlayer;
    //            player.cantmove = false;
    //        }
    //    }
    //}
}
