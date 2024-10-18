using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RollingEnemy : Enemy
{
    public GameObject rollingObecjt;
    public Transform parent;

    public override void Move()
    {
        if (!die)
        {
            if (tap.tracking)
            {
                if (!activeAttack )
                {
                    RollingMove();
                }
            }

            //RollingPatrol();
        }
    }

    public void RollingMove()
    {
        rb.MovePosition(transform.position + transform.forward * Time.deltaTime * eStat.moveSpeed);
        if(soundplayer!= null)
        soundplayer.PlayMoveSound();
    }

    //public void RollingPatrol()
    //{
    //    Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange);

    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        if (colliders[i].CompareTag("Player"))
    //        {
    //            target = colliders[i].transform;
    //            checkPlayer = true;
    //            tracking = true;
    //        }
    //    }
    //}

    public override void Damaged(float damage)
    {
        base.Damaged(damage);
        eStat.hp -= damage;
        if (eStat.hp <= 0)
        {
            eStat.hp = 0;

            Dead();
        }
        else
        {
            StartCoroutine(WaitHittedDelay());
        }        
    }

    public override void Dead()
    {
        rollingObecjt.GetComponent<RollingObject>().enemyDie = true;
        rollingObecjt.transform.SetParent(null);
        base.Dead();
    }

    IEnumerator WaitHittedDelay()
    {
        rb.velocity = Vector3.zero;
        activeAttack = true;
        yield return new WaitForSeconds(.5f);
        activeAttack = false;
    }
}
