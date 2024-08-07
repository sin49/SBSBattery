using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "New MUltiPly item", menuName = "Custom/new MUltiPly item")]
public class MUltiPlyitem : item
{


    public float InitItemPower;
    public float ItemPower;
    public UpgradeStatus upgradeStatus;
    void ItemEffect(int itemnumber)
    {
        switch (upgradeStatus)
        {
            case UpgradeStatus.Energy:
                PlayerStat.instance.HPBonus -= ReturnItemPower(itemnumber - 1);
                PlayerStat.instance.HPBonus += ReturnItemPower(itemnumber);
                break;
            case UpgradeStatus.MoveSpeed:
                PlayerStat.instance.MoveSpeedBonus -= ReturnItemPower(itemnumber - 1);
                PlayerStat.instance.MoveSpeedBonus += ReturnItemPower(itemnumber);
                break;
        }
    }
    public void GetItem(int number)
    {

        ItemEffect(number);
    }
  public  float ReturnItemPower(int number)
    {
        if (number <= 0) return 0;
       return InitItemPower + ItemPower * number;
    }

}
