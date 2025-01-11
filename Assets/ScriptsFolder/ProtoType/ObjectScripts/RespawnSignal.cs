using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSignal : MonoBehaviour
{
    public static RespawnSignal Instance;

    [Header("재생성 알림UI")]
    public GameObject spawnUISignal;

    private void Awake()
    {
        Instance = this;
        spawnUISignal.SetActive(false);
    }

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

    IEnumerator cor;

    IEnumerator Respawn()
    {
        spawnUISignal.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        spawnUISignal.SetActive(false);
    }
}
