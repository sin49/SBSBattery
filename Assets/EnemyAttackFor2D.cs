using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackFor2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlayerStat.instance.Trans3D&&collision.CompareTag("Player"))
        {
            PlayerHandler.instance.CurrentPlayer.Damaged(1);
        }
    }
   
}
