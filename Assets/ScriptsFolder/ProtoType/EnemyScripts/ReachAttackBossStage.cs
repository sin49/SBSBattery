using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachAttackBossStage : MonoBehaviour
{
    BossStageEnemy bse;
    public float damage;
    private void Awake()
    {
        bse = GetComponentInParent<BossStageEnemy>();
        damage = bse.damage;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (!PlayerHandler.instance.CurrentPlayer.onInvincible)
            {
                player.Damaged(damage);
            }
        }

    }
}
