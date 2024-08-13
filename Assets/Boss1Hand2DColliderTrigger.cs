using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Hand2DColliderTrigger : MonoBehaviour
{
  public  Boss1Hand hand;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlayerStat.instance.Trans3D && collision.CompareTag("PlayerAttack"))
        {
            hand.Damaged(1);
            collision.gameObject.SetActive(false);
        }
    }
}
