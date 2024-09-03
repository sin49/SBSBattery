using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractDoor : instantiteminteractiveObject
{
    public Animator animator;
    protected override void Awake()
    {
        base.Awake();
        if(animator==null)
        animator = GetComponent<Animator>();
    }
    public override void Active(direction direct)
    {
        base.Active(direct);

        animator.SetBool("Open", actived);
        //юс╫ц
        animator.SetInteger("direction", 1);
    }
}
