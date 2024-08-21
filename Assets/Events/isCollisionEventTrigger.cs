using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isCollisionEventTrigger : MonoBehaviour
{
    public event Action<Collision> TriggerEvent;
    public event Action<Collision> TriggerExitEvent;

    public void registerEvent(Action<Collision> enter, Action<Collision> exit)
    {
        TriggerEvent += enter;
        TriggerExitEvent += exit;
    }

    private void OnDestroy()
    {
        TriggerEvent = null;
    }
    private void OnCollisionEnter(Collision other)
    {
        TriggerEvent(other);
    }
    private void OnCollisionExit(Collision other)
    {
        TriggerExitEvent(other);
    }
   
}
