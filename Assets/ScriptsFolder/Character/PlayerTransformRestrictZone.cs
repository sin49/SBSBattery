using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformRestrictZone : MonoBehaviour
{
    public Transform WayPoint;
    public TransformType AcceptType;
    private void OnTriggerStay(Collider other)
    {
        if (PlayerHandler.instance.CurrentType != AcceptType && other.CompareTag("Player"))
        {
            //Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            CapsuleCollider col = other.gameObject.GetComponent<CapsuleCollider>();
            col.enabled = false;

            other.gameObject.transform.position = WayPoint.position;
            col.enabled = true;


        }
    }
   
}
