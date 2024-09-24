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
    // Update is called once per frame
    void Update()
    {
        if(animator != null) 
        animator.SetBool("Active", active);
    }
    private void OnCollisionEnter(Collision collision)
    {
      if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
       
            active = true;
            Send(active);
        }
    }
   
   
   

    public override void register(signalReceiver receiver, int index)
    {
       
        Receiver.Add(receiver);
        signalnumber = index;
    }
}
