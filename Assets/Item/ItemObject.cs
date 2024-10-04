using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public abstract class ItemObject : MonoBehaviour
{

    public abstract void GetITemData(item data);
    public GameObject GetItemEffect;
    bool itemactive;
    [Header("����Ʈ 0��: ������ �Ծ��� �� ���� �Ҹ�")]
    public SoundEffectListPlayer soundEffectListPlayer;
    public float timer = 1.5f;
    protected abstract void ItemPickUp();

    private void Awake()
    {
        soundEffectListPlayer = GetComponent<SoundEffectListPlayer>();
    }

    public void createitemeffect()
    {
      Instantiate(  GetItemEffect,PlayerHandler.instance.CurrentPlayer.transform.position, Quaternion.identity );
    }
    protected void getItemSoundPlay()
    {
        if( soundEffectListPlayer != null ) 
        soundEffectListPlayer.PlayAudio(0);
    }
    IEnumerator itemDeactivecorutine()
    {
        yield return new WaitForSeconds(timer);
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!itemactive)
            {
                getItemSoundPlay();
                ItemPickUp();
                createitemeffect();
                this.GetComponent<Renderer>().enabled = false ;
                StartCoroutine(itemDeactivecorutine());
            }
        
        }
    }
}
