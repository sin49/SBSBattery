using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1HandAttackCollider : MonoBehaviour
{
    public Boss1Hand hand;
    private void OnTriggerEnter(Collider other)
    {
        if ((int)PlayerStat.instance.MoveState >= 4 && other.CompareTag("Player"))
        {
            Debug.Log("fdsa");
            hand.handattack();
        }
    }
}
