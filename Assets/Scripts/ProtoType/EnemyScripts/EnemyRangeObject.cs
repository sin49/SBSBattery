using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeObject : MonoBehaviour
{
    public float rangeSpeed;
    public float damage;

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
        if (other.CompareTag("Player")
            && !PlayerHandler.instance.CurrentPlayer.onInvincible)
        {
            PlayerHandler.instance.CurrentPlayer.Damaged(damage);
            PoolingManager.instance.ReturnPoolObject(this.gameObject);
        }
        else if (other.CompareTag("Ground") || other.CompareTag("InteractiveObject")
            || other.CompareTag("InteractivePlatform") || other.CompareTag("GameController"))
        {
            PoolingManager.instance.ReturnPoolObject(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("InteractiveObject")
            || collision.gameObject.CompareTag("InteractivePlatform") || collision.gameObject.CompareTag("GameController"))
        {
            PoolingManager.instance.ReturnPoolObject(this.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        PoolingManager.instance.ReturnPoolObject(this.gameObject);
    }
}
