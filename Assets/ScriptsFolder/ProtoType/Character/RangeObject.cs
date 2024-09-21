using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeObject : MonoBehaviour
{
    public float damage;
    public float rangeSpeed;

    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    private void Update()
    {
        transform.Translate(transform.forward * rangeSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Damaged(damage);
            Destroy(gameObject);
        }
    }
}
