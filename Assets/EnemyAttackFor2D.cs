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
        if (
                (int)PlayerStat.instance.MoveState >= 4 && collision.CompareTag("Player"))
        {
    
            Attack();
        }
    }
   
}
