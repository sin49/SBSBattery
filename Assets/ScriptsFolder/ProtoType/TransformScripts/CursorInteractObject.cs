using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInteractObject : MonoBehaviour
{
    public bool caught, thrown;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("InteractiveObject") 
            || collision.gameObject.CompareTag("Ground"))
        {
            Enemy enemy;
            if (TryGetComponent<Enemy>(out enemy))
            {

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy;
        if (TryGetComponent<Enemy>(out enemy))
        {
            if (other.CompareTag("Enemy"))
            {
                DamagedByPAttack script;
                if (TryGetComponent<DamagedByPAttack>(out script))
                {
                    script.Damaged(1);
                }
            }
            enemy.Dead();
        }
    }
}
