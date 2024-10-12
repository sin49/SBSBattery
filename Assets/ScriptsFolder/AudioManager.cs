using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public enum AudioType { BG,SE}
public class AudioManager : MonoBehaviour
{
    public AudioMixer defaultMIxergroup;
 public  AudioMixerGroup SE;
    public AudioMixerGroup BG;

    public static AudioManager instance;
    //AudioSource BackGrouundAudioSource;
    HashSet<SEPlayer> SEAudioSources = new HashSet<SEPlayer>();
 public   void setAudiogroupSettingBG(AudioSource a)
    {
        a.outputAudioMixerGroup = BG;
    }
   public void setAudiogroupSettingSE(AudioSource a)
    {
        a.outputAudioMixerGroup = SE;
    }
    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("LastestMasterVolume"))
            MasterVolume = PlayerPrefs.GetFloat("LastestMasterVolume");
        if (PlayerPrefs.HasKey("LastestBgmVolume"))
            BGVolume = PlayerPrefs.GetFloat("LastestBgmVolume");
        if (PlayerPrefs.HasKey("LastestSeVolume"))
            SEVolume = PlayerPrefs.GetFloat("LastestSeVolume");
    }
    [Header("백그라운드 오디오 볼륨"), Range(0, 1)]
    public float BGVolume;
    [Header("효과음 오디오 볼륨"), Range(0, 1)]
    public float SEVolume;
    [Header("마스터 오디오 볼륨"), Range(0, 1)]
    public float MasterVolume;
    void UpdateMixerSetting()
    {
        if(MasterVolume>0)
        defaultMIxergroup.SetFloat("MasterVolume",Mathf.Log10( MasterVolume)*20);
        else
            defaultMIxergroup.SetFloat("MasterVolume", -80);
        if(BGVolume>0)
        defaultMIxergroup.SetFloat("BGVolume", Mathf.Log10(BGVolume) * 20);
        else
            defaultMIxergroup.SetFloat("BGVolume", -80);
        if(SEVolume>0)
        defaultMIxergroup.SetFloat("SEVolume", Mathf.Log10(SEVolume) * 20);
        else
            defaultMIxergroup.SetFloat("SEVolume",-80);
    }
    public void GetAudioSetting(AudioType type,AudioSource source)
    {
        switch (type)
        {
            case AudioType.BG:
                //BackGrouundAudioSource = source;
                source.outputAudioMixerGroup = BG;
                break;
                case AudioType.SE:
                if(source.GetComponent<SEPlayer>()!=null)
                SEAudioSources.Add(source.GetComponent<SEPlayer>());
                source.outputAudioMixerGroup = SE;
                break;
            default:
                break;
        }
    }
    public void RemoveSEMember(SEPlayer se)
    {
        if(SEAudioSources.Contains(se))
            SEAudioSources.Remove(se);
    }
    void Update()
    {
        UpdateMixerSetting();
    }
}
