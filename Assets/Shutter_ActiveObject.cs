using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutter_ActiveObject : Shutter
{
    [Header("활성화되기 까지의 시간 이걸로 에니메이션과 맞추샘")]
    public float initTIme = 0.15f;
    float inittimer;
    [Header("활성화 오브젝트 위치(활성화 오브젝트와 같은 위치여도 상관 없음)")]
    public Transform ActiveTransform;
    [Header("활성화 오브젝트")]
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
