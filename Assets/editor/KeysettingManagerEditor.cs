using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(KeySettingManager))]
public class KeysettingManagerEditor : Editor
{
    KeySettingManager m_Instance;
    private void Awake()
    {
        m_Instance = (KeySettingManager)target;
    }
    void createpreset()
    {
        KeysettingPreset k=ScriptableObject.CreateInstance<KeysettingPreset>();
        k.attackkeycode = (int)m_Instance.AttackKeycode;
        k.jumpkeycode = (int)m_Instance.jumpKeycode;
        k.dimensionchangekeycode = (int)m_Instance.DimensionChangeKeycode;
        k.skillkeycode = (int)m_Instance.SkillKeycode;
        k.downattackkeycode = (int)m_Instance.DownAttackKeycode;
        k.interactkeycode = (int)m_Instance.InteractKeycode;
        k.deformkeycode = (int)m_Instance.DeformKeycode;



        AssetDatabase.CreateAsset(k,$"Asset\\KeySetting\\{m_Instance.keysettingpresetname}.asset");
        AssetDatabase.SaveAssets();
        m_Instance.preset = k;
    }
    void LoadPreset(KeysettingPreset kp)
    {
        m_Instance.AttackKeycode =(KeyCode) kp.attackkeycode;
        m_Instance.jumpKeycode = (KeyCode)kp.jumpkeycode;
        m_Instance.DimensionChangeKeycode =(KeyCode)kp.dimensionchangekeycode;
        m_Instance.SkillKeycode =(KeyCode)kp.skillkeycode;
        m_Instance.DownAttackKeycode =(KeyCode)kp.downattackkeycode;
        m_Instance.InteractKeycode =(KeyCode)kp.interactkeycode;
        m_Instance.DeformKeycode =(KeyCode)kp.deformkeycode;
        m_Instance.keysettingpresetname = kp.name;
    }
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("프리셋 저장"))
        {
            createpreset();
        }
        if (GUILayout.Button("프리셋 불려오기"))
        {
            if (m_Instance.preset != null)
            {
                LoadPreset(m_Instance.preset);
            }
        }
        base.OnInspectorGUI();
    }
}
