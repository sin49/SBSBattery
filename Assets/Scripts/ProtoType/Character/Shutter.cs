using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutter : signalReceiver
{
    Animator ani;

 
    
    private void Awake()
    {
        ani = GetComponent<Animator>();
        register();
    }
    private void Update()
    {
        ani.SetBool("Open",active );
    }

}
