using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
   public GameObject Mesh;
 public   ParticleSystem ParticleSystem;
    private void Awake()
    {
        ParticleSystem.gameObject.SetActive(false);
        Mesh.gameObject.SetActive(true);
    }
    void BoxBroke()
    {
        ParticleSystem.gameObject.SetActive(true);
        Mesh.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BoxBroke();
        }
    }
    private void OnDisable()
    {
        ParticleSystem.gameObject.SetActive(false);
        Mesh.gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player p;
            if(other.TryGetComponent<Player>(out p))
            {
                if (p.downAttack)
                {
                    BoxBroke();
                }
            }
        }
    }
}
