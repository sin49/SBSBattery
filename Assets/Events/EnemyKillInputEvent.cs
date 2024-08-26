using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillInputEvent : InputEvent
{
    public GameObject obj;
    public bool eKill;

    public override void initialize()
    {
        eKill = false;
    }

    public override bool input(object o)
    {
        return eKill;
    }

    public void EnemyKill()
    {
        if (eKill)
        {
            foreach (GameObject obj in this.obj.transform)
            {
                obj.GetComponent<Enemy>().Dead();
            }
        }
    }
}
