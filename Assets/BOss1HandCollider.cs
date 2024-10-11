using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOss1HandCollider : MonoBehaviour
{
    Boss1Hand hand;
    private void Awake()
    {
        hand=transform.parent.GetComponent<Boss1Hand>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((int)PlayerStat.instance.MoveState >= 4 && other.CompareTag("PlayerAttack"))
        {
            hand.Damaged(1);
        }
        if ((int)PlayerStat.instance.MoveState >= 4 && other.CompareTag("Player"))
        {
            hand.handattack();
        }
    }
}
