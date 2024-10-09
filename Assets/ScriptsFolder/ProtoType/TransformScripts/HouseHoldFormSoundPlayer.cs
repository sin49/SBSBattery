using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHoldFormSoundPlayer : PlayerSoundPlayer
{
    [Header("돌진 중 재생 사운드")]
    public AudioClip rushingClip;
    [Header("돌진 중 재생 볼륨"), Range(0, 1)]
    public float rushingVolume;

    [Header("돌진 벽꽝 재생 사운드")]
    public AudioClip WallColClip;
    [Header("돌진 벽꽝 재생 볼륨"), Range(0, 1)]
    public float WallColVolume;


    [Header("급정지 재생 사운드")]
    public AudioClip RushSTopClip;
    [Header("급정지 재생 볼륨"), Range(0, 1)]
    public float RushSTopVolume;


    public bool rushing;

    public void WallCollidePlay()
    {
        if (WallColClip == null)
            return;
        audiosource.Stop();
        audiosource.loop = false;
        rushing = false;
        audiosource.clip = WallColClip;
        audiosource.volume = WallColVolume;
        audiosource.Play();
    }

    public void rushsoundend()
    {
        audiosource.loop = false;
        rushing = false;
    }
    public void PlayRushStop()
    {
        if (RushSTopClip == null)
            return;
        audiosource.Stop();
        audiosource.loop = false;
        audiosource.clip = RushSTopClip;
        audiosource.volume = RushSTopVolume;

        audiosource.Play();
    }
    public void rushsoundpause()
    {
        if (rushing)
        {
            audiosource.Pause();
        }
    }
    public void rushsoundresume()
    {
        if (rushing)
        {
            audiosource.Play();
        }
    }
    public void rushingAudio()
    {
        if (rushingClip == null)
            return;
        if (!rushing)
        {
            audiosource.Stop();
      
            rushing = true;

        }
        else if(audiosource.isPlaying) 
        {
            return;
        }

        audiosource.loop = true;
        audiosource.clip = rushingClip;
            audiosource.volume = rushingVolume;
            audiosource.Play();

    }

}
