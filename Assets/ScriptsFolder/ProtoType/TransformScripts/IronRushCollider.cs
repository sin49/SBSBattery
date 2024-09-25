using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronRushCollider : PlayerAttack
{
    HouseholdIronTransform householdIron;
    public GameObject hitEffect;
    ParticleSystem saveEffect;
    private void Start()
    {
        householdIron = GetComponentInParent<HouseholdIronTransform>();
        //saveEffect = Instantiate(hitEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        damage = householdIron.rushDamage;
        gameObject.SetActive(false);
    }

    public override void DamageCollider(Collider other)
    {
        base.DamageCollider(other);
        Vector3 effectPos = new(other.transform.position.x, other.transform.position.y + .5f, other.transform.position.z);
        Instantiate(hitEffect, effectPos, Quaternion.identity);
        /*saveEffect.transform.position = new(other.transform.position.x, other.transform.position.y + .5f, other.transform.position.z);
        saveEffect.Play();*/
    }
}
