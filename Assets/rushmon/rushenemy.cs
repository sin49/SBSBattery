using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rushenemy : Enemy
{
    [Header("���� ���� ������")]
    public float rushinitdelay;
    [Header("���� ����")]
    public float rushdistance;
    [Header("���� �ӵ�")]
    public float rushspeed;
    [Header("���� �ĵ�����")]
    public float rushcooltime;
    bool onrush;
    IEnumerator rush()
    {
        Debug.Log("����!");
        onrush = true;
        this.transform.LookAt(new Vector3( target.position.x,this.transform.position.y,target.position.z));
    
        Vector3 initposition = transform.position;
        yield return new WaitForSeconds(rushinitdelay);
        while ((transform.position -
            initposition).magnitude < rushdistance)
        {
            attackCollider.SetActive(true);
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * rushspeed);
            yield return null;
        }
        attackCollider.SetActive(false);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(rushcooltime);
        onrush = false;
        InitAttackCoolTime();
    }
    public override void Attack()
    {
        if (onrush)
            return;
        base.Attack();
        StartCoroutine(rush());
    }
}
