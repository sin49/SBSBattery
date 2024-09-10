using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1UI : MonoBehaviour
{
    public Slider MonitorHP;
    public Slider RhandHP;
    public Slider LhandHP;

    public BossTv TV;

    // Update is called once per frame
    void Update()
    {
        if (TV != null)
        {
            MonitorHP.maxValue = TV.lifeCountMax;
            MonitorHP.value=TV.lifeCount;

            RhandHP.maxValue = TV.HandHP;
            LhandHP.maxValue = TV.HandHP;

            RhandHP.value = TV.RHand.HP;
            LhandHP.value = TV.LHand.HP;
        }
    }
}
