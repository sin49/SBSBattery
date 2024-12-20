using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;
using UnityEngine.UI;

[Serializable]
public class DefaultKeyData
{
    public List<string> kName = new List<string>();
    public List<int> kNumber = new List<int>();
}

[Serializable]
public class KeySaveData
{
    public List<string> kName = new List<string>();
    public List<int> kNumber = new List<int>();
}

public class KeySettingManager : MonoBehaviour
{
    public static KeySettingManager instance;

    public KeySaveData kSaveData = new KeySaveData();
    public DefaultKeyData kDefaultData = new DefaultKeyData();
    public PadSaveData pSaveData = new PadSaveData();
    public PadDefaultData pDefaultData = new PadDefaultData();

    public Dictionary<KeyCode, string> padCodeDic =
        new Dictionary<KeyCode, string>()
        {
            {KeyCode.Joystick1Button0, "A" },
            {KeyCode.Joystick1Button1, "B"},
            {KeyCode.Joystick1Button2, "X" },
            {KeyCode.Joystick1Button3, "Y"},
            {KeyCode.Joystick1Button4, "LB"},
            {KeyCode.Joystick1Button5, "RB" }
        };

    public Dictionary<string, string> padTriggerDic =
        new Dictionary<string, string>()
        {
            {"XboxRT", "RT" },
            {"XboxLT", "LT" }
        };

    public List<KeyCode> defaultKeyGroup = new List<KeyCode>();
    public List<KeyCode> changeKeyGroup = new List<KeyCode>();

    public List<KeyCode> defaultPadGroup = new List<KeyCode>();
    public List<bool> defaultRT = new List<bool>();
    public List<bool> defaultLT = new List<bool>();

    public List<KeyCode> changePadGroup = new List<KeyCode>();
    public List<bool> changeRT = new List<bool>();
    public List<bool> changeLT = new List<bool>();

    [Header("프리셋")]
    public KeysettingPreset preset;
    [Header("프리셋 이름")]
    public string keysettingpresetname;
    #region 키보드 입력
    [Header("공격")]
    public KeyCode AttackKeycode=KeyCode.X;
    [Header("점프")]
    public KeyCode jumpKeycode = KeyCode.C;
    [Header("화면전환")]
    public KeyCode DimensionChangeKeycode = KeyCode.Space;
    [Header("스킬")]
    public KeyCode SkillKeycode = KeyCode.S;
    [Header("내려찍기")]
    public KeyCode DownAttackKeycode = KeyCode.A;

    [Header("상호작용")]
    public KeyCode InteractKeycode = KeyCode.F;
    [Header("변신 풀기")]
    public KeyCode DeformKeycode = KeyCode.Q;
    public TextMeshProUGUI interactText;

    [Header("UI 상호작용")]
    public KeyCode UIactiveKeycode = KeyCode.C;
    public KeyCode UIdeactiveKeycode = KeyCode.Escape;
    #endregion

    #region 게임패드 입력(Xbox)
    [Header("게임패드 입련 관련")]
    public KeyCode JumpPadCode = KeyCode.Joystick1Button0; // A
    public KeyCode DownAttackPadCode = KeyCode.Joystick1Button1; // B
    public KeyCode AttackPadCode = KeyCode.Joystick1Button2; // X
    public KeyCode InteractPadCode = KeyCode.Joystick1Button3; // Y
    public KeyCode SkillPadCode = KeyCode.Joystick1Button5; // RB
    public KeyCode PausePadCode = KeyCode.Joystick1Button7; // start button in xbox
    public KeyCode dimensionPadCode = KeyCode.None;
    [HideInInspector]public bool atkRT, atkLT;
    [HideInInspector] public bool jumpRT, jumpLT;
    [HideInInspector] public bool downAtkRT, downAtkLT;
    [HideInInspector] public bool interactRT, interactLT;
    [HideInInspector] public bool skillRT, skillLT;
    [HideInInspector] public bool dimensionRT, dimensionLT;
    #endregion

    private void Awake()
    {
        instance = this;

        SetDefaultKey();
        SetDefaultPad();
    }

