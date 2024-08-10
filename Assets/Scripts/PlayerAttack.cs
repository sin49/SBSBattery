using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage;
    public virtual void DamageCollider(Collider other)
    {
        DamagedByPAttack Script;
        if (other.TryGetComponent<DamagedByPAttack>(out Script))
        {
            Script.Damaged(damage);
            gameObject.SetActive(false);
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            DamageCollider(other);

        }
    }
}
