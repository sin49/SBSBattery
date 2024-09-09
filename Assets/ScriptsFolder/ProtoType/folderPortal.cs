using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class folderPortal : MonoBehaviour
{
    public event Action Portalevent;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Portalevent.Invoke();
            Portalevent = null;
            this.transform.parent.gameObject.SetActive(false);
        }
    }
}
