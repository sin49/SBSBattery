using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss1Hand : MonoBehaviour,DamagedByPAttack
{
    public float HP;
    public BossTv bosstv;
   public bool active;

    public event Action HandDominateEvent;
    public void Damaged(float f)
    {
        if (HP > 0)
        {
            HP-=f;
            if (HP == 0)
            {
                HandDominateEvent?.Invoke();
                active = false;
                HandDominateEvent = null;
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Damaged(1);
        }
    }
}
