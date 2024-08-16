using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss1Hand : MonoBehaviour,DamagedByPAttack
{
    public float HP;
    public BossTv bosstv;
   public bool active;
    public Boss1HandSoundPlayer soundplayer;
    public event Action HandDominateEvent;
    private void Awake()
    {
        soundplayer = GetComponent<Boss1HandSoundPlayer>();
    }
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
                if(soundplayer!=null)
                soundplayer.HandDestoryClipPlay();
                return;
            }
            if (soundplayer != null)
                soundplayer.HandHittedClipPlay();
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
