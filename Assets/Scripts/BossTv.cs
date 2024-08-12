using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTv : MonoBehaviour
{
    public GameObject Monitor;
    public BossHandle LHand;
    public BossHandle RHand;

    public BossFalling bf;

    public int lifeCount;
    public int lifeCountMax;

    private void Start()
    {
        //LSweaper();
        //RSpotlight();
        FallingAttack();
    }
    public void LSweaper()
    {
        StartCoroutine(LHand.Sweaper());
    }
    public void RSweaper()
    {
        StartCoroutine(RHand.Sweaper());
    }    

    public void FallingAttack()
    {
        bf.CreateFallingObject();
    }
}
