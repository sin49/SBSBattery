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
            GUILayout.TextArea("üũ����Ʈ ��ȣ:" + PlayerPrefs.GetInt("CheckPointIndex"));
        else
            GUILayout.TextArea("üũ����Ʈ ���� �� ����");

        if (GUILayout.Button("üũ����Ʈ �ʱ�ȭ"))
        {
            PlayerPrefs.SetString("LastestStageName", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetInt("CheckPointIndex", 0);
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("üũ����Ʈ �ڷ�"))
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
        if (GUILayout.Button("üũ����Ʈ ������"))
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

        string s="�۵� ����";
        switch (transformtype)
        {
            case TransformType.Default:
                s = "���͸�";
                break;
            case TransformType.remoteform:
                s = "������";
                break;
            case TransformType.ironform:
                s = "�ٸ���";
                break;
            case TransformType.mouseform:
                s = "���콺";
                break;
            default:
                break;
        }
        EditorGUILayout.LabelField("���� ����", s);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("���� �� �ڷ�"))
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
        if (GUILayout.Button("���� �� ������"))
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
