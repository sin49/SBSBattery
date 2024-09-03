using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantItemObject : ItemObject
{
    public instantitem instantitem;
    public override void GetITemData(item data)
    {
        instantitem = data as instantitem;
    }

    protected override void ItemPickUp()
    {
  PlayerInventory.instance.instants.Add(instantitem.ItemCode,instantitem);
    }

}
