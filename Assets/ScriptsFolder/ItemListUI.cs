using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemListUI : MonoBehaviour
{
    public ItemIconUI itemicon;

    List<ItemIconUI> EssentialItemList=new List<ItemIconUI>();
    public Transform EssentialItemListTransform;
    public SettingUI settingUi;
    public SelectUI KjsSetting;

    public MultiplyItemIconUI EnergyMultiple;
    public MultiplyItemIconUI SpeedMultiple;
    List<ItemIconUI> MultiplyItemList = new List<ItemIconUI>();
    public Transform MultiplyItemListTransform;


    List<ItemIconUI> handleitemlist;

    ItemIconUI selectitemicon;

    int index;

    public bool OnHandle;

    public GameObject SelectedUI;

    public ItemDescriptionUi ItemDescriptionUI;

    void swapUI()
    {
        if (settingUi != null)
            settingUi.ActiveUI();
        else
        {
            if (KjsSetting != null)
            {
                gameObject.SetActive(false);
                KjsSetting.uiGroup.gameObject.SetActive(true);
                KjsSetting.ActiveUI();
            }
        }
        OnHandle = false;
        UpdateSelectInfo();
    }

    void AddItem(item i,List<ItemIconUI> list,Transform t)
    {
        var a=Instantiate(itemicon.gameObject,t).GetComponent<ItemIconUI>();
        a.SetItem(i);
        list.Add(a);
    }
    void AddItem(MultiplyItemIconUI ui,MUltiPlyitem i,int number, List<ItemIconUI> list, Transform t)
    {
        var a = Instantiate(ui.gameObject, t).GetComponent<MultiplyItemIconUI>();
        a.SetItem(i, number);
        list.Add(a);
    }
    public void ActiveItemListUI()
    {
        OnHandle = true;
        if (EssentialItemList.Count != 0)
        {
            index = EssentialItemList.Count - 1;

            handleitemlist = EssentialItemList;
        }
        else
        {
            handleitemlist = MultiplyItemList;
            index = MultiplyItemList.Count - 1;
 
        }
        UpdateSelectInfo();
    }
 
   void initializeItemList()
    {

        OnHandle = false;
        handleitemlist = EssentialItemList;
        var elist = PlayerInventory.instance.returnEssentialItems();
        for(int n = 0; n < elist.Count; n++)
        {
            if (EssentialItemList.Count  <= n)
            {
                AddItem(elist[n], EssentialItemList, EssentialItemListTransform);
            }
            else
            {
                EssentialItemList[n].SetItem(elist[n]);
            }
        }
       
        var Mlist = PlayerInventory.instance.ReturnMultipluNumber();
        //foreach(var item in PlayerInventory.instance.MultiplyItems)
        //{
        //    AddItem(item, Mlist[item.upgradeStatus], MultiplyItemList, MultiplyItemListTransform);
        //}
        for (int n = 0; n < PlayerInventory.instance.MultiplyItems.Length; n++)
        {
            if (MultiplyItemList.Count  <= n)
            {
               
               
         
                switch (
                    PlayerInventory.instance.MultiplyItems[n].upgradeStatus)
                {
                    case UpgradeStatus.Energy:
                        AddItem(EnergyMultiple,PlayerInventory.instance.MultiplyItems[n], Mlist[
                    PlayerInventory.instance.MultiplyItems[n].upgradeStatus], MultiplyItemList, MultiplyItemListTransform);
                        break;
                    case UpgradeStatus.MoveSpeed:
                        AddItem(SpeedMultiple,PlayerInventory.instance.MultiplyItems[n], Mlist[
                   PlayerInventory.instance.MultiplyItems[n].upgradeStatus], MultiplyItemList, MultiplyItemListTransform);
                        break;
                }
            }
            else
            {
                var a = MultiplyItemList[n] as MultiplyItemIconUI;
                    a.SetItem(PlayerInventory.instance.MultiplyItems[n], Mlist[
                    PlayerInventory.instance.MultiplyItems[n].upgradeStatus]);
            }
        }

       

        UpdateSelectInfo();
    }
    void UpdateSelectInfo()
    {

        if (OnHandle)
        {
            SelectedUI.SetActive(true);
            ItemDescriptionUI.gameObject.SetActive(true);
        }
        else
        {
            SelectedUI.SetActive(false);
            ItemDescriptionUI.gameObject.SetActive(false);
            return;
        }
        if (index >= handleitemlist.Count)
        {
            index = handleitemlist.Count - 1;
        }
        selectitemicon = handleitemlist[index];
        SelectedUI.transform.position=selectitemicon.transform.position;
        if (handleitemlist == EssentialItemList)
            ItemDescriptionUI.UpdateInfo(handleitemlist[index].Getitem());
        else
        {
         MultiplyItemIconUI ui= handleitemlist[index] as MultiplyItemIconUI;
            var tuple = ui.GetMultiplyitem();
            ItemDescriptionUI.UpdateInfo(tuple.Item1, tuple.Item2);
        }
        //description에다 아이템 정보 받아서 제목,설명 표기
    }
    void handleItemList()
    {
        if (!OnHandle)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (index < handleitemlist.Count-1)
            {
                index++;
                UpdateSelectInfo();
            }
            /*else
            {
                swapUI();
            }*/
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (index >0)
            {
                index--;
                UpdateSelectInfo();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(handleitemlist!=EssentialItemList)
            {
                handleitemlist = EssentialItemList;
                UpdateSelectInfo();
            }    
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (handleitemlist == EssentialItemList)
            {
                //여기에 multiply
                handleitemlist = MultiplyItemList;
                UpdateSelectInfo();
            }
        }

        if (Input.GetKeyDown(KeySettingManager.instance.UIdeactiveKeycode))
        {
            swapUI();
        }
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    DeactiveItemListUI();
   
        //}
    }

    private void OnEnable()
    {
        initializeItemList();
    }
  
    // Update is called once per frame
    void Update()
    {
        handleItemList();
    }
}
