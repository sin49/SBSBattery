using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : MonoBehaviour
{
    public abstract void GetITemData(item data);

    protected abstract void ItemPickUp();


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemPickUp();
                this.gameObject.SetActive(false);
        
        }
    }
}
