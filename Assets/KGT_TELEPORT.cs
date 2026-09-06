using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KGT_TELEPORT : MonoBehaviour
{
    public Transform Teleport_End;
    
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = Teleport_End.position;
        }
    }
}
