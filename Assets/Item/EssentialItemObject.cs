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

    protected override void ItemPickUp(string s)
    {
        if (!PlayerInventory.instance.itemdatas.ContainsKey(itemindex))
        {
            item.itemcode = s;
            PlayerInventory.instance.ADDEssentialItem(item);
        }
    }

    bool check;
    private void Update()
    {
        if(!check)
        Disable();
    }

    public void Disable()
    {
        Debug.Log("Disable 실행");
        check = true;
        //foreach (KeyValuePair<string, item> kvp in PlayerInventory.instance.itemdatas)
        //{
        //    Debug.Log("키값");
        //    Debug.Log($"key:{kvp.Key}, value{kvp.Value}");
        //}

        if (PlayerInventory.instance.itemdatas.ContainsKey(itemindex))
            gameObject.SetActive(false);
    }
}
