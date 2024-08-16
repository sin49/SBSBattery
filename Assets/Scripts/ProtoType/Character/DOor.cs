using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOor : InteractiveObject
{
    public Animator animator;
  
    bool Open;
    public override void Active(direction direct)
    {
        base.Active(direct);
        animator.SetInteger("direction", (int)direct);
  
        animator.SetBool("Open", true);
    }
   
    

}

