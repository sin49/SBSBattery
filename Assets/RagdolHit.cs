using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdolHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            DamagedByPAttack script;
            if(other.TryGetComponent<DamagedByPAttack>(out script))
            {
                script.Damaged(1);
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
