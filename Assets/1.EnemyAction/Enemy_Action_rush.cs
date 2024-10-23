using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Action_rush : NormalEnemyAction
{

    [Header("돌진 시간")]
    public float rushtime;
    [Header("돌진 속도")]
    public float rushspeed;

    [Header("플레이어 밀려나는 정도")]
    public float PlayerForce = 3;
   
    bool onrush;

    
  
    public override void register(Enemy e)
    {
        base.register(e);
        rushattackcollider rushattack =    e.attackCollider.GetComponent<rushattackcollider>();
        rushattack.playerforce = PlayerForce;
        rushattack.registerrushendevent(stoprush);
    }
    
    public override void Invoke(Transform target = null)
    {
        base.Invoke();
        if(!onrush&&e!=null)
            StartCoroutine(rush(target));
    }
    IEnumerator rush(Transform target)
    {
        Vector3 targetpos= target.position;
        Debug.Log("돌진!");
        onrush = true;
        e.transform.LookAt(new Vector3(targetpos.x, targetpos.y, targetpos.z));

  
   
        //PlayAttackSound();
        float timer = 0;
 
        while (timer < rushtime)
        {
            
            e. attackCollider.SetActive(true);
           e.  rb.MovePosition(e.transform.position + e.transform.forward * Time.deltaTime * rushspeed);
            timer += Time.deltaTime;
            yield return null;
        }
     e.   attackCollider.SetActive(false);
    e.    rb.velocity = Vector3.zero;

        onrush = false;
     e.   activeAttack = false;
     e.   InitAttackCoolTime();
    }
    public void stoprush()
    {
        StopAllCoroutines();
        //InitAttackCoolTime();
        onrush = false;
        e.activeAttack = false;
    }
    public override void StopAction()
    {
        base.StopAction();
        stoprush();
    }
  

}
