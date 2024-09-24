using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageRangeCollider : MonoBehaviour
{
    public BossStageEnemy bse;    

    private void Awake()
    {
       bse = GetComponentInParent<BossStageEnemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && bse.completeSpawn)
        {
            bse.onAttack = true;
            bse.attackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && bse.completeSpawn)
        {
            bse.attackRange = false;
        }
    }
}
