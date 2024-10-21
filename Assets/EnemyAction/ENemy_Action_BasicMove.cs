using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENemy_Action_BasicMove : NormalEnemyAction
{
    
    public override void Invoke(Transform target = null)
    {
       if(e.rb!=null)
            e.rb.MovePosition(e.environmentforce + transform.position + transform.forward * Time.deltaTime * e.eStat.moveSpeed);
    }
}
