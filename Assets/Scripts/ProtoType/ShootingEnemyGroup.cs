using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyGroup : MonoBehaviour
{
   HashSet<ShootingEnemy> Shootinggroupmembers=new HashSet<ShootingEnemy>();
    public event Action<ShootingEnemyGroup> OnwaveCleard;
    public void removeMember(ShootingEnemy member)
    {
        Shootinggroupmembers.Remove(member);
    }
    
    private void Awake()
    {

        foreach (var s in GetComponentsInChildren<ShootingEnemy>())
        {
            Shootinggroupmembers.Add(s);
  
            s.gameObject.SetActive(false);
            s.Destroyevent += RemoveMember;
        }
     
    }
  public void startwave()
    {
        foreach (var s in Shootinggroupmembers)
        {
        
            s.gameObject.SetActive(true);

        }
    }

     void RemoveMember(ShootingEnemy e)
    {
        if (Shootinggroupmembers.Contains(e))
        {
            Shootinggroupmembers.Remove(e);
            e.Destroyevent -= RemoveMember;
            OnwaveCleared();
        }
        
    }
  
 void OnwaveCleared()
    {
        if (Shootinggroupmembers.Count == 0)
        {
            OnwaveCleard?.Invoke(this);
        }
    }

}
