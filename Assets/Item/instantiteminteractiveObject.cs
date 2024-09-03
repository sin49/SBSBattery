using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiteminteractiveObject : InteractiveObject
{

    public instantitem Instantitem;
  protected  bool actived;

    public override void Active(direction direct)
    {
        if (PlayerInventory.instance.checkinstantitem(Instantitem))
        {
            actived = true;
            base.Active(direct);
        }

    }
}
