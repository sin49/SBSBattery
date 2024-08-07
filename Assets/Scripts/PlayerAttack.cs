using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            DamagedByPAttack Script;
            if (other.TryGetComponent<DamagedByPAttack>(out Script))
            {
                Script.Damaged(damage);
                gameObject.SetActive(false);
            }

        }
    }
}
