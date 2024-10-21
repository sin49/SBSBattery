using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Action_rush : NormalEnemyAction
{
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
        Debug.Log("����!");
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
