using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosFix : MonoBehaviour
{
    public Transform TF;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            if (transform.localPosition.z != collision.gameObject.transform.position.z&&PlayerStat.instance.MoveState==PlayerMoveState.SideX)
            {                
                Transform pos = collision.gameObject.transform;
                if (TF != null)
                    collision.gameObject.transform.position = new(pos.position.x, pos.position.y, TF.position.z);
                else
                    collision.gameObject.transform.position = new(pos.position.x, pos.position.y, transform.position.z);
            }

          else  if (transform.localPosition.x != collision.gameObject.transform.position.x && PlayerStat.instance.MoveState == PlayerMoveState.SideZ)
            {
                Transform pos = collision.gameObject.transform;
                if (TF != null)
                    collision.gameObject.transform.position = new(TF.position.x, pos.position.y, pos.position.z);
                else
                    collision.gameObject.transform.position = new(pos.position.x, pos.position.y, pos.position.z);
            }
        }
    }
}
