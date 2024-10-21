using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction_Swing : NormalEnemyAction
{
    //public EnemyMeleeAttack meleeattack;
    public float damage;
    IEnumerator MeleeAttack( float timer)
    {

        e.attackCollider.gameObject.SetActive(true);
        yield return new WaitForSeconds(timer);
        e.attackCollider.gameObject.SetActive(false);
        DisableActionMethod();
    }

    public override void Invoke(Action ActionENd, Transform target = null)
    {
        base.Invoke(ActionENd, target);
        StartCoroutine(MeleeAttack(0.25f));
    }
}
