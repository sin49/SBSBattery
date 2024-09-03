using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionInputEvent : InputEvent
{
    public bool interact;
    public override void initialize()
    {
        interact = false;
    }

    private void Awake()
    {
        initialize();
    }

    private void Start()
    {
        PlayerHandler.instance.registerinteractevent(Interact);
    }

    public override bool input(object o)
    {
        return interact;
    }

    void Interact()
    {
        interact = true;
    }
}
