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

        if(GUILayout.Button("인벤토리 초기화"))
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
            showEssentialItems = EditorGUILayout.Foldout(showEssentialItems, "에센셜 아이템");
            if (showEssentialItems)
            {
                EditorGUI.indentLevel++;
                for(int i = 0; i < savedata.essentialitems.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    savedata.essentialitems[i].itemname =
                        EditorGUILayout.TextField("아이템 이름", savedata.essentialitems[i].itemname);
                    if (GUILayout.Button("삭제"))
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

                EditorGUILayout.LabelField("에센셜 아이템 추가");
                EditorGUILayout.BeginHorizontal();
                Essentialitem newitem = (Essentialitem)EditorGUILayout.ObjectField(null, typeof(Essentialitem), false);
                if (GUILayout.Button("추가")&&newitem!=null)
                {
                    savedata.essentialitems.Add(new EssentialitemData(newitem));
                    playerInventory.SaveData(savedata);
                    if (Application.isPlaying)
                        playerInventory.LoadInventoryData();
                }
                EditorGUILayout.EndHorizontal();
            }
        showMultiplys= EditorGUILayout.Foldout(showMultiplys, "중첩 아이템");
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
                            s = "체력";
                            break;
                        case UpgradeStatus.MoveSpeed:
                            s = "이동 속도";
                            break;
                        default:
                            s = "아직 안 만듬 ㅅㄱ";
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
