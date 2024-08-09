using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //ebug.Log($"트리거 감지 중 {other.gameObject}");   
        if (other.CompareTag("Player") &&enemy.target!=null/*&& !enemy.activeTv*/ && !enemy.onAttack && !enemy.onStun)
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
    }
}
