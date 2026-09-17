using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPlaceCollider2D : MonoBehaviour
{
  public TransformPlace transformPlace;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            if (PlayerHandler.instance.CurrentType == transformPlace.type) return;

            DownAttackCollider2D p;
            if (collision.gameObject.TryGetComponent<DownAttackCollider2D>(out p))
            {
              transformPlace.  transformStart(PlayerHandler.instance.CurrentPlayer.gameObject);
            }
           
        }
    }
}
