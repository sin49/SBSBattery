using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;
using UnityEngine.UI;

public class KeySettingManager : MonoBehaviour
{
    public static KeySettingManager instance;
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
    //[Header("스킬")]
    //public KeyCode SkillKeycode = KeyCode.S;
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
    }
    private void Update()
    {
        if (interactText != null)
            interactText.text = InteractKeycode.ToString();
    }

    float tValue;

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
}
