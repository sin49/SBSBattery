using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEventTrigger : MonoBehaviour
{
    public FallEventSound feSound;

    private void Awake()
    {
        feSound = GetComponent<FallEventSound>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("낙사 판정 실행");
            feSound.FallingSoundPlay();
            PlayerHandler.instance.PlayerFallEventInvoke();
        }
    }
}
