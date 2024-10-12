using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rushattackcollider : MonoBehaviour
{
    public rushenemy rushenemy_;

    private void Awake()
    {
        rushenemy_=transform.parent.GetComponent<rushenemy>();
    }
    IEnumerator playerforced(Player p)
    {
        PlayerHandler.instance.CantHandle = true;
        p.playerRb.AddForce(rushenemy_.transform.forward * rushenemy_.PlayerForce, ForceMode.Impulse);
        yield return new WaitForSeconds(rushenemy_.Playerstuntime);
        PlayerHandler.instance.CantHandle = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rushenemy_.stoprush();
            Player p = PlayerHandler.instance.CurrentPlayer;
            if (!p.onInvincible)
            {
                p.Damaged(1);
                StartCoroutine(playerforced(p));
            }
        }
    }
    
}
