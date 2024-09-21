using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingManager : MonoBehaviour
{
    public bool Onhit;
    public float hittime;
    float hittimer;
    public static RacingManager instance;


 public void hit()
    {
        Onhit = true;

    }
    private void FixedUpdate()
    {
        if (Onhit)
        {
            hittimer += Time.deltaTime;
            if(hittimer> hittime)
            {
                hittimer = 0;
                Onhit = false;
            }
        }
    }
    private void Awake()
    {
        instance = this;

    }
}
