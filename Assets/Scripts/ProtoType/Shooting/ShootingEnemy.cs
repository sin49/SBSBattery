using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ShootingEnemy : ShootingObject
{






    public event Action<ShootingEnemy> Destroyevent;
 
    public override void Start()
    {
        base.Start();
    
    }
    private void OnDisable()
    {
        Destroyevent?.Invoke(this);
    }
   
    //void DeactiveOutsideViewport()
    //{
    //    if (this.transform.localPosition.x < ShootingFIeld.instance.MaxSizeX+snappoint &&this.transform.localPosition.x > ShootingFIeld.instance.MinSizeX- snappoint ||
    //        this.transform.localPosition.y < ShootingFIeld.instance.MaxSizeY + snappoint && this.transform.localPosition.y > ShootingFIeld.instance.MinSizeY- snappoint)
    //        onviewport = true;
    //    else
    //        onviewport = false;
       
    //}
    public float enemyMoverange; 
    void EnemyMoveToPlayer()
    {

        transform.Translate(TargetVector.normalized * movespeed * Time.deltaTime);

    }
   
    protected virtual void EnemyAi()
    {
    if (enemyMoverange < TargetVector.magnitude)
    {

            EnemyMoveToPlayer();
        }
      
    }
  
    protected virtual void SetTarget()
    {
        TargetVector =(ShootingPlayer.instance.transform.position - transform.position);
        TargetVector.x *= -1;
    }
    protected virtual void FixedUpdate()
    {


        SetTarget();

     

        EnemyAi();


    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<ShootingPlayer>().hitted();
        }
    }
}
