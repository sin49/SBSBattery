using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackFor2D : MonoBehaviour
{
    public float damage;

    public virtual void DamageCollider(Collider2D other)
    {
        DamagedByPAttack Script;
        if (other.TryGetComponent<DamagedByPAttack>(out Script))
        {
            Script.Damaged(damage);
           
        }
        this.transform.parent.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")&& 
                (int)PlayerStat.instance.MoveState < 4)
        {
            Debug.Log("플레이어 2D 공격");
            DamageCollider(collision);

        }
    }
   
}
