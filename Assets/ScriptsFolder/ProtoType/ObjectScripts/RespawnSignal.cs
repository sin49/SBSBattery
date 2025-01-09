using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSignal : MonoBehaviour
{
    public static RespawnSignal Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Header("재생성 알림UI")]
    public GameObject spawnUISignal;
    IEnumerator cor;

    public void SignalStart()
    {
        if (cor != null)
        {
            StopCoroutine(cor);
            cor = null;
        }

        cor = Respawn();
        StartCoroutine(cor);
    }

    IEnumerator Respawn()
    {
        spawnUISignal.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        spawnUISignal.SetActive(false);
    }
}
