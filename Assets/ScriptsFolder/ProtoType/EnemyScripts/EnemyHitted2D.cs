using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitted2D : MonoBehaviour
{
    Enemy enemy;
    private void Awake()
    {
        enemy=transform.parent.GetComponent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack")&&(int)PlayerStat.instance.MoveState<4)
        {
            if(enemy!=null)
            enemy.Damaged(1);
            else

        }
    }
}
