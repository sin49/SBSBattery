using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("0번 세이브 시작 사운드")]
    public ParticleSystem ChkPointParticle;
    public SoundEffectListPlayer soundplayer;
    private void Awake()
    {
        ChkPointParticle.gameObject.SetActive(false);
        soundplayer =GetComponent<SoundEffectListPlayer>();
    }
    public int index;
    public GameObject spawn(GameObject obj)
    {
        var player= Instantiate(obj, ChkPointParticle.transform.position, ChkPointParticle.transform.rotation);
        ChkPointParticle.gameObject.SetActive(true);
     PlayerHandler.instance.   registerRemoteUI(player);
        return player;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
       
            if (PlayerSpawnManager.Instance.CurrentCheckPoint != this)
            {
                PlayerSpawnManager.Instance.ChangeCheckPoint(this);
                if(soundplayer!=null) 
                    soundplayer.PlayAudio(0);
                Debug.Log($"체크포인트{index}에 닿음");
            }
        }
    }
}
