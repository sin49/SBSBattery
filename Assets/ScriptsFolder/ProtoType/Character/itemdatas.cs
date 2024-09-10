using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InvetorySaveData
{
    public List<EssentialitemData> essentialitems = new List<EssentialitemData>();
    public List<UpgradeStatus> Upgradesstatus = new List<UpgradeStatus>();
    public List<int> Multiplys = new List<int>();
    public InvetorySaveData()
    {
        essentialitems = new List<EssentialitemData>();

        Upgradesstatus = new List<UpgradeStatus>();
        int n = Enum.GetValues(typeof(UpgradeStatus)).Length;

        Multiplys = new List<int>();
        for (int i = 0; i < n; i++)
        {
            Upgradesstatus.Add((UpgradeStatus)i);
            Multiplys.Add(0);
        }
    }

}
[Serializable]
public class EssentialitemData
{
    public EssentialitemData(Essentialitem e)
    {
        itemname = e.itemname;
        itemdescription = e.itemdescription;
        itemcode = e.itemcode;
        Debug.Log("저장 아이템" + e.itemname + e.itemdescription);
    }
    public string itemname;
    public string itemdescription;
    public string itemcode;
}