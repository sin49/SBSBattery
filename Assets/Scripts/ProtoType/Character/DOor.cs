using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOor : InteractiveObject
{
    public Animator animator;
  
    bool Open;
    protected override void Awake()
    {
        base.Awake();
        InteractOnce = true;
    }
    public override void Active(direction direct)
    {
        base.Active(direct);
        animator.SetInteger("direction", (int)direct);
  
        animator.SetBool("Open", true);
        //CanInteract = false;
    }
   
    

}

