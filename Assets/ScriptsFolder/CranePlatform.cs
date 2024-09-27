using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CranePlatform : MonoBehaviour
{
    public Crane crane;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {

            collision.transform.SetParent(this.transform);
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (crane.CraneMove)
                PlayerHandler.instance.playerjumprestirct();
            else
                PlayerHandler.instance.playerjumpaccept();

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHandler.instance.playerjumpaccept();
            collision.transform.SetParent(null);

        }
    }
}