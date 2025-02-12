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
        if (collision.CompareTag("PlayerAttack") && (int)PlayerStat.instance.MoveState < 4)
        {
            if (enemy != null)
            {
                if (collision.gameObject.TryGetComponent<PlayerAttackFor2D>(out PlayerAttackFor2D pa))
                {
                    Debug.Log($"2D레이저 대미지: {collision.gameObject.GetComponent<PlayerAttackFor2D>().damage}");
                    enemy.Damaged(collision.gameObject.GetComponent<PlayerAttackFor2D>().damage);
                }
                else
                {
                    Debug.Log("그냥 대미지");
                    enemy.Damaged(1);
                }
            }


        }
    }
}
