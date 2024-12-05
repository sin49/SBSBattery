using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public KeyCode PausePadCode = KeyCode.Joystick1Button7; // LB
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
}
