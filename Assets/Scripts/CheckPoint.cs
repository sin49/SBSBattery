using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("0�� ���̺� ���� ����")]
    public Transform ChkPointTestPrefab;
    public SoundEffectListPlayer soundplayer;
    private void Awake()
    {
        ChkPointTestPrefab.gameObject.SetActive(false);
        soundplayer=GetComponent<SoundEffectListPlayer>();
    }
    public int index;
    public GameObject spawn(GameObject obj)
    {
        var player= Instantiate(obj, ChkPointTestPrefab.position, ChkPointTestPrefab.rotation);
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
                Debug.Log($"üũ����Ʈ{index}�� ����");
            }
        }
    }
}
