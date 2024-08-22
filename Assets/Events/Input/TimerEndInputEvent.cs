using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface timerComponent
{
    public void StartTimer();
    public void StopTimer();
}
public class TimerEndInputEvent : MonoBehaviour,InputEvent,timerComponent
{
    bool timerstart;

    public float timer;
    public bool input(object o = null)
    {
        if (timer > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void StartTimer()
    {
        timerstart = true;
    }

    public void StopTimer()
    {
        timerstart = false;
    }

  

    private void FixedUpdate()
    {
        if (timer > 0 && timerstart)
        {
            timer -= Time.fixedDeltaTime;
        }
    }
}
