using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class signalSender:MonoBehaviour
{
    public bool active;
    public abstract void register(signalReceiver receiver,int index);
    public abstract void Send(bool signal);
}
public abstract class signalReceiver:MonoBehaviour
{
    public List<signalSender> signalSenders;
    //bool[] signals;
   public bool active;
    public void CheckSignal()
    {
        bool chk = true;
        foreach(signalSender a in signalSenders)
        {
            chk &= a.active;
        }
        //foreach (signalSender a in signalSenders)
        //{
        //    chk &= a.active;
        //}
       active = chk;
    }
    public void register()
    {
       for(int i = 0; i < signalSenders.Count; i++)
        {
            if (signalSenders[i]!=null)
            signalSenders[i].register(this,i);
        }
        //signals = new bool[signalSenders.Count];
    }
    public void Receive(bool signal,int signalnumber)
    {
        CheckSignal();
    }
}