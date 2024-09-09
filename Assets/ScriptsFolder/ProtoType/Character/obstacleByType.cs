using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleByType : MonoBehaviour
{
   public TransformType type;
    private void OnTriggerEnter(Collider other)
    {
       if((other.CompareTag("PlayerAttack")) && PlayerHandler.instance.CurrentType==type)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.collider.CompareTag("Player")) && PlayerHandler.instance.CurrentType == type)
        {
            Destroy(gameObject);
        }
    }
}
