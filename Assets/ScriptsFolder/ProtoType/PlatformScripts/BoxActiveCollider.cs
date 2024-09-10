using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxActiveCollider : MonoBehaviour
{
    EnemyInstantiateObject enemyBox;

    bool checkPlayer;

    private void Awake()
    {
        enemyBox = GetComponentInParent<EnemyInstantiateObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !checkPlayer)
        {
            checkPlayer = true;
            if (enemyBox.boxAnim != null)
            {
                enemyBox.boxAnim.SetTrigger("Open");
            }
        }
    }
}
