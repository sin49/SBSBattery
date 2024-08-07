using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchCollider : MonoBehaviour
{
    Enemy enemy;
    BoxCollider searchCollider;
    Coroutine ct;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        searchCollider = GetComponent<BoxCollider>();        
    }

    private void FixedUpdate()
    {
        searchCollider.center = enemy.searchColliderPos;
        searchCollider.size = enemy.searchColliderRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ct != null)
            {
                StopCoroutine(ct);
            }
            enemy.onPatrol = false;
            enemy.target = other.transform;
            enemy.tracking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
     
        if (other.CompareTag("Player"))
        {
            if (ct != null)
            {
                StopCoroutine(ct);
            }
            ct = StartCoroutine(WaitAndPatrol());

            //enemy.tracking = false;
            //enemy.onPatrol = true;
        }
    }

    IEnumerator WaitAndPatrol()
    {
        yield return new WaitForSeconds(enemy.trackingTime);
        enemy.target = null;
        enemy.onPatrol = true;
    }
}
