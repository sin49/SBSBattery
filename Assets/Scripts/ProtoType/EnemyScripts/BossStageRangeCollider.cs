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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bse.onAttack = true;
        }
    }
}
