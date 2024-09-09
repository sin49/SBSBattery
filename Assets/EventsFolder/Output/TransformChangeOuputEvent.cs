using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChangeOuputEvent : OutputEvent
{
    public TransformType formType;
    public int index;

    public override void output()
    {
        OutputTransform();
        base.output();
    }

    public void OutputTransform()
    {
        formType = (TransformType)index;
        Player player = PlayerHandler.instance.CurrentPlayer;
        Destroy(Instantiate(player.changeEffect, transform.position, Quaternion.identity), 2f);
        PlayerHandler.instance.transformed(formType);
    }
}
