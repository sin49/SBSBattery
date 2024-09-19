using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFormCursor : MonoBehaviour
{
    public bool onCatch;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("InteractivePlatform"))
        {
            if (!onCatch)
            {
                onCatch = true;
            }
        }
    }
}