    private void Start()
    {
        CheckKeyData();
        CheckPadData();
    }
    private void Update()
    {
        if (interactText != null)
            interactText.text = InteractKeycode.ToString();
    }

    float tValue;

    string keyDefaultFile = "KeyDefaultData.json";
    string keySaveFile = "KeySaveData.json";

    string padDefaultFile = "PadDefaultData.json";
    string padSaveFile = "PadSaveData.json";

    #region 키보드 설정
    public void SetDefaultKey()
    {
        defaultKeyGroup.Add(KeyCode.X); // attack
        defaultKeyGroup.Add(KeyCode.Z); // jump
        defaultKeyGroup.Add(KeyCode.C); // downattack
        defaultKeyGroup.Add(KeyCode.F); // interact
        defaultKeyGroup.Add(KeyCode.Space); // dimensionchange

        if (kDefaultData.kName.Count != 0)
        {
            kDefaultData.kName.Clear();
            kDefaultData.kNumber.Clear();
        }

        for (int i = 0; i < defaultKeyGroup.Count; i++)
        {
            kDefaultData.kName.Add(defaultKeyGroup[i].ToString());
            kDefaultData.kNumber.Add((int)defaultKeyGroup[i]);
        }

        string jsonData = JsonUtility.ToJson(kDefaultData);
        string savePath = Path.Combine(Application.persistentDataPath, keyDefaultFile);
        File.WriteAllText(savePath, jsonData);
    }

    public void CheckKeyData()
    {
        string savePath = Path.Combine(Application.persistentDataPath, keySaveFile);
        string defaultPath = Path.Combine(Application.persistentDataPath, keyDefaultFile);

        if (File.Exists(savePath))
        {
            string saveFile = File.ReadAllText(savePath);
            kSaveData = JsonUtility.FromJson<KeySaveData>(saveFile);
            InitSaveKey(kSaveData);
        }
        else if(File.Exists(defaultPath))
        {
            string defaultFile = File.ReadAllText(defaultPath);
            kDefaultData = JsonUtility.FromJson<DefaultKeyData>(defaultFile);
            InitDefaultKey(kDefaultData);
        }
    }

    public void InitDefaultKey(DefaultKeyData keydata)
    {
        AttackKeycode = (KeyCode)keydata.kNumber[0];
        jumpKeycode = (KeyCode)keydata.kNumber[1];
        DownAttackKeycode = (KeyCode)keydata.kNumber[2];
        InteractKeycode = (KeyCode)keydata.kNumber[3];
        DimensionChangeKeycode = (KeyCode)keydata.kNumber[4];
    }

    public void InitSaveKey(KeySaveData keydata)
    {
        AttackKeycode = (KeyCode)keydata.kNumber[0];
        jumpKeycode = (KeyCode)keydata.kNumber[1];
        DownAttackKeycode = (KeyCode)keydata.kNumber[2];
        InteractKeycode = (KeyCode)keydata.kNumber[3];
        DimensionChangeKeycode = (KeyCode)keydata.kNumber[4];
    }

    public void SaveKeyData()
    {
        if (kSaveData.kName.Count != 0)
        {
            kSaveData.kName.Clear();
            kSaveData.kNumber.Clear();
        }

        ChangeKeySetting();

        for (int i = 0; i < changeKeyGroup.Count; i++)
        {
            kSaveData.kName.Add(changeKeyGroup[i].ToString());
            kSaveData.kNumber.Add((int)changeKeyGroup[i]);
        }

        string jsonData = JsonUtility.ToJson(kSaveData);
        string savePath = Path.Combine(Application.persistentDataPath, keySaveFile);

        File.WriteAllText(savePath, jsonData);
    }

    public void ChangeKeySetting()
    {
        if (changeKeyGroup.Count != 0)
            changeKeyGroup.Clear();

        changeKeyGroup.Add(AttackKeycode);
        changeKeyGroup.Add(jumpKeycode);
        changeKeyGroup.Add(DownAttackKeycode);
        changeKeyGroup.Add(InteractKeycode);
        changeKeyGroup.Add(DimensionChangeKeycode);
    }
    #endregion

