using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : ShootingEnemy
{
    float AttackTimer;
    public float attacktime;
    public float enemyAttackrange;
    public override void Start()
    {
        base.Start();
        AttackTimer = attacktime;
    }
    public IEnumerator Attack()
    {
        var bullet = Instantiate(Bullet, this.transform.position, transform.rotation);
        bullet.GetComponent<ShootingBullet>().Setbullet(bulletspeed, TargetVector.normalized, bulletlifetime, false);
        onshoot = true;
        yield return new WaitForSeconds(AttackDelay);
        onshoot = false;
    }
    protected virtual void ActiveAttack()
    {
        if (!onshoot)
            AttackTimer -= Time.deltaTime;
        if (AttackTimer <= 0)
        {
            StartCoroutine(Attack());
            AttackTimer = attacktime;
        }
    }
    protected override void EnemyAi()
    {
        base.EnemyAi();
        if(enemyAttackrange >= TargetVector.magnitude)
        {
            ActiveAttack();
        }
    }
}
