using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isTriggerEventHandler : MonoBehaviour
{
    public event Action<Collider> TriggerEvent;
    public event Action<Collider> TriggerExitEvent;
    public void registerEvent(Action<Collider> enter, Action<Collider> exit)
    {
        TriggerEvent = null;
        TriggerExitEvent = null;
        TriggerEvent += enter;
        TriggerExitEvent += exit;
    }

    private void OnDestroy()
    {
        TriggerEvent = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        TriggerEvent?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        TriggerExitEvent?.Invoke(other);
    }
}
