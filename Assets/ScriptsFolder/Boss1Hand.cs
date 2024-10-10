using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class Boss1Hand : MonoBehaviour,DamagedByPAttack
{
    public float HP;
    public BossTv bosstv;
    public bool AttackState;
   public bool active;
    public float shakertimer;
    public CinemachineImpulseSource shaker;
    public Boss1HandSoundPlayer soundplayer;
    public event Action HandDominateEvent;


    public GameObject HittedEffect;
    IEnumerator shakecorutine()
    {
        while (true)
        {
            shaker.GenerateImpulse();
            yield return new WaitForSeconds(shakertimer);
        }
    }
    public void stopShake()
    {
        StopAllCoroutines();
    }
    public void makeshake()
    {
        StartCoroutine(shakecorutine());
    }
    private void Awake()
    {
        soundplayer = GetComponent<Boss1HandSoundPlayer>();
    }
    public void Damaged(float f)
    {
        if (HP > 0)
        {
            HP-=f;
            if(HittedEffect!=null)
            Instantiate(HittedEffect, this.transform.position, Quaternion.identity);
            if (HP == 0)
            {
                stopShake();
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
            if (AttackState)
            {
                Debug.Log("¼Õ°ø°Ý");
                other.GetComponent<Player>().Damaged(1);

            }
        }
    }
}
