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
        enemymovepattern();
    }
    public override void Move()
    {
        //if (movepattern == EnemyMovePattern.patrol)
        //{���⸦ �ý���ȭ�� ���� ���۾����� ���α�


        if (!activeAttack && tap.tracking)
        {
            Vector3 target = tap.GetTarget();
            transform.rotation = Quaternion.LookRotation(target);
            enemymovepattern();

        }
        //ismove = move ���ϸ��̼� ���� ����->move������ �ű��
        if (movepattern == EnemyMovePattern.stop)
        {
            if (tap.tracking && !activeAttack && tap.PlayerDetected)
            {
                isMove = true;
            }
            else
            {
                isMove = false;
            }
        }
        else
        {
            if (tap.tracking && !activeAttack)
            {
                isMove = true;
            }
            else
            {
                isMove = false;
            }
        }
        MoveAnimationPlay();
        //}     
        //if (!die || !hitted)
        //{

        //    if (tracking)
        //    {

        //        if (movepattern == EnemyMovePattern.patrol)
        //        {
        //            if (patrolType == PatrolType.movePatrol && mae.searchCollider.onPatrol)
        //            {

        //                PatrolTracking();
        //            }
        //        }
        //        if (mae.searchCollider.searchPlayer)
        //            TrackingMove();

        //    }
        //}
    }
    IEnumerator jumpmove()
    {
        //if (activeAttack)
        //{
        //    activeAttack = false;
        //    oncorutine = false;
        //    yield return new WaitForSeconds(jumpdelay);
        //    yield break;
        //}
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
        materials[0] = mae.emmissionBackMat; // ����
        materials[1] = mae.emmissionHeadMat; // �� ����������

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
