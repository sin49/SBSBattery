using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch :  signalSender
{
   public Animator animator;


    protected override void Awake()
    {
        base.Awake();
    }
    public override void Send(bool signal)
    {
        if (Receiver.Count != 0)
        {
            foreach (var a in Receiver)
            {
                a.Receive(signal, signalnumber);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(animator != null) 
        animator.SetBool("Active", active);
    }
    private void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")||
            collision.gameObject.CompareTag("CursorObject"))
        {
            if (!active)
            {
                active = true;
                sound.PlayAudio(0);
            }
            Send(active);
        }
    }
   
   
   

    public override void register(signalReceiver receiver, int index)
    {
       
        Receiver.Add(receiver);
        signalnumber = index;
    }
}
