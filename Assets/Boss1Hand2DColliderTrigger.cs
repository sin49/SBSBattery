using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Hand2DColliderTrigger : MonoBehaviour
{
  public  Boss1Hand hand;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((
                PlayerStat.instance.MoveState != PlayerMoveState.Trans3D ||
                PlayerStat.instance.MoveState != PlayerMoveState.Trans3D2) && collision.CompareTag("PlayerAttack"))
        {
            if (hand.AttackState)
            {
               
                hand.Damaged(1);
            }
     
        }
    }
}
