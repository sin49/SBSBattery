using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Action_rush : NormalEnemyAction
{
    [Header("돌진 사전 딜레이")]
    public float rushinitdelay;
    [Header("돌진 시간")]
    public float rushtime;
    [Header("돌진 속도")]
    public float rushspeed;
    [Header("돌진 후딜레이")]
    public float rushcooltime;
    [Header("플레이어 밀려나는 정도")]
    public float PlayerForce = 3;
    [Header("플레이어 밀려날 때 잠깐 조작 못하게 해야 함 그 시간임")]
    public float Playerstuntime = 0.25f;
    bool onrush;
    public override void register(Enemy e)
    {
        base.register(e);

    }
    
    public override void Invoke(Transform target = null)
    {
        if(!onrush&&e!=null)
            StartCoroutine(rush(target));
    }
    IEnumerator rush(Transform target)
    {
        Debug.Log("돌진!");
        onrush = true;
        this.transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));


        yield return new WaitForSeconds(rushinitdelay);
        //PlayAttackSound();
        float timer = 0;
        while (timer < rushtime)
        {
            e. attackCollider.SetActive(true);
           e.  rb.MovePosition(transform.position + transform.forward * Time.deltaTime * rushspeed);
            timer += Time.deltaTime;
            yield return null;
        }
     e.   attackCollider.SetActive(false);
    e.    rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(rushcooltime);
        onrush = false;
     e.   activeAttack = false;
     e.   InitAttackCoolTime();
    }
    public void stoprush()
    {
        StopAllCoroutines();
        //InitAttackCoolTime();
        StartCoroutine(stoprushcorutine());
    }
    public override void StopAction()
    {
        base.StopAction();
        stoprush();
    }
    IEnumerator stoprushcorutine()
    {
        onrush = true;
        yield return new WaitForSeconds(rushcooltime);
        onrush = false;
        e.activeAttack = false;

    }

}
