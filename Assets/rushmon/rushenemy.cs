using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rushenemy : Enemy
{
    [Header("돌진 사전 딜레이")]
    public float rushinitdelay;
    [Header("돌진 시간")]
    public float rushtime;
    [Header("돌진 속도")]
    public float rushspeed;
    [Header("돌진 후딜레이")]
    public float rushcooltime;
    bool onrush;
    IEnumerator rush()
    {
        Debug.Log("돌진!");
        onrush = true;
        //this.transform.LookAt(new Vector3( target.position.x,this.transform.position.y,target.position.z));

       
        yield return new WaitForSeconds(rushinitdelay);
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
