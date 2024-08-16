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
    [Header("��׶��� ����� ����"),Range(0,1)]
    public float BGVolume;
    [Header("ȿ���� ����� ����"), Range(0, 1)]
    public float SEVolume;
    [Header("������ ����� ����"), Range(0, 1)]
    public float MasterVolume;
    public static AudioManager instance;
    //AudioSource BackGrouundAudioSource;
    HashSet<SEPlayer> SEAudioSources = new HashSet<SEPlayer>();
    private void Awake()
    {
        instance = this;
    }
    void UpdateMixerSetting()
    {
        if(MasterVolume>0)
        defaultMIxergroup.SetFloat("MasterVolume",Mathf.Log10( MasterVolume)*20);
        else
            defaultMIxergroup.SetFloat("MasterVolume", 0);
        if(BGVolume>0)
        defaultMIxergroup.SetFloat("BGVolume", Mathf.Log10(BGVolume) * 20);
        else
            defaultMIxergroup.SetFloat("BGVolume", 0);
        if(SEVolume>0)
        defaultMIxergroup.SetFloat("SEVolume", Mathf.Log10(SEVolume) * 20);
        else
            defaultMIxergroup.SetFloat("SEVolume",0);
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
