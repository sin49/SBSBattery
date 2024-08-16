using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutter : signalReceiver
{
    Animator ani;

 
    
    protected virtual void Awake()
    {
        ani = GetComponent<Animator>();
        register();
    }
    private void Update()
    {
        ani.SetBool("Open",active );
    }

}
