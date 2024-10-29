using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction_jumpMove : NormalEnemyAction
{
    //점프값 받기
    bool oncorutine;
    public float jumpforce;
    public float jumpdelay;

    private void Awake()
    {
        jumpforce = 5;
        jumpdelay = 0.2f;
    }
    public event Action ShadowOff;
    public event Action ShadowOn;
    public override void register(Enemy e)
    {
        base.register(e);
        ShadowOff = e.TurnOffCharacterShadow;
        ShadowOn = e.TurnOnCharacterShadow;
    }
    public override void Invoke(Transform target = null)
    {
        base.Invoke(target);
        if(!oncorutine)
        StartCoroutine(jumpmove());
    }

    IEnumerator jumpmove()
    {
       
        oncorutine = true;
      e.  animaor.SetTrigger("jump");
        yield return new WaitForSeconds(0.07f);
        ShadowOff?.Invoke();
        e.rb.AddForce(Vector3.up * jumpforce +e. eStat.moveSpeed * e.transform.forward, ForceMode.Impulse);
        yield return new WaitForSeconds(1.08f);
        ShadowOn?.Invoke();
        yield return new WaitForSeconds(jumpdelay);
        oncorutine = false;
    }
}
