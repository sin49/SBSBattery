using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialItemObject : ItemObject
{
    //언젠간 에디터 만들어서 정상화 할지도.......
 public   Essentialitem item;

    public override void GetITemData(item data)
    {
       item=data as Essentialitem;
    }

    protected override void ItemPickUp()
    {
        PlayerInventory.instance.ADDEssentialItem(item);
    }

}
