using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingObject : MonoBehaviour
{
    public Enemy enemy;
    public float damage;
    public bool enemyDie;

    private void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        damage = enemy.eStat.atk;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerHandler.instance.CurrentPlayer.onInvincible
            && !enemyDie)
        {
            other.GetComponent<Player>().Damaged(damage);

            enemy.activeAttack = true;
            enemy.DelayTime();
        }
    }
}
