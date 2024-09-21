using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpenemy : Enemy
{
    bool onground;
    //public bool PlayernotChase;
    public float jumpforce;
    public float jumpdelay=1;
    bool oncorutine;
    public override void enemymovepattern()
    {
       
        if (onground&&!oncorutine)
        {
            StartCoroutine(jumpmove());
        }
    }
    public override void Attack()
    {
        Debug.Log("АјАн Сп");
        TrackingMove();
    }
    IEnumerator jumpmove()
    {
        if (activeAttack)
            yield break;
        oncorutine = true;
        animaor.SetTrigger("jump");
        yield return new WaitForSeconds(0.07f);
        if (activeAttack)
        {
            oncorutine = false;
            yield return new WaitForSeconds(jumpdelay);
            yield break;
        }
        rb.AddForce(Vector3.up * jumpforce + eStat.moveSpeed * transform.forward, ForceMode.Impulse);
        yield return new WaitForSeconds(1.08f);

        yield return new WaitForSeconds(jumpdelay);
        oncorutine = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
     
                onground = false;
       
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (rb.velocity.y <= 0 && !onground)
            {
                onground = true;
            }
        }
    }
}
