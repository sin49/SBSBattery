using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHittedField : MonoBehaviour
{
    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !enemy.onFlat)
        {
            PlayerHandler.instance.CurrentPlayer.Damaged(1);
        }
    }
}
