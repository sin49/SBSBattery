using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rushenemy : Enemy
{
    [Header("���� ����:���� ����")]
    [Header("���� ���� ������")]
    public float rushinitdelay;
    [Header("���� �ð�")]
    public float rushtime;
    [Header("���� �ӵ�")]
    public float rushspeed;
    [Header("���� �ĵ�����")]
    public float rushcooltime;
    [Header("�÷��̾� �з����� ����")]
    public float PlayerForce = 3;
    [Header("�÷��̾� �з��� �� ��� ���� ���ϰ� �ؾ� �� �� �ð���")]
    public float Playerstuntime = 0.25f;
    bool onrush;
    IEnumerator rush()
    {
        Debug.Log("����!");
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
