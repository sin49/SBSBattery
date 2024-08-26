using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePassOutputEvent : OutputEvent
{
    public string scenename;
    public override void output()
    {
        
        base.output();
        GameManager.instance.LoadingSceneWithKariEffect(scenename);
    }
}
