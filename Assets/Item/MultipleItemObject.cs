using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleItemObject : ItemObject
{
    public UpgradeStatus status;
    MUltiPlyitem obj;
    public override void GetITemData(item data)
    {
        obj = data as MUltiPlyitem;
    }

    protected override void ItemPickUp()
    {
        PlayerInventory.instance.AddMultiplyItem(status);
    }

   
}
