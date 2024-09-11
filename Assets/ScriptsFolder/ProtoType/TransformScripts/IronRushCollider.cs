using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronRushCollider : MonoBehaviour
{
    HouseholdIronTransform householdIron;
    public float damage;

    private void Start()
    {
        householdIron = GetComponentInParent<HouseholdIronTransform>();
        damage = householdIron.rushDamage;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && householdIron.canRushAttack)
        {
            other.GetComponent<Enemy>().Damaged(damage);
        }
    }
}
