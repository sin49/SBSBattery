using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingenemy : Enemy
{
    public override void enemymovepattern()
    {
        transform.Translate(transform.forward
            * eStat.moveSpeed * Time.deltaTime);
    }
    public override void Attack()
    {
        TrackingMove();
    }
}
