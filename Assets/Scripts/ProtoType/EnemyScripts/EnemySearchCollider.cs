using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchCollider : MonoBehaviour
{
    Enemy enemy;
    BoxCollider searchCollider;
    
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        searchCollider = GetComponent<BoxCollider>();        
    }

    private void FixedUpdate()
    {
        if (enemy.searchCollider != null && searchCollider != null)
        {
            searchCollider.center = enemy.searchColliderPos;
            searchCollider.size = enemy.searchColliderRange;
        }
    }

    private void OnDrawGizmos()
    {
        if (CharColliderColor.instance != null)
        {
            Gizmos.color = CharColliderColor.instance.searchRange;
        }

        Collider collider = GetComponent<Collider>();
        if (collider is BoxCollider box)
        {
            Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.target = other.transform;
            if (!enemy.wallCheck)
            {
                enemy.searchPlayer = true;
                enemy.onPatrol = false;
            }
            else
            {
                enemy.searchPlayer = false;
                enemy.onPatrol = true;
            }            
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
     
        if (other.CompareTag("Player"))
        {
            if (enemy.onPatrol)
            {
                enemy.searchPlayer = false;
            }
        }
    }*/
}
