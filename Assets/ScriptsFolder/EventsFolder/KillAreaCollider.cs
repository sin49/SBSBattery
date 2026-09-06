using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAreaCollider : MonoBehaviour
{
    public List<GameObject> enemyGroup = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyGroup.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            foreach(GameObject enemy in enemyGroup)
            {
                if (enemy == other.gameObject)
                {
                    enemyGroup.Remove(enemy);
                    return;
                }
            }
        }
    }

    public void EnemyAllDie()
    {
        if (enemyGroup.Count > 0)
        {
            foreach (GameObject enemy in enemyGroup)
            {
                enemy.GetComponent<Enemy>().Dead();
            }
        }
    }
}
