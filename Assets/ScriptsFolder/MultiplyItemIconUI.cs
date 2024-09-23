using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplyItemIconUI : ItemIconUI
{
    int number;
    public Transform numbertransform;
    public Image numberimage;
    List<Image> Images=new List<Image>();
    public void SetItem(MUltiPlyitem i,int n)
    {
        SetItem(i);
        number = n;
        for(int num = 0; num < number; num++)
        {
            if (Images.Count <= num)
            {
            var a=    Instantiate(numberimage.gameObject, numbertransform).GetComponent<Image>();
                Images.Add(a);
            }
        }
    ;
    }
    public Tuple<MUltiPlyitem,int> GetMultiplyitem()
    {

        return new Tuple<MUltiPlyitem, int>(
            i as MUltiPlyitem,number
            );
    }
}
