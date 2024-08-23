using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerPlayingInputEvent : MonoBehaviour, InputEvent, timerComponent
{
    bool timerstart;

    public float timer;
    public bool input(object o = null)
    {
        if (timer > 0&& timerstart)
        {
            return true;
        }
        else
        {
            return false;
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
