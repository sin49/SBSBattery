using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOss1HandCollider : MonoBehaviour,DamagedByPAttack
{
    Boss1Hand hand;

    public void Damaged(float f)
    {
        Debug.Log("asdf");
        if ((int)PlayerStat.instance.MoveState >= 4 )
        {
    
            hand.Damaged(1);
        }
    }

    private void Awake()
    {
        hand=transform.parent.GetComponent<Boss1Hand>();
    }
   
}
