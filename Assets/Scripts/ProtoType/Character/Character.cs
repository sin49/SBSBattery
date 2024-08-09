using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character: MonoBehaviour
{
    public abstract void Attack();
    public abstract void Damaged(float damage);
    public abstract void Move();
    public abstract void Dead();

    protected Rigidbody rb;
    protected virtual void Awake()
    {
        rb= GetComponent<Rigidbody>();
            
    }
}
