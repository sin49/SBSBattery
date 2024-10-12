using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Hand2DColliderTrigger : MonoBehaviour
{
  public  Boss1Hand hand;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (
                (int)PlayerStat.instance.MoveState < 4  && collision.CompareTag("PlayerAttack"))
        {
  
               
                hand.Damaged(1);
 
     
        }
        if ((int)PlayerStat.instance.MoveState < 4 && collision.CompareTag("Player"))
        {
            hand.handattack();
        }
    }
}
