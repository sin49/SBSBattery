using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ironRushMeleeSoundPlayer2D : MonoBehaviour
{
    IronRushCollider collide;
    private void Start()
    {
        collide=this.transform.parent.GetComponent<IronRushCollider>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collide.soundEffectListPlayer.PlayAudio(0);
        }
    }
}
