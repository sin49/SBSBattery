using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    int playerLayer;
    int monsterContactLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Physics.IgnoreLayerCollision(playerLayer, monsterContactLayer, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            Physics.IgnoreLayerCollision(playerLayer, monsterContactLayer, false);
        }
    }
}
