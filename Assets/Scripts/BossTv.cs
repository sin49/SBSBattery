using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTv : MonoBehaviour
{
    public GameObject Monitor;
    public BossHandle LHand;
    public BossHandle RHand;

    public GameObject Spotlight;

    public int lifeCount;
    public int lifeCountMax;

    private void Start()
    {
        //LSweaper();
        RSpotlight();
    }
    public void LSweaper()
    {
        StartCoroutine(LHand.Sweaper());
    }
    public void RSweaper()
    {
        StartCoroutine(RHand.Sweaper());
    }
    public void RSpotlight()
    {
        RHand.SpotLightSHow();
        //StartCoroutine(RHand.SpotLightShow());
    }
    
    public void LSpotlight()
    {
        LHand.SpotLightSHow();
        //StartCoroutine(LHand.SpotLightShow());
    }
}
