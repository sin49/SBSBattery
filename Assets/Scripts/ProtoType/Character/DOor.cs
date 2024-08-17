using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOor : InteractiveObject
{
    public Animator animator;
  
    bool Open;

    public bool sideZ;
    protected override void Awake()
    {
        base.Awake();
        InteractOnce = true;
    }
    public override void Active(direction direct)
    {
        base.Active(direct);
        int direction=0;
        var a = this.transform.position - PlayerHandler.instance.CurrentPlayer.transform.position;
        if (!sideZ) {
            if (a.x > 0)
                direction = 1;
            else
                direction = -1;
        }
        else
        {
            if (a.z > 0)
                direction = 1;
            else
                direction = -1;
        }
        animator.SetInteger("direction", direction);
  
        animator.SetBool("Open", true);
        //CanInteract = false;
    }
   
    

}

