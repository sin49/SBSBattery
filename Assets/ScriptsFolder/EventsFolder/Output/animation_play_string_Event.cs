using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_play_string_Event : OutputEvent
{
    public string clipname;
    public Animator animator;
    public override void output()
    {
        base.output();
        animator.Play(clipname);
    }
}
