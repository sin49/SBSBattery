using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDescriptionUi : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
  public void UpdateInfo(item i)
    {
        Title.text = i.itemname;
        Description.text = i.itemdescription;
    }
    public void UpdateInfo(MUltiPlyitem i,int n)
    {
        Title.text = i.itemname;
        Description.text = i.itemdescription+"\n 효과 적용치: "+
            i.ReturnItemPower(n);
    }
}
