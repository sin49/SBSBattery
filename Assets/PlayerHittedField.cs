using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHittedField : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHandler.instance.CurrentPlayer.Damaged(1);
        }
    }
}
