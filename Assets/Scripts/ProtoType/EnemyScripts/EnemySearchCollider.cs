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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.onPatrol = false;
            enemy.target = other.transform;
            enemy.tracking = true;
            enemy.searchPlayer = true;
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
