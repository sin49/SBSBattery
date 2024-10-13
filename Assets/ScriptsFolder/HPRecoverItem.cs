using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPRecoverItem : ItemObject
{
    public float HPRecoverPoint;

    public GameObject HealEffect;

    private void Start()
    {
        GetItemEffect = HealEffect;
    }
    public override void GetITemData(item data)
    {
    
    }
    protected override void ItemPickUp()
    {
        PlayerStat.instance.RecoverHP(HPRecoverPoint);
        Instantiate(HealEffect, PlayerHandler.instance.CurrentPlayer.transform.position,
            Quaternion.identity);
        Destroy(gameObject);
    }
    public override void getitem()
    {
        if (PlayerStat.instance.hp < PlayerStat.instance.hpMax)
        {
          
            base.getitem();
            ItemPickUp();
        }
    }

  
}
