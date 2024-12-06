using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTransform : InteractTutorial
{
    public TransformType interactType;
    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && PlayerHandler.instance.CurrentType == interactType)
        {
            base.OnTriggerStay(other);
        }
    }
}
