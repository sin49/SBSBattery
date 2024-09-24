using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEventTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHandler.instance.PlayerFallOut();
        }
    }
}
