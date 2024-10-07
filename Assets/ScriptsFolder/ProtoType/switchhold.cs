using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchhold : Switch
{
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")
            ||
            collision.gameObject.CompareTag("CursorObject"))
        {
            Debug.Log("Switch DeActive");
            active = false;
            Send(active);
        }
    }
}
