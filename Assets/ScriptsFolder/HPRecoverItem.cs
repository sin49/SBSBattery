using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPRecoverItem : MonoBehaviour
{
    public float HPRecoverPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerStat.instance.hp < PlayerStat.instance.hpMax)
            {
                PlayerStat.instance.RecoverHP(HPRecoverPoint);
                Destroy(gameObject);
            }
        }
    }
}
