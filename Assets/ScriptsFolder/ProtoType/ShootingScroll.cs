using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingScroll : MonoBehaviour
{
    Slider slider;

    int maxvalue=1;
    int CurrentValue=0;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
  public  void setSlide(int n)
    {
        maxvalue = n;
        CurrentValue = 0;
    }
  public void GetVaule()
    {
        CurrentValue++;
    }
    void Update()
    {
        slider.maxValue = maxvalue;
        slider.value = CurrentValue;
    }
}
