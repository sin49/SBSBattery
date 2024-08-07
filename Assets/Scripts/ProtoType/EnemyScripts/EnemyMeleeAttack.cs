using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public Enemy enemy;
    float damage;


    private void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        damage = enemy.eStat.atk;
    }

    private void OnEnable()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void AttackReady(Enemy enemy, float timer)
    {
        StartCoroutine(MeleeAttack(enemy, timer));
    }

    IEnumerator MeleeAttack(Enemy enemy, float timer)
    {
        yield return new WaitForSeconds(timer);
        this.gameObject.SetActive(false);
        Debug.Log("초기화하자");
        enemy.InitAttackCoolTime();
    }

    /*private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !enemy.activeTv && !onAttack)
        {
            onAttack = true;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerHandler.instance.CurrentPlayer.onInvincible)
        {
            other.GetComponent<Player>().Damaged(damage);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;            
        }
    }
}
