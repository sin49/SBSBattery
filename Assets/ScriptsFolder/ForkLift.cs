using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkLift : signalReceiver
{
    //���� ��ũ��Ʈ�� �ߺ��̴� ��ĥ ��
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
