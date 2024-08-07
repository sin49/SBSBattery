using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PlayerInventory))]
public class PlayerInventoryEditor : Editor
{
    PlayerInventory playerInventory;
    string filepath;
    InvetorySaveData savedata;
    bool showEssentialItems = true;
    bool showMultiplys = true;
    private void OnEnable()
    {
        playerInventory = (PlayerInventory)target;
        filepath = Path.Combine(Application.persistentDataPath, "InventorySave.json");
        savedata = playerInventory.LoadData();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("�κ��丮 �ʱ�ȭ"))
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
                playerInventory.SaveData(new InvetorySaveData());
                if(Application.isPlaying)
                    playerInventory.LoadInventoryData();
            }
       }
        if (savedata != null) {
            showEssentialItems = EditorGUILayout.Foldout(showEssentialItems, "������ ������");
            if (showEssentialItems)
            {
                EditorGUI.indentLevel++;
                for(int i = 0; i < savedata.essentialitems.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    savedata.essentialitems[i].itemname =
                        EditorGUILayout.TextField("������ �̸�", savedata.essentialitems[i].itemname);
                    if (GUILayout.Button("����"))
                    {
                        savedata.essentialitems.RemoveAt(i);
                        playerInventory.SaveData(savedata);
                        if (Application.isPlaying)
                            playerInventory.LoadInventoryData();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("������ ������ �߰�");
                EditorGUILayout.BeginHorizontal();
                Essentialitem newitem = (Essentialitem)EditorGUILayout.ObjectField(null, typeof(Essentialitem), false);
                if (GUILayout.Button("�߰�")&&newitem!=null)
                {
                    savedata.essentialitems.Add(new EssentialitemData(newitem));
                    playerInventory.SaveData(savedata);
                    if (Application.isPlaying)
                        playerInventory.LoadInventoryData();
                }
                EditorGUILayout.EndHorizontal();
            }
        showMultiplys= EditorGUILayout.Foldout(showMultiplys, "��ø ������");
            if (showMultiplys)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < savedata.Multiplys.Count; i++)
                {
                    string s;
                    var a = savedata.Upgradesstatus[i];
                    switch (a)
                    {
                        case UpgradeStatus.Energy:
                            s = "ü��";
                            break;
                        case UpgradeStatus.MoveSpeed:
                            s = "�̵� �ӵ�";
                            break;
                        default:
                            s = "���� �� ���� ����";
                            break;
                    }
                    savedata.Multiplys[i] = EditorGUILayout.IntField(s
                        , savedata.Multiplys[i]);
                }
                EditorGUI.indentLevel--;
            }
            if (GUI.changed)
            {
               playerInventory.SaveData(savedata);
                if (Application.isPlaying)
                    playerInventory.LoadInventoryData();
            }
        }

    }
}
