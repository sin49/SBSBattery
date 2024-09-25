using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeySettingManager : MonoBehaviour
{
    public static KeySettingManager instance;

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
