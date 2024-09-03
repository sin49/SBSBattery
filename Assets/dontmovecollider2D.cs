using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dontmovecollider2D : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            
            if (PlayerHandler.instance.CurrentPlayer != null)
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            var vel = PlayerHandler.instance.CurrentPlayer.playerRb.velocity;
            PlayerHandler.instance.CurrentPlayer.playerRb.velocity = new Vector3(0, vel.y, 0);
            if (PlayerHandler.instance.CurrentPlayer != null)
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            //var vel = PlayerHandler.instance.CurrentPlayer.playerRb.velocity;
            //PlayerHandler.instance.CurrentPlayer.playerRb.velocity = new Vector3(0, vel.y, 0);
            if (PlayerHandler.instance.CurrentPlayer != null)
                PlayerHandler.instance.CurrentPlayer.SetWallcheck(true);
        }
    }
}
