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
    float invinclibletimer_;
    public CinemachineImpulseSource shaker;
    public Boss1HandSoundPlayer soundplayer;
    public event Action HandDominateEvent;
    public bool invincible;
    public float invincibletimer = 0.12f;
    
    public GameObject HittedEffect;
    public void shakeonce()
    {
        shaker.GenerateImpulse();
    }
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

    private void FixedUpdate()
    {
        if (invinclibletimer_ > 0)
        {
            invinclibletimer_ -= Time.deltaTime;
            if (invinclibletimer_ <= 0)
            {
                invincible = false;
            }
        }
    }
    public void Damaged(float f)
    {
        if (invincible)
            return;
        if (HP > 0)
        {
            HP-=f;
            if(HittedEffect!=null)
            Instantiate(HittedEffect, this.transform.position, Quaternion.identity);
            invincible = true;
            invinclibletimer_ = invincibletimer;
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
    public void handattack()
    {
        if (AttackState)
        {
            Debug.Log("¼Õ°ø°Ý");
           PlayerHandler.instance.CurrentPlayer.Damaged(1);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            handattack();
        }
    }
}
