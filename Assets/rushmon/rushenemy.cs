using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rushenemy : Enemy
{
    [Header("공격 사운드:돌진 사운드")]
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
    IEnumerator rush()
    {
        Debug.Log("돌진!");
        onrush = true;
        //this.transform.LookAt(new Vector3( target.position.x,this.transform.position.y,target.position.z));

       
        yield return new WaitForSeconds(rushinitdelay);
        PlayAttackSound();
        float timer = 0;
        while (timer < rushtime)
        {
            attackCollider.SetActive(true);
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * rushspeed);
            timer += Time.deltaTime;
            yield return null;
        }
        attackCollider.SetActive(false);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(rushcooltime);
        onrush = false;
        activeAttack = false;
        InitAttackCoolTime();
    }
   public void stoprush()
    {
        StopAllCoroutines();
        InitAttackCoolTime();
        StartCoroutine(stoprushcorutine());
    }
    IEnumerator stoprushcorutine()
    {
        onrush = true;
        yield return new WaitForSeconds(rushcooltime);
        onrush = false;
        activeAttack = false;
    }
    public override void Attack()
    {
        if (onrush)
            return;
     
        if (animaor != null)
            animaor.Play("EnemyAttack");
        if (actionhandler != null)
            actionhandler.invokemainaction();
        StartCoroutine(rush());
    }

    public override void FlatByIronDwonAttack(float downAtkEndTime)
    {
        base.FlatByIronDwonAttack(downAtkEndTime);
        stoprush();
    }

    public override void StartEmmissionHitMat()
    {
        skinRenderer.material = hittedMat;
    }

    public override void EndEmmissionHitMat()
    {
        skinRenderer.material = idleMat;
    }

    public override void EndHitMat()
    {
        return;
    }
}
