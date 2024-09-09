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
        EnemyKill();
        return eKill;
    }

    public void EnemyKill()
    {
        if (eKill)
        {
            foreach (Transform transform in this.obj.transform)
            {
                transform.GetComponent<Enemy>().Dead();
            }
            eKill = false;
        }
    }

    public void AddEnemyDeadEvent(GameObject obj)
    {
        Character c = obj.GetComponent<Character>();
        c.registerdeadevent(EnemyKill);
        arraySize++;
    }
}
