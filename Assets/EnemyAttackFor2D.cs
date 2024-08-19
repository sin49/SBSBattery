using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAttackFor2D : MonoBehaviour
{
  protected virtual  void Attack()
    {
     
        PlayerHandler.instance.CurrentPlayer.Damaged(1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((
                PlayerStat.instance.MoveState != PlayerMoveState.Trans3D ||
                PlayerStat.instance.MoveState != PlayerMoveState.Trans3D2) && collision.CompareTag("Player"))
        {
    
            Attack();
        }
    }
   
}
