using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
 public   ParticleSystem ParticleSystem_;
    private void Awake()
    {
        ParticleSystem_.gameObject.SetActive(false);
      gameObject.SetActive(true);
    }
    void BoxBroke()
    {
        ParticleSystem_.gameObject.SetActive(true);
        ParticleSystem_.transform.SetParent(null);
      gameObject.SetActive(false);
    }
  
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            DownAttackCollider p;
            if(other.TryGetComponent<DownAttackCollider>(out p))
            {
                //if (p.downAttack)
                //{
                    BoxBroke();
                //}
            }
        }
    }
}
