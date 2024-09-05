using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class InvetorySaveData
{
    public List<EssentialitemData> essentialitems = new List<EssentialitemData>();
    public List<UpgradeStatus> Upgradesstatus = new List<UpgradeStatus>();
    public List<int> Multiplys = new List<int>();
    public InvetorySaveData(){
        essentialitems = new List<EssentialitemData>();
        
        Upgradesstatus = new List<UpgradeStatus>();
     int n=   Enum.GetValues(typeof(UpgradeStatus)).Length;
  
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

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
   Dictionary<string, Essentialitem> EssentialItems = new Dictionary<string, Essentialitem>();
  public Dictionary<string,instantitem> instants= new Dictionary<string,instantitem>();
    event Action itemGetAction;
    public bool checkinstantitem(string s)
    {
        if (instants.ContainsKey(s))
        {
            instants.Remove(s);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void registerItemGetAction(Action a)
    {
        itemGetAction += a;
    }
    public List<Essentialitem> returnEssentialItems()
    {
        List<Essentialitem> list= new List<Essentialitem>();
        foreach (KeyValuePair<string, Essentialitem> kvp in EssentialItems)
        {
            list.Add(kvp.Value);
        }
        return list;
    }
    public ItemUI itemui;

    public MUltiPlyitem[] MultiplyItems=new MUltiPlyitem[2];
   


    Dictionary<UpgradeStatus, MUltiPlyitem> MultiplyitemDict = new Dictionary<UpgradeStatus, MUltiPlyitem>();
    Dictionary<UpgradeStatus, int> MultiplyitemNumberDict = new Dictionary<UpgradeStatus, int>();
    public Dictionary<UpgradeStatus, int> ReturnMultipluNumber()
    {
        return MultiplyitemNumberDict;
    }
  public void SaveData(InvetorySaveData savedata)
    {
        string json = JsonUtility.ToJson(savedata);
        string filePath = Path.Combine(Application.persistentDataPath, "InventorySave.json");
        File.WriteAllText(filePath, json);
    }
    public void SaveInventoryData()
    {
        InvetorySaveData saveData = new InvetorySaveData();
        saveData.essentialitems.Clear();
       foreach (KeyValuePair<string,Essentialitem> kvp in EssentialItems)
        {
            EssentialitemData e = new EssentialitemData(kvp.Value);
            saveData.essentialitems.Add(e);
        }
        saveData.Upgradesstatus.Clear();
        saveData.Multiplys.Clear();
        foreach (KeyValuePair<UpgradeStatus, int> item in MultiplyitemNumberDict)
        {
           
           
            saveData.Upgradesstatus.Add(item.Key);
            saveData.Multiplys.Add(item.Value);
        }
        SaveData(saveData);
    }
   public InvetorySaveData LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "InventorySave.json");
        if (File.Exists(filePath))
        {
            var a = File.ReadAllText(filePath);
            return JsonUtility.FromJson<InvetorySaveData>(a);
        }
        else
            return null;
    }
    public void LoadInventoryData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "InventorySave.json");
        EssentialItems.Clear();
        MultiplyitemNumberDict.Clear();
        for (int n = 0; n < MultiplyItems.Length; n++)
        {
            if (MultiplyItems[n] != null)
            {
                MultiplyitemNumberDict.Add(MultiplyItems[n].upgradeStatus, 0);
            }
        }
        if (File.Exists(filePath))
        {


            InvetorySaveData savedata = LoadData();

     
            foreach(EssentialitemData e in savedata.essentialitems)
            {
                Essentialitem Eitem= ScriptableObject.CreateInstance<Essentialitem>();
               Eitem.itemname = e.itemname;
                Eitem.itemdescription = e.itemdescription;
                Eitem.itemcode=e.itemcode;
                EssentialItems.Add(Eitem.itemcode, Eitem);
            }
     
           for(int n = 0; n < savedata.Upgradesstatus.Count; n++)
            {
                MultiplyitemNumberDict[savedata.Upgradesstatus[n]] = savedata.Multiplys[n];
            }
           foreach(MUltiPlyitem i in MultiplyItems)
            {
                i.GetItem(MultiplyitemNumberDict[i.upgradeStatus]);
            }
        }
        else
        {
          
        }
    }
    private void Awake()
    {
        instance = this;
        for (int n = 0; n < MultiplyItems.Length; n++)
        {
            if (MultiplyItems[n] != null)
            {
                MultiplyitemDict.Add(MultiplyItems[n].upgradeStatus, MultiplyItems[n]);
                MultiplyitemNumberDict.Add(MultiplyItems[n].upgradeStatus, 0);
            }
        }
    }
    public bool checkessesntialitem(string itemcode)
    {
        if (EssentialItems.ContainsKey(itemcode))
            return true;
        else
            return false;
    }
    public List<string> returnitemkeys()
    {
        List<string> strings = new List<string>();
        foreach (string s in EssentialItems.Keys)
        {
            strings.Add(s);
        }
        return strings;
    }
    //public int[] returnMultiplyitemkeys()
    //{
    //    int[] ints = new int[MultiplyItems.Length];
    //    for(int n = 0; n < MultiplyItems.Length; n++)
    //    {
    //        ints[n] = MultiplyItems[n].itemnumber;
    //    }
     
    //    return ints;
    //}
   
    public void ADDEssentialItem(Essentialitem i)
    {
 
        if(!EssentialItems.ContainsKey(i.itemcode))
            EssentialItems.Add(i.itemcode, i);
        SaveInventoryData();
        itemGetAction?.Invoke();
        itemui.activeUI(i);

    }
    public void AddMultiplyItem(UpgradeStatus s)
    {
        if (MultiplyitemDict.ContainsKey(s))
        {
            MultiplyitemNumberDict[s]++;
            MultiplyitemDict[s].GetItem(MultiplyitemNumberDict[s]);

            //SaveInventoryData();
            itemGetAction?.Invoke();
            itemui.activeUI(MultiplyitemDict[s]);
        }
    }
 

}
