using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer :  SEPlayer
{
    [Header("�Ÿ� ���� ����")]
    [Header("����7(���е� �ƴ�)�� ������ ����� ����� �� �ְ� ��")]
    [Header("����7�� �׽�Ʈ�� �� ���� �� �ϸ� ���ļ� ����Ǵ� �Ͱ� ����")]
    [Header("����� �����")]
    public AudioClip audioclip;
    [Header("�� ������� ����"),Range(0,1)]
    public float volume;

 

    bool OnViewport;
 
    private void OnBecameVisible()
    {
        OnViewport = true;
    }
    private void OnBecameInvisible()
    {
        OnViewport = false;
    }
  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            PlayAudio();
        }
    }
    public void PlayAudio()
    {
        if (audioclip != null)
        {
            audiosource.clip = audioclip;
            audiosource.volume = volume;
            audiosource.Play();
        }
    }
   
}
