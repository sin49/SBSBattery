using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_Explosion : NormalEnemyAction
{
    GameObject explosionobj;

    public override void Invoke(Transform target = null)
    {
        base.Invoke(target);
      var obj=  Instantiate(explosionobj, e.transform.position, Quaternion.identity);
        obj.SetActive(true);
        e.transform.parent.gameObject.SetActive(false);

    }
   
}
