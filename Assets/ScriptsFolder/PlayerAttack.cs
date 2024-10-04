using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damage;
    
    public float attackTime;
    float attackTimer;

    private void Awake()
    {
        attackTimer = attackTime;
    }

    protected virtual void Update()
    {
        if (attackTimer > 0 && gameObject.activeSelf)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
            attackTimer = attackTime;
        }

    }

    public virtual void DamageCollider(Collider other)
    {
        DamagedByPAttack Script;
        if (other.TryGetComponent<DamagedByPAttack>(out Script))
        {
            Script.Damaged(damage);
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
