using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction_Swing : EnemyAction
{
    public EnemyMeleeAttack meleeattack;
    public float damage;
    IEnumerator MeleeAttack(float timer)
    {
        meleeattack.SetDamage(damage);
        meleeattack.gameObject.SetActive(true);
        yield return new WaitForSeconds(timer);
        meleeattack.gameObject.SetActive(false);
    }

    public override void Invoke(Action ActionENd, Transform target = null)
    {
        base.Invoke(ActionENd, target);
        StartCoroutine(MeleeAttack( ActionLifeTIme));
    }
}
