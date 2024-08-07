using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CranePlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {

            collision.transform.SetParent(this.transform);
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {

            collision.transform.SetParent(null);

        }
    }
}