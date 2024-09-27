using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

[CustomEditor(typeof(PlayerSpawnManager))]
public class PlayerSapwnEditor : Editor
{
   PlayerSpawnManager spawn;
    private void OnEnable()
    {
        spawn = (PlayerSpawnManager)target;
    }
    public override void OnInspectorGUI()
    {
        if (PlayerPrefs.HasKey("CheckPointIndex"))
            GUILayout.TextArea("체크포인트 번호:" + PlayerPrefs.GetInt("CheckPointIndex"));
        else
            GUILayout.TextArea("체크포인트 저장 값 없음");

        if (GUILayout.Button("체크포인트 초기화"))
        {
            PlayerPrefs.SetString("LastestStageName", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetInt("CheckPointIndex", 0);
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("체크포인트 뒤로"))
        {
            int n = 0;
            PlayerPrefs.SetString("LastestStageName", SceneManager.GetActiveScene().name);
            if (PlayerPrefs.HasKey("CheckPointIndex"))
                n = PlayerPrefs.GetInt("CheckPointIndex");
            n--;
            if (n < 0)
            {
                n = 0;
            }
            PlayerPrefs.SetInt("CheckPointIndex", n);
            if (spawn.CheckpointChkCamera != null)
                spawn.CheckpointChkCamera.transform.position = spawn.Checkpoints[n].transform.position + Vector3.back * 4;
            if (Application.isPlaying)
                PlayerHandler.instance.CurrentPlayer.gameObject.transform.position = spawn.Checkpoints[n].transform.position;


        }
        if (GUILayout.Button("체크포인트 앞으로"))
        {
            int n = 0;
            PlayerPrefs.SetString("LastestStageName", SceneManager.GetActiveScene().name);
            if (PlayerPrefs.HasKey("CheckPointIndex"))
                n = PlayerPrefs.GetInt("CheckPointIndex");
            n++;
            if (n >= spawn.Checkpoints.Length)
            {
                n = spawn.Checkpoints.Length;
            }
            PlayerPrefs.SetInt("CheckPointIndex", n);
            if (spawn.CheckpointChkCamera!=null)
           spawn. CheckpointChkCamera.transform.position = spawn.Checkpoints[n].transform.position + Vector3.back * 4;
            if (Application.isPlaying)
                PlayerHandler.instance.CurrentPlayer.gameObject.transform.position = spawn.Checkpoints[n].transform.position;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        TransformType transformtype = TransformType.Default;
        if (PlayerPrefs.HasKey("TransformType"))
            transformtype =(TransformType) PlayerPrefs.GetInt("TransformType");

        string s="작동 안함";
        switch (transformtype)
        {
            case TransformType.Default:
                s = "배터리";
                break;
            case TransformType.remoteform:
                s = "리모컨";
                break;
            case TransformType.ironform:
                s = "다리미";
                break;
            case TransformType.mouseform:
                s = "마우스";
                break;
            default:
                break;
        }
        EditorGUILayout.LabelField("변신 상태", s);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("변신 폼 뒤로"))
        {
            if (transformtype != TransformType.Default)
            {
                transformtype--;
            }
            PlayerPrefs.SetInt("TransformType", (int)transformtype);
            if (Application.isPlaying)
            {
                PlayerHandler.instance.transformed(transformtype);
            }
        }
        if (GUILayout.Button("변신 폼 앞으로"))
        {
            if (transformtype != TransformType.mouseform)
            {
                transformtype++;
            }
            PlayerPrefs.SetInt("TransformType", (int)transformtype);
            if (Application.isPlaying)
            {
                PlayerHandler.instance.transformed(transformtype);
            }
        }
        EditorGUILayout.EndHorizontal();
        base.OnInspectorGUI();
      
       
    }
}
