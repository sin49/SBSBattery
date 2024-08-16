using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Shutter : signalReceiver
{
    Animator ani;

 
    
    protected override void Awake()
    {
        base.Awake();
        ani = GetComponent<Animator>();
        register();
    }
    private void Update()
    {
        ani.SetBool("Open",active );
    }

}
