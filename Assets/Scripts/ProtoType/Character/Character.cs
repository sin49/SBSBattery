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
    public void registerattackevent(Action a)
    {
        attackevent += a;
    }
    public void registermoveevent(Action a)
    {
        moveevent += a;
    }
 
    public void registerdeadevent(Action a)
    {
        deadevent += a;
    }
    public virtual void Attack()
    {
        attackevent?.Invoke();
    }
    public virtual void Damaged(float damage)
    {
        hittedevent?.Invoke();
    }
    public virtual void Move()
    {
        moveevent?.Invoke();
    }
    public virtual void Dead()
    {
        deadevent?.Invoke();//사망 이벤트 실행
    }

    protected Rigidbody rb;
    protected virtual void Awake()
    {
        rb= GetComponent<Rigidbody>();

            soundplayer = GetComponent<CharacterSoundPlayer>();
    }
}
