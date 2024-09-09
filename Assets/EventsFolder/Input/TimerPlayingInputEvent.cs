using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class TimerPlayingInputEvent :  InputEvent, timerComponent
{
    public
    bool timerstart;
    public bool StartTimerAwake;
    public float timer;
    [HideInInspector]
    public float timer_;
    public override bool input(object o = null)
    {
        if (timer_ > 0&& timerstart)
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
        if (timer_ > 0 && timerstart)
        {
            timer_ -= Time.fixedDeltaTime;
        }
    }

    public override void initialize()
    {
        timer_ = timer;
        if (StartTimerAwake)
            StartTimer();
    }
}
