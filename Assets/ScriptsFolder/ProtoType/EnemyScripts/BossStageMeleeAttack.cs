using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageMeleeAttack : MonoBehaviour
{
    public BossStageEnemy bse;
    public float damage;

    private void Awake()
    {
        bse= GetComponentInParent<BossStageEnemy>();
        damage = bse.damage;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {        
        GetComponent<SphereCollider>().enabled = true;
    }

    public void MeleeAttack()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(WaitAndFalse());
    }

    IEnumerator WaitAndFalse()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);        
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerHandler.instance.CurrentPlayer.onInvincible)
        {
            other.GetComponent<Player>().Damaged(damage);
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