    #region 패드 설정
    public void SetDefaultPad()
    {
        defaultPadGroup.Add(KeyCode.Joystick1Button2); // attack
        defaultPadGroup.Add(KeyCode.Joystick1Button0); // jump
        defaultPadGroup.Add(KeyCode.Joystick1Button1); // downattack
        defaultPadGroup.Add(KeyCode.Joystick1Button3); // interact
        defaultPadGroup.Add(KeyCode.Joystick1Button4); // dimensionchange

        if (pDefaultData.pName.Count != 0)
        {
            pDefaultData.pName.Clear();
            pDefaultData.pNumber.Clear();
        }

        dimensionRT = true;

        defaultRT.Add(atkRT); defaultLT.Add(atkLT);
        defaultRT.Add(jumpRT); defaultLT.Add(jumpLT);
        defaultRT.Add(downAtkRT); defaultLT.Add(downAtkLT);
        defaultRT.Add(interactRT); defaultLT.Add(interactLT);
        defaultRT.Add(dimensionRT); defaultLT.Add(dimensionLT);

        for (int i = 0; i < defaultPadGroup.Count; i++)
        {
            pDefaultData.pName.Add(defaultPadGroup[i].ToString());
            pDefaultData.pNumber.Add((int)defaultPadGroup[i]);
            pDefaultData.RT.Add(defaultRT[i]);
            pDefaultData.LT.Add(defaultLT[i]);
        }

        string jsonData = JsonUtility.ToJson(pDefaultData);
        string savePath = Path.Combine(Application.persistentDataPath, padDefaultFile);

        File.WriteAllText(savePath, jsonData);
    }

