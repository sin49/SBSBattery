using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialItemObject : ItemObject
{
    //������ ������ ���� ����ȭ ������.......
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
