using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rushattackcollider : MonoBehaviour
{

    public float damage;
    public event Action rushendevent;
    public float playerforce;
    public void registerrushendevent(Action a)
    {
        rushendevent += a;
    }
 
    IEnumerator playerforced(Player p)
    {
        PlayerHandler.instance.CantHandle = true;
        p.playerRb.AddForce(this.transform.forward * playerforce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.25f);
        PlayerHandler.instance.CantHandle = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rushendevent.Invoke();
            Player p = PlayerHandler.instance.CurrentPlayer;
            if (!p.onInvincible)
            {
                p.Damaged(damage);
                StartCoroutine(playerforced(p));
            }
        }
    }
    
}
