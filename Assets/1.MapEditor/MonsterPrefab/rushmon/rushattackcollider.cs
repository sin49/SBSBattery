using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rushattackcollider : MonoBehaviour
{
    public Enemy rushenemy_;
    public float damage;
    public event Action rushendevent;
    public float playerforce;
    public void registerrushendevent(Action a)
    {
        rushendevent += a;
    }
    private void Awake()
    {
        rushenemy_=transform.parent.GetComponent<rushenemy>();
    }
    IEnumerator playerforced(Player p)
    {
        PlayerHandler.instance.CantHandle = true;
        p.playerRb.AddForce(rushenemy_.transform.forward * playerforce, ForceMode.Impulse);
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
