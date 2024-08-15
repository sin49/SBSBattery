using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutter_ActiveObject : Shutter
{
    public float initTIme = 0.15f;
    float inittimer;
    public Transform ActiveTransform;
    public GameObject ACtiveObject;
    protected override void Awake()
    {
        base.Awake();
        if (ACtiveObject.activeSelf)
        {
            ACtiveObject.SetActive(false);
        }
    }
    public override void CheckSignal()
    {
        base.CheckSignal();
        if (active == true)
        {
            StartCoroutine(CreateObjectEvent());
        }
        else
            if (ACtiveObject.activeSelf)
        {
            StopAllCoroutines();
            ACtiveObject.SetActive(false);
        }
    }
    IEnumerator CreateObjectEvent() {

        inittimer = initTIme;
        while (active)
        {
            if(initTIme>0)
            initTIme -= Time.fixedDeltaTime;
            else
            {
                ACtiveObject.SetActive(true);
                if (ActiveTransform != null)
                {
                    ACtiveObject.transform.position = ActiveTransform.position;
                }
                break;
            }
            yield return null;  
        }
    }
    
}
