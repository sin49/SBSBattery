using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamCollideer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("������ �� �ǰ�");
            PlayerHandler.instance.CurrentPlayer.Damaged(1);
        }
    }
}
