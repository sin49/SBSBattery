using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemObject : MonoBehaviour
{
    public abstract void GetITemData(item data);
    public GameObject GetItemEffect;
    protected abstract void ItemPickUp();

    public void createitemeffect()
    {
      Instantiate(  GetItemEffect,PlayerHandler.instance.CurrentPlayer.transform.position, Quaternion.identity );
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ItemPickUp();
            createitemeffect();
                this.gameObject.SetActive(false);
        
        }
    }
}
