using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemIconUI : MonoBehaviour
{
    protected item i;

 
    
  public virtual void SetItem(item i)
    {

    this.i = i;

    }

    public item Getitem()
    {
        return i;
    }
}
