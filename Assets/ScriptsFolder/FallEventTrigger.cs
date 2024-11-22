using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEventTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("낙사 판정 실행");
            PlayerHandler.instance.PlayerFallEventInvoke();
        }
    }
}
