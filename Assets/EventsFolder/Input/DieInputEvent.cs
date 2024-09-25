using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInputEvent : InputEvent
{
    public Character character;
    public bool cDie;
    public override void initialize()
    {
        cDie = false;
    }
    private void Awake()
    {
        initialize();
        if (character != null)
            character.registerdeadevent(deadevent);
    }
    public override bool input(object o)
    {
        return cDie;
    }
    void deadevent()
    {
        cDie = true;
    }
      
}
