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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.target = other.transform;
            enemy.searchPlayer = true;
            if (!enemy.wallCheck)
            {
                enemy.onPatrol = false;
                enemy.tracking = true;
                
            }
            else
            {
                enemy.onPatrol = true;
                enemy.searchPlayer = false;
            }
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
     
        if (other.CompareTag("Player"))
        {
            //enemy.tracking = false;
            enemy.onPatrol = true;
        }
    }*/
}
