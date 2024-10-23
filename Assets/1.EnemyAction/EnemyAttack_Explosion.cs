using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAttack_Explosion : NormalEnemyAction
{
    GameObject explosionobj;
    public override void register(Enemy e)
    {
        base.register(e);
        explosionobj = e.attackCollider;
    }
    public override void Invoke(Transform target = null)
    {
        base.Invoke(target);
      var obj=  Instantiate(explosionobj, e.transform.position, Quaternion.identity);
        Vector3 objscale = obj.transform.localScale;
        objscale *= 2.5f;
        obj.transform.localScale = objscale;
        obj.SetActive(true);
        e.transform.parent.gameObject.SetActive(false);

    }
   
}
