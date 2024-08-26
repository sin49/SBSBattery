using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyRemoveOutputEvent : OutputEvent
{
    public GameObject selectEnemy;
    public override void output()
    {
        throw new System.NotImplementedException();
    }

    public void EnemySelectAndDelete()
    {
        Destroy(selectEnemy);
        selectEnemy = null;
    }
}
