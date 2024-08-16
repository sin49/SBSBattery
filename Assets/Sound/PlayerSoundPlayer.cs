using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public interface SEPlayer
{

}

public class PlayerSoundPlayer : MonoBehaviour, SEPlayer
{
    [Header("이동 사운드")]
    public AudioClip MoveClip;
    [Header("이동 사운드 볼륨"), Range(0, 1)]
    public float MoveVolume;

    [Header("공격 사운드")]
    public AudioClip AttackClip;
    [Header("공격 사운드 볼륨"),Range(0,1)]
    public float AttackVolume;
    [Header("점프 사운드")]
    public AudioClip JumpClip;
    [Header("점프 사운드 볼륨"),Range(0,1)]
    public float JumpVolume;


    AudioType audiotype = AudioType.SE;

 AudioSource audiosource;
    private void Awake()
    {

            audiosource = gameObject.AddComponent<AudioSource>();
            audiosource.minDistance = audiosource.maxDistance;
            audiosource.dopplerLevel = 0;
            audiosource.loop = false;


    }
    private void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.GetAudioSetting(audiotype, audiosource);
        }
    }

    public void PlayAttackAudio()
    {
        if (AttackClip != null)
        {
            audiosource.Stop();
            audiosource.clip = AttackClip;
            audiosource.volume = AttackVolume;
            audiosource.Play();
        }
    }
    public void PlayJumpAudio()
    {
        audiosource.Stop();
        audiosource.clip = JumpClip;
        audiosource.volume = JumpVolume;
        audiosource.Play();
    }
    private void OnDestroy()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.RemoveSEMember(this);
        }
    }
}
