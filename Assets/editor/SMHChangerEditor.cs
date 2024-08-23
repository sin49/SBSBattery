using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[CustomEditor(typeof(SMHChanger))]
public class SMHChangerEditor : Editor
{
    SMHChanger shmmanager;

    private void Awake()
    {
        shmmanager = (SMHChanger)target;
    }
   
  
    public override void OnInspectorGUI()
    {

        //if (GUILayout.Button("현재 SHM 설정을 저장한다"))
        //{
        //    if (shmmanager.volume != null)
        //    {
              
        //            shmmanager.SavePreset();
   

        //    }
        //}
        base.OnInspectorGUI();

    }
}
