using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class item2DCollider : MonoBehaviour
{
    public ItemObject itemobj;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (int)PlayerStat.instance.MoveState < 4)
        {
            itemobj.getitem();
        }
    }
}
