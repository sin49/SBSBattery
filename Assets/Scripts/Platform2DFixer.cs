using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform2DFixer : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (PlayerStat.instance.MoveState != PlayerMoveState.Trans3D)
            {
         
                Transform player = collision.transform.parent;
                player.position = new Vector3(player.position.x, player.position.y, this.transform.position.z);
            }
        }
    }
}
