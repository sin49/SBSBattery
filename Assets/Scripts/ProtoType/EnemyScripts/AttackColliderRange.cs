using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AttackColliderRange : MonoBehaviour
{
    public Enemy enemy;
    BoxCollider rangeCollider;
    private void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        rangeCollider = GetComponent<BoxCollider>();
        if (enemy.rangeCollider != null)
        {
            enemy.rangePos = rangeCollider.center;
            enemy.rangeSize = rangeCollider.size;
        }
    }

    private void Update()
    {
        if (rangeCollider != null)
        {
            rangeCollider.center = enemy.rangePos;
            rangeCollider.size = enemy.rangeSize;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //ebug.Log($"Ʈ���� ���� �� {other.gameObject}");   
        if (other.CompareTag("Player") &&enemy.target!=null && !enemy.onStun && !enemy.onAttack)
        {
            if (!enemy.wallCheck)
            {
                if (enemy.transform.position.x < enemy.target.position.x)
                {
                    enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    enemy.transform.rotation = Quaternion.Euler(0, -90, 0);
                }

                enemy.onAttack = true;
            }

            enemy.attackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enemy.target != null && !enemy.onStun)
        {
            enemy.attackRange = false;
        }
    }
}
