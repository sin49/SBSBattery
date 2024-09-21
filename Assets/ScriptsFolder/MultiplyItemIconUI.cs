using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplyItemIconUI : ItemIconUI
{
    int number;
    public TextMeshProUGUI numbertext;
    public void SetItem(MUltiPlyitem i,int n)
    {
        SetItem(i);
        number = n;
        numbertext.text = "X"+number;
    }
    public Tuple<MUltiPlyitem,int> GetMultiplyitem()
    {

        return new Tuple<MUltiPlyitem, int>(
            i as MUltiPlyitem,number
            );
    }
}
