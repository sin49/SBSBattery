using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemIconUI : MonoBehaviour
{
    protected item i;
    public TextMeshProUGUI text;
 
    
  public virtual void SetItem(item i)
    {

    this.i = i;
        text.text = i.name;
    }

    public item Getitem()
    {
        return i;
    }
}
