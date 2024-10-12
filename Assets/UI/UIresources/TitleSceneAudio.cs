using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneAudio : MonoBehaviour
{
    ButtonSoundEffectPlayer sep;
    public bool active;

    private void Awake()
    {
        sep = GetComponent<ButtonSoundEffectPlayer>();
    }

    private void Update()
    {
        if (!active)
            return;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sep.PlaySelectAudio();
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C))
        {
            sep.PlayActiveAudio();
        }
    }
}
