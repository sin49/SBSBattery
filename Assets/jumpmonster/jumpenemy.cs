using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpenemy : Enemy
{

    bool onground;
    //public bool PlayernotChase;
    [Header("점프 높이(앞으로 가는 힘은 estat.movespeed로 결정)")]
    public float jumpforce;
    [Header("점프 후 딜레이")]
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
        Debug.Log("공격 중");
        enemymovepattern();
    }
    public override void Move()
    {
        //if (movepattern == EnemyMovePattern.patrol)
        //{여기를 시스템화를 위한 밑작업으로 빼두기


        if (!activeAttack && tap.tracking)
        {
            Vector3 target = tap.GetTarget();
            transform.rotation = Quaternion.LookRotation(target);
            enemymovepattern();

        }
        //ismove = move 에니메이션 관련 변수->move쪽으로 옮기기
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
    //0번째: 바디, 1번째: 투명 유리
    public override void StartEmmissionHitMat()
    {
        Material[] materials = mae.skinRenderer.materials;
        materials[0] = mae.emmissionBackMat; // 몸통
        materials[1] = mae.emmissionHeadMat; // 얼굴 반투명유리

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
