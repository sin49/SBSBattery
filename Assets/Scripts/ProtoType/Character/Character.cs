using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character: MonoBehaviour
{
    protected CharacterSoundPlayer soundplayer;
    event Action hittedevent;
    event Action attackevent;
    event Action moveevent;
    event Action deadevent;
    public void registerhittedevent(Action a)
    {
        hittedevent += a;
    }
   public void registerdeadevent(Action a)
    {
        deadevent += a;
    }
    public abstract void Attack();
    public virtual void Damaged(float damage)
    {
        hittedevent?.Invoke();
    }
    public abstract void Move();
    public abstract void Dead();

    protected Rigidbody rb;
    protected virtual void Awake()
    {
        rb= GetComponent<Rigidbody>();

            soundplayer = GetComponent<CharacterSoundPlayer>();
    }
}
