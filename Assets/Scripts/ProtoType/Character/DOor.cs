using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOor : InteractiveObject
{
    public Animator animator;
    private void Awake()
    {
        InteractOption = InteractOption.ray;
    }
    bool Open;
    public override void Active(direction direct)
    {
        animator.SetInteger("direction", (int)direct);
  
        animator.SetBool("Open", true);
    }
   
    

}

