using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction_jumpMove : NormalEnemyAction
{
    //점프값 받기
    bool oncorutine;
    public float jumpforce;
    public float jumpdelay;
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

        e.rb.AddForce(Vector3.up * jumpforce +e. eStat.moveSpeed * e.transform.forward, ForceMode.Impulse);
        yield return new WaitForSeconds(1.08f);

        yield return new WaitForSeconds(jumpdelay);
        oncorutine = false;
    }
}
