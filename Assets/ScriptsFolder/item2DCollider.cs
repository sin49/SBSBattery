using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class item2DCollider : MonoBehaviour
{
    public float HPRecoverPoint=1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerStat.instance.hp < PlayerStat.instance.hpMax&&
                (int)PlayerStat.instance.MoveState < 4)
            {
                PlayerStat.instance.RecoverHP(HPRecoverPoint);
                Destroy(gameObject);
            }
        }
    }
}
