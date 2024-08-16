using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkLift : signalReceiver
{
    //셔터 스크립트와 중복이니 합칠 것
    Animator ani;



    protected override void Awake()
    {
        ani = GetComponent<Animator>();
        register();
    }
    private void Update()
    {
        ani.SetBool("Active", active);
    }
}
