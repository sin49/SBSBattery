using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamCollideer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("레이저 빔 피격");
            PlayerHandler.instance.CurrentPlayer.Damaged(1);
        }
    }
}
