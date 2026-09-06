
using System;
using System.Linq;

using UnityEngine;

public class HouseholdIron : Player
{
    public SpecialAttackInfo saGroup;    

    public override void Skill1()
    {
        GameObject sa =  Instantiate(saGroup.saPrefab, firePoint.position, Quaternion.identity);
        sa.GetComponent<SpecialMeleeCollider>().SetDamage(PlayerStat.instance.atk);
        PlayerHandler.instance.CurrentPower -= saGroup.saPowerEnergy;
    }
}
