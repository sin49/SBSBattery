using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface timerComponent
{
    public void StartTimer();
    public void StopTimer();
}
[Serializable]
public class TimerEndInputEvent :InputEvent, timerComponent
{
    public
    bool timerstart;
    public bool StartTimerAwake;
    public float timer;
    [HideInInspector    ]
  public  float timer_;
    public override bool input(object o = null)
    {
        if (timer_ > 0)
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

    private void Start()
    {
        initialize();
    }

    private void FixedUpdate()
    {
        if (timer > 0 && timerstart)
        {
            timer_ -= Time.fixedDeltaTime;
            if(timer_<=0)
            {
                timerstart = false;
     
            }    
        }
    }



    public override void initialize()
    {
        timer_ = timer;
        if (StartTimerAwake)
            StartTimer();
    }
}
