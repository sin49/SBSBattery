using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SpecialMeleeCollider : MonoBehaviour
{
    public State characterState;
    public float damage;

    private void OnEnable()
    {
        StartCoroutine(ActiveSpecialAttack());
    }

    IEnumerator ActiveSpecialAttack()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //other.GetComponent<Enemy>().eStat.characterState = characterState;
            other.GetComponent<Enemy>().Damaged(damage, gameObject);
        }
    }*/
}
