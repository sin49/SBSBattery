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

        //if (GUILayout.Button("���� SHM ������ �����Ѵ�"))
        //{
        //    if (shmmanager.volume != null)
        //    {
              
        //            shmmanager.SavePreset();
   

        //    }
        //}
        base.OnInspectorGUI();

    }
}
