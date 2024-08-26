using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer_Start_Event : OutputEvent
{
    public List< timerComponent> timers= new List<timerComponent>();
    public override void output()
    {
        base.output();
        foreach (var timer in timers)
        {
            timer.StartTimer();
        }
        
    }
}
