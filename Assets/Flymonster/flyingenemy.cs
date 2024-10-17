using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingenemy : Enemy
{
    public override void enemymovepattern()
    {
        transform.LookAt( testTarget+transform.position);
        transform.Translate(Vector3.forward
            * eStat.moveSpeed * Time.deltaTime);
    }
    public override void PatrolChange()
    {
    
    }
    public override bool SetRotation()
    {
        return true;
    }
    public override void PatrolTracking()
    {
        
        testTarget = tap.targetPatrol - transform.position;
        enemymovepattern();
        if (soundplayer != null)
            soundplayer.PlayMoveSound();
        Debug.Log("TTMAg" + testTarget.magnitude);
        if (testTarget.magnitude < tap.patrolDistance)
        {
            tracking = false;
            StartCoroutine(InitPatrolTarget());
        }
    }
    public override void TrackingMove()
    {
        testTarget = target.position - transform.position;
        enemymovepattern();
            if (soundplayer != null)
                soundplayer.PlayMoveSound();
   

        if (!callCheck)
        {
            if (tap.disToPlayer > tap.trackingDistance /*|| f > 6*/)
            {
                searchCollider.searchPlayer = false;
                target = null;
                searchCollider.onPatrol = true;
            }
        }
    }
    public override void Attack()
    {
        TrackingMove();
    }
    public override void Damaged(float damage)
    {
        rb.isKinematic = false;
        base.Damaged(damage);
        rb.isKinematic = true;
    }
}
