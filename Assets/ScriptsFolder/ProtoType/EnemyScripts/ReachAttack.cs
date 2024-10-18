using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachAttack : MonoBehaviour
{    
    public float damage;

    public void SetDamage(float value)
    {
        damage = value;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerHandler.instance.CurrentPlayer.onInvincible)
        {
            other.GetComponent<Player>().Damaged(damage);
        }
    }*/
    public bool onStun;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerHandler.instance.CurrentPlayer.onInvincible)
        {
            if (!onStun)
            {
                other.GetComponent<Player>().Damaged(damage);
            }
        }
    }
}
