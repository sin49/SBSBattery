using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlInputEvent : InputEvent
{
    public bool keyInput;
    public override void initialize()
    {
        keyInput = false;
    }

    private void Start()
    {
        PlayerHandler.instance.registerkeyinputevent(KeyInput);
    }

    public override bool input(object o)
    {
        return keyInput;
    }

    public void KeyInput()
    {
        keyInput = true;
    }
}
