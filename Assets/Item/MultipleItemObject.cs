using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleItemObject : ItemObject
{
    public UpgradeStatus status;
    public MUltiPlyitem obj;
    public override void GetITemData(item data)
    {
        obj = data as MUltiPlyitem;
    }

    protected override void ItemPickUp(string s)
    {
        if (!PlayerInventory.instance.itemdatas.ContainsKey(itemindex))
        {
            obj.itemcode = s;
            PlayerInventory.instance.AddMultiplyItem(obj);
        }
    }


}
