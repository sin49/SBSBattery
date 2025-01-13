using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

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
            {KeyCode.JoystickButton0, "A" },
            {KeyCode.JoystickButton1, "B"},
            {KeyCode.JoystickButton2, "X"},
            {KeyCode.JoystickButton3, "Y"},
            {KeyCode.JoystickButton4, "LB"},
            {KeyCode.JoystickButton5, "RB"}
        };

    public Dictionary<string, string> padTriggerDic =
        new Dictionary<string, string>()
        {
            {"XboxRT", "RT" },
            {"XboxLT", "LT" }
        };

    public List<KeyCode> defaultKeyGroup = new List<KeyCode>();
    public List<KeyCode> changeKeyGroup = new List<KeyCode>();

    List<KeyCode> notChangeKeyGroup = new List<KeyCode>();

    public List<KeyCode> defaultPadGroup = new List<KeyCode>();
    public List<bool> defaultRT = new List<bool>();
    public List<bool> defaultLT = new List<bool>();

    public List<KeyCode> changePadGroup = new List<KeyCode>();
    public List<bool> changeRT = new List<bool>();
    public List<bool> changeLT = new List<bool>();

    List<KeyCode> notChangePadGroup = new List<KeyCode>();
    List<bool> notChangeRT = new List<bool>();
    List<bool> notChangeLT = new List<bool>();

    [Header("프리셋")]
    public KeysettingPreset preset;
    [Header("프리셋 이름")]
    public string keysettingpresetname;
    #region 키보드 입력
    [Header("이동키")]
    public KeyCode upKeycode = KeyCode.UpArrow;
    public KeyCode downKeycode = KeyCode.DownArrow;
    public KeyCode rightKeycode = KeyCode.RightArrow;
    public KeyCode leftKeycode = KeyCode.LeftArrow;
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
    public KeyCode JumpPadCode = KeyCode.JoystickButton0; // A
    public KeyCode DownAttackPadCode = KeyCode.JoystickButton1; // B
    public KeyCode AttackPadCode = KeyCode.JoystickButton2; // X
    public KeyCode InteractPadCode = KeyCode.JoystickButton3; // Y
    public KeyCode SkillPadCode = KeyCode.JoystickButton5; // RB
    public KeyCode PausePadCode = KeyCode.JoystickButton7; // start button in xbox
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

    [HideInInspector] public int changeIndex;
    [HideInInspector] public bool sameValue;
    #region 키보드 설정
    public void SetDefaultKey()
    {
        defaultKeyGroup.Add(KeyCode.UpArrow);
        defaultKeyGroup.Add(KeyCode.DownArrow);
        defaultKeyGroup.Add(KeyCode.RightArrow);
        defaultKeyGroup.Add(KeyCode.LeftArrow);
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

        foreach (KeyCode key in defaultKeyGroup)
        {
            kDefaultData.kName.Add(key.ToString());
            kDefaultData.kNumber.Add((int)key);
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
        upKeycode = (KeyCode)keydata.kNumber[0];
        downKeycode = (KeyCode)keydata.kNumber[1];
        rightKeycode = (KeyCode)keydata.kNumber[2];
        leftKeycode = (KeyCode)keydata.kNumber[3];
        AttackKeycode = (KeyCode)keydata.kNumber[4];
        jumpKeycode = (KeyCode)keydata.kNumber[5];
        DownAttackKeycode = (KeyCode)keydata.kNumber[6];
        InteractKeycode = (KeyCode)keydata.kNumber[7];
        DimensionChangeKeycode = (KeyCode)keydata.kNumber[8];

        foreach (int key in keydata.kNumber)
        {
            notChangeKeyGroup.Add((KeyCode)key);
        }
    }

    public void InitSaveKey(KeySaveData keydata)
    {
        upKeycode = (KeyCode)keydata.kNumber[0];
        downKeycode = (KeyCode)keydata.kNumber[1];
        rightKeycode = (KeyCode)keydata.kNumber[2];
        leftKeycode = (KeyCode)keydata.kNumber[3];
        AttackKeycode = (KeyCode)keydata.kNumber[4];
        jumpKeycode = (KeyCode)keydata.kNumber[5];
        DownAttackKeycode = (KeyCode)keydata.kNumber[6];
        InteractKeycode = (KeyCode)keydata.kNumber[7];
        DimensionChangeKeycode = (KeyCode)keydata.kNumber[8];

        foreach (int key in keydata.kNumber)
        {
            notChangeKeyGroup.Add((KeyCode)key);
        }
    }

    public void ChangeKeyData(KeyCode inputKey, int index)
    {
        Debug.Log("SaveKeyData호출");
        if (kSaveData.kName.Count != 0)
        {
            kSaveData.kName.Clear();
            kSaveData.kNumber.Clear();
        }

        ChangeKeySetting(inputKey, index);

        foreach (KeyCode key in changeKeyGroup)
        {
            kSaveData.kName.Add(key.ToString());
            kSaveData.kNumber.Add((int)key);
        }
    }    

    public void ChangeKeySetting(KeyCode keyinput, int index)
    {
        if (changeKeyGroup.Count != 0)
            changeKeyGroup.Clear();

        changeKeyGroup.Add(upKeycode);
        changeKeyGroup.Add(downKeycode);
        changeKeyGroup.Add(rightKeycode);
        changeKeyGroup.Add(leftKeycode);
        changeKeyGroup.Add(AttackKeycode);
        changeKeyGroup.Add(jumpKeycode);
        changeKeyGroup.Add(DownAttackKeycode);
        changeKeyGroup.Add(InteractKeycode);
        changeKeyGroup.Add(DimensionChangeKeycode);

        CheckDuplicateKey(keyinput, index);
    }

    public void SaveKeyData()
    {
        notChangeKeyGroup.Clear();

        foreach (int key in kSaveData.kNumber)
            notChangeKeyGroup.Add((KeyCode)key);


        string jsonData = JsonUtility.ToJson(kSaveData);
        string savePath = Path.Combine(Application.persistentDataPath, keySaveFile);

        File.WriteAllText(savePath, jsonData);
    }

    public void ReturnKeyData()
    {
        upKeycode = notChangeKeyGroup[0];
        downKeycode = notChangeKeyGroup[1];
        rightKeycode = notChangeKeyGroup[2];
        leftKeycode = notChangeKeyGroup[3];
        AttackKeycode = notChangeKeyGroup[4];
        jumpKeycode = notChangeKeyGroup[5];
        DownAttackKeycode = notChangeKeyGroup[6];
        InteractKeycode = notChangeKeyGroup[7];
        DimensionChangeKeycode = notChangeKeyGroup[8];

        if (kSaveData.kNumber.Count != 0)
        {
            kSaveData.kName.Clear();
            kSaveData.kNumber.Clear();
        }

        foreach (KeyCode key in notChangeKeyGroup)
        {
            kSaveData.kName.Add(key.ToString());
            kSaveData.kNumber.Add((int)key);
        }
    }

    [HideInInspector] public KeyCode beforeKey = KeyCode.None;

    public void CheckDuplicateKey(KeyCode keyinput, int index)
    {
        for (int i = 0; i < changeKeyGroup.Count; i++)
        {
            if (i == index) continue;

            if (changeKeyGroup[i] == keyinput)
            {
                changeKeyGroup[i] = KeyCode.None;
                changeIndex = i;
                sameValue = true;
                return;
            }
            else
                sameValue = false;
        }
    }
    #endregion

    #region 패드 설정
    public void SetDefaultPad()
    { 
        defaultPadGroup.Add(KeyCode.JoystickButton2); // attack
        defaultPadGroup.Add(KeyCode.JoystickButton0); // jump
        defaultPadGroup.Add(KeyCode.JoystickButton1); // downattack
        defaultPadGroup.Add(KeyCode.JoystickButton3); // interact
        defaultPadGroup.Add(KeyCode.None); // dimensionchange

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

        for (int i = 0; i < padData.pNumber.Count; i++)
        {
            notChangePadGroup.Add((KeyCode)padData.pNumber[i]);
            notChangeRT.Add(padData.RT[i]);
            notChangeLT.Add(padData.LT[i]);
        }

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

        for (int i = 0; i < padData.pNumber.Count; i++)
        {
            notChangePadGroup.Add((KeyCode)padData.pNumber[i]);
            notChangeRT.Add(padData.RT[i]);
            notChangeLT.Add(padData.LT[i]);

            changePadGroup.Add((KeyCode)padData.pNumber[i]);
            changeRT.Add(padData.RT[i]);
            changeLT.Add(padData.LT[i]);
        }
    }

    public void ChangePadData(KeyCode padcode, int index, bool padTrigger = false, string triggerName="")
    {
        PadSaveDataReset();

        if (changePadGroup.Count != 0)
        {
            changePadGroup.Clear();
            changeRT.Clear();
            changeLT.Clear();
        }        

        changePadGroup.Add(AttackPadCode);
        changePadGroup.Add(JumpPadCode);
        changePadGroup.Add(DownAttackPadCode);
        changePadGroup.Add(InteractPadCode);
        changePadGroup.Add(dimensionPadCode);

        changeRT.Add(atkRT); changeLT.Add(atkLT);
        changeRT.Add(jumpRT); changeLT.Add(jumpLT);
        changeRT.Add(downAtkRT); changeLT.Add(downAtkLT);
        changeRT.Add(interactRT); changeLT.Add(interactLT);
        changeRT.Add(dimensionRT); changeLT.Add(dimensionLT);

        DuplicatePad(padcode, index, padTrigger, triggerName);
    }

    [HideInInspector] public KeyCode beforePadCode = KeyCode.None;
    [HideInInspector] public bool changeTrigger, sameTrigger, saveRT, saveLT;

    public void ReturnPadData()
    {
        AttackPadCode = notChangePadGroup[0];
        JumpPadCode = notChangePadGroup[1];
        DownAttackPadCode = notChangePadGroup[2];
        InteractPadCode = notChangePadGroup[3];
        dimensionPadCode = notChangePadGroup[4];

        atkRT = notChangeRT[0]; atkLT = notChangeLT[0];
        jumpRT = notChangeRT[1]; jumpLT = notChangeLT[1];
        downAtkRT = notChangeRT[2]; downAtkLT = notChangeLT[2];
        interactRT = notChangeRT[3]; interactLT = notChangeLT[3];
        dimensionRT = notChangeRT[4]; dimensionLT = notChangeLT[4];

        changePadGroup.Clear();
        changeRT.Clear();
        changeLT.Clear();

        //Debug.Log($"1returnpad {pSaveData.pName.Count}");

        for (int i = 0; i < notChangePadGroup.Count; i++)
        {
            pSaveData.pName.Add(notChangePadGroup[i].ToString());
            pSaveData.pNumber.Add((int)notChangePadGroup[i]);
            pSaveData.RT.Add(notChangeRT[i]);
            pSaveData.LT.Add(notChangeLT[i]);

            changePadGroup.Add(notChangePadGroup[i]);
            changeRT.Add(notChangeRT[i]);
            changeLT.Add(notChangeLT[i]);
        }

        //Debug.Log($"2returnpad {pSaveData.pName.Count}");
    }

    public void SavePadData()
    {
        Debug.Log("call savepaddata method");
        AttackPadCode = changePadGroup[0];
        JumpPadCode = changePadGroup[1];
        DownAttackPadCode = changePadGroup[2];
        InteractPadCode = changePadGroup[3];
        dimensionPadCode = changePadGroup[4];

        atkRT = changeRT[0]; atkLT = changeLT[0];
        jumpRT = changeRT[1]; jumpLT = changeLT[1];
        downAtkRT = changeRT[2]; downAtkLT = changeLT[2];
        interactRT = changeRT[3]; interactLT = changeLT[3];
        dimensionRT = changeRT[4]; dimensionLT = changeLT[4];

        notChangePadGroup.Clear(); notChangeRT.Clear(); notChangeLT.Clear();
        //PadSaveDataReset();

        //Debug.Log($"1savepad :{pSaveData.pName.Count}");
        for (int i = 0; i < changePadGroup.Count; i++)
        {
            pSaveData.pName.Add(changePadGroup[i].ToString());
            pSaveData.pNumber.Add((int)changePadGroup[i]);
            pSaveData.RT.Add(changeRT[i]);
            pSaveData.LT.Add(changeLT[i]);

            notChangePadGroup.Add(changePadGroup[i]);
            notChangeRT.Add(changeRT[i]);
            notChangeLT.Add(changeLT[i]);
        }
        //Debug.Log($"2savepad: {pSaveData.pName.Count}");

        string jsonData = JsonUtility.ToJson(pSaveData);
        string savePath = Path.Combine(Application.persistentDataPath, padSaveFile);

        File.WriteAllText(savePath, jsonData);
    }

    public void PadSaveDataReset()
    {
        if (pSaveData.pName.Count != 0)
        {
            pSaveData.pName.Clear();
            pSaveData.pNumber.Clear();
            pSaveData.RT.Clear();
            pSaveData.LT.Clear();
        }
    }

    public void DuplicatePad(KeyCode padcode, int index, bool padTrigger = false, string triggerName = "")
    {
        sameTrigger = changeTrigger = false;
        for (int i = 0; i < changePadGroup.Count; i++)
        {
            if (i == index) continue;

            if (!padTrigger)
            {                
                //Debug.Log("트리거 입력아님");
                //Debug.Log($"값 체크 {i}번째 changepad{changePadGroup[i]}, padcode {padcode}");
                if (changePadGroup[i] == padcode)
                {
                    if (!changeRT[i] && !changeLT[i])
                    {
                        changePadGroup[i] = KeyCode.None;
                        changeIndex = i;
                        sameValue = true;
                        break;
                    }
                }
            }
            else if (padTrigger)
            {
                //Debug.Log($"트리거 입력입니다, index {index}");
                changeTrigger = true;
                if (triggerName == "RT")
                {
                    if (changeRT[i])
                    {
                        sameTrigger = true;
                        changeRT[i] = false;
                        changeIndex = i;
                    }
                }
                else if (triggerName == "LT")
                {
                    if (changeLT[i])
                    {
                        sameTrigger = true;
                        changeLT[i] = false;
                        changeIndex = i;
                    }
                }
            }
        }
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
            if (Input.GetKey(AttackPadCode))
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

    public bool CheckKeyInput()
    {
        if (Input.GetKey(AttackKeycode) || Input.GetKey(jumpKeycode) || Input.GetKey(InteractKeycode)
            || Input.GetKey(DimensionChangeKeycode) || Input.GetKey(DownAttackKeycode) || Input.GetKey(downKeycode)
            || Input.GetKey(upKeycode) || Input.GetKey(rightKeycode) || Input.GetKey(leftKeycode))
        {
            return true;
        }
        else
            return false;

    }
}
