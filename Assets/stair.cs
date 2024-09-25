using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class stair : MonoBehaviour
{
    public float upgradeDownForce = 60;
    public float upgradeMoveforce = 4;
    public Collider StairColliders;
    public Player p;
    public PhysicMaterial m;

    private void FixedUpdate()
    {
        updatestair();
    }
    void updatestair()
    {
        if (p != null)
        {
            if (!p.isRun)
            {
                StairColliders.material = m;
            }
            else
            {
                StairColliders.material = null;
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            p = PlayerHandler.instance.CurrentPlayer;
            p.onstairforce = upgradeMoveforce;
            p.stairdownforce = upgradeDownForce;
           p.onstair = true;
      
        
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHandler.instance.CurrentPlayer.onstair = false;
            p = null;
        }
    }
}
