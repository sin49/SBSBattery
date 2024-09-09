using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachAttack : MonoBehaviour
{
    public Enemy enemy;
    public float damage;

    private void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        damage = enemy.eStat.atk;
    }

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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerHandler.instance.CurrentPlayer.onInvincible)
        {
            other.GetComponent<Player>().Damaged(damage);
            enemy.reachCheck = true;
        }
    }
}
