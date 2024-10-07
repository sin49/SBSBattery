using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHoldFormSoundPlayer : PlayerSoundPlayer
{
    [Header("돌진 중 재생 사운드")]
    public AudioClip rushingClip;
    [Header("돌진 중 재생 볼륨"), Range(0, 1)]
    public float rushingVolume;

   


    public bool rushing;

    public void rushsoundend()
    {
        audiosource.loop = false;
        rushing = false;
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

        if (!rushing)
        {
            audiosource.Stop();
           
            audiosource.loop = true;
            rushing = true;
         
        }
      
            audiosource.clip = rushingClip;
            audiosource.volume = rushingVolume;
            audiosource.Play();

    }

}