    public void CheckPadData()
    {
        string savePath = Path.Combine(Application.persistentDataPath, padSaveFile);
        string defaultPath = Path.Combine(Application.persistentDataPath, padDefaultFile);
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            pSaveData = JsonUtility.FromJson<PadSaveData>(jsonData);

            InitSavePad(pSaveData);
        }
        else
        {
            string jsonData = File.ReadAllText(defaultPath);
            pDefaultData = JsonUtility.FromJson<PadDefaultData>(jsonData);

            InitDefaultPad(pDefaultData);
        }
    }

    public void InitDefaultPad(PadDefaultData padData)
    {
        AttackPadCode = (KeyCode)padData.pNumber[0];
        JumpPadCode = (KeyCode)padData.pNumber[1];
        DownAttackPadCode = (KeyCode)padData.pNumber[2];
        InteractPadCode = (KeyCode)padData.pNumber[3];
        dimensionPadCode = (KeyCode)padData.pNumber[4];

        atkRT = padData.RT[0]; atkLT = padData.LT[0];
        jumpRT = padData.RT[1]; jumpLT = padData.LT[1];
        downAtkRT = padData.RT[2]; downAtkLT = padData.LT[2];
        interactRT = padData.RT[3]; interactLT = padData.LT[3];
        dimensionRT = padData.RT[4]; dimensionLT = padData.LT[4];

    }

    public void InitSavePad(PadSaveData padData)
    {
        AttackPadCode = (KeyCode)padData.pNumber[0];
        JumpPadCode = (KeyCode)padData.pNumber[1];
        DownAttackPadCode = (KeyCode)padData.pNumber[2];
        InteractPadCode = (KeyCode)padData.pNumber[3];
        dimensionPadCode = (KeyCode)padData.pNumber[4];

        atkRT = padData.RT[0]; atkLT = padData.LT[0];
        jumpRT = padData.RT[1]; jumpLT = padData.LT[1];
        downAtkRT = padData.RT[2]; downAtkLT = padData.LT[2];
        interactRT = padData.RT[3]; interactLT = padData.LT[3];
        dimensionRT = padData.RT[4]; dimensionLT = padData.LT[4];
    }

    public void SavePadData()
    {
        if (pSaveData.pName.Count != 0)
        {
            pSaveData.pName.Clear();
            pSaveData.pNumber.Clear();
            pSaveData.RT.Clear();
            pSaveData.LT.Clear();
        }

        if (changePadGroup.Count != 0)
        {
            changePadGroup.Clear();
        }
        else
        {
            Debug.Log("변경할 패드 그룹의 카운트 초과입니다");
            return;
        }

        changePadGroup.Add(AttackPadCode);
        changePadGroup.Add(JumpPadCode);
        changePadGroup.Add(DownAttackPadCode);
        changePadGroup.Add(InteractPadCode);
        changePadGroup.Add(dimensionPadCode);

        for (int i = 0; i < changePadGroup.Count; i++)
        {
            pSaveData.pName.Add(changePadGroup[i].ToString());
            pSaveData.pNumber.Add((int)changePadGroup[i]);
            pSaveData.RT.Add(changeRT[i]);
            pSaveData.LT.Add(changeLT[i]);
        }

        string jsonData = JsonUtility.ToJson(pSaveData);
        string savePath = Path.Combine(Application.persistentDataPath, jsonData);

        File.WriteAllText(savePath, jsonData);
    }
    #endregion

    #region 패드입력
    public bool AttackPad()
    {
        bool check = false;
        if (atkRT)
        {
            tValue = Input.GetAxisRaw("XboxRT");
            if (tValue == 1)
            {
                check = true;
            }
        }
        else if(atkLT)
        {
            tValue = Input.GetAxisRaw("XboxLT");
            if (tValue == 1)
            {
                check = true;
            }
        }
        else
        {
            if (Input.GetKey(dimensionPadCode))
            {
                check = true;
            }
        }

        return check;
    }

    public bool JumpPad()
    {
        bool check = false;

        if (jumpRT)
        {
            tValue = Input.GetAxisRaw("XboxRT");
            if (tValue == 1)
            {
                check = true;
            }

        }
        else if(jumpLT)
        {
            tValue = Input.GetAxisRaw("XboxLT");
            if (tValue == 1)
            {
                check = true;
            }
        }
        else
        {
            if (Input.GetKey(JumpPadCode))
            {
                check = true;
            }
        }

        return check;
    }

    public bool DownAttackPad()
    {
        bool check = false;

        if (downAtkRT)
        {
            tValue = Input.GetAxisRaw("XboxRT");
            if (tValue == 1)
                check = true;
        }
        else if(downAtkLT)
        {
            tValue = Input.GetAxisRaw("XboxLT");
            if (tValue == 1)
                check = true;
        }
        else
        {
            if (Input.GetKey(DownAttackPadCode))
            {
                check = true;
            }
        }

        return check;
    }

    public bool SkillPad()
    {
        bool check = false;

        if (skillRT)
        {
            tValue = Input.GetAxisRaw("XboxRT");
            if(tValue ==1)
            check = true;
        }
        else if(skillLT)
        {
            tValue = Input.GetAxisRaw("XboxLT");
            if(tValue ==1)
            check = true;
        }
        else
        {
            if (Input.GetKey(SkillPadCode))
            {
                check = true;
            }
        }

        return check;
    }

    public bool DimensionPad()
    {
        bool check = false;

        if (dimensionRT)
        {
            tValue = Input.GetAxisRaw("XboxRT");
            if (tValue == 1)
            {
                check = true;
            }
        }
        else if (dimensionLT)
        {
            tValue = Input.GetAxisRaw("XboxLT");
            if (tValue == 1)
                check = true;
        }
        else
        {
            if (Input.GetKey(dimensionPadCode))
            {
                check = true;
            }
        }

        return check;
    }

    public bool InteractPad()
    {
        bool check = false;

        if (interactRT)
        {
            tValue = Input.GetAxisRaw("XboxRT");
            if (tValue == 1)
                check = true;
        }
        else if (interactLT)
        {
            tValue = Input.GetAxisRaw("XboxLT");
            if (tValue == 1)
                check = true;
        }
        else
        {
            if (Input.GetKey(InteractPadCode))
                check = true;
        }

        return check;
    }
    #endregion

    public string SetPadName(KeyCode padcode)
    {
        string t = "none";
        if (padCodeDic.TryGetValue(padcode, out string padName))
        {
            t = padName;
        }

        return t;
    }
}
