using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxActiveCollider : MonoBehaviour
{
 public   EnemyInstantiateObject enemyBox;

    bool checkPlayer;

  
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
