using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : ShootingEnemy
{

    public shootingShieldrotate SSrot;
    public shootingShield SS;
    public ShootingSpriteRotate SSrtSpriteRot;

    protected override void FixedUpdate()
    {
        SSrot.Target = TargetVector+(Vector2)transform.position;
        SS.Target = TargetVector + (Vector2)transform.position;
        SSrtSpriteRot.target = TargetVector + (Vector2)transform.position;
        base.FixedUpdate();
    }
   
}
