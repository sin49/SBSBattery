using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpenemy : Enemy
{

    bool onground;
    //public bool PlayernotChase;
    [Header("���� ����(������ ���� ���� estat.movespeed�� ����)")]
    public float jumpforce;
    [Header("���� �� ������")]
    public float jumpdelay=1;
   
    bool oncorutine;
    [Header("�� �κ� �̹̼� ��Ƽ����")]
    public Material emmissionHeadMat;

    protected override void MoveAnimationPlay()
    {
        
    }
    public override void enemymovepattern()
    {
       
        if (onground&&!oncorutine)
        {
            StartCoroutine(jumpmove());
        }
    }
    public override void Attack()
    {
        Debug.Log("���� ��");
        TrackingMove();
    }
    public override void Move()
    {
        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {

            if (tracking)
            {

                if (movepattern == EnemyMovePattern.patrol)
                {
                    if (patrolType == PatrolType.movePatrol && searchCollider.onPatrol)
                    {

                        PatrolTracking();
                    }
                }
                if (searchCollider.searchPlayer)
                    TrackingMove();

            }
        }
    }
    IEnumerator jumpmove()
    {
        //    if (activeAttack)
        //    {
        //        activeAttack = false;
        //        oncorutine = false;
        //        yield return new WaitForSeconds(jumpdelay);
        //        yield break;
        //    }
        oncorutine = true;
        animaor.SetTrigger("jump");
        yield return new WaitForSeconds(0.07f);
        //if (activeAttack)
        //{
        //    activeAttack = false;
        //    oncorutine = false;
        //    yield return new WaitForSeconds(jumpdelay);
        //    yield break;
        //}
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
    //0��°: �ٵ�, 1��°: ���� ����
    public override void StartEmmissionHitMat()
    {
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = mae.hittedMat;
        materials[1] = emmissionHeadMat;

        mae.skinRenderer.materials = materials;
    }

    public override void EndEmmissionHitMat()
    {
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = mae.idleMat;
        materials[1] = mae.headMat;

        mae.skinRenderer.materials = materials;
    }

    public override void EndHitMat()
    {
        return;
    }
}
