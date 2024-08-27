using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeySettingManager : MonoBehaviour
{
    public static KeySettingManager instance;

    [Header("����")]
    public KeyCode AttackKeycode=KeyCode.X;
    [Header("����")]
    public KeyCode jumpKeycode = KeyCode.C;
    [Header("ȭ����ȯ")]
    public KeyCode DimensionChangeKeycode = KeyCode.Space;
    [Header("��ų")]
    public KeyCode SkillKeycode = KeyCode.S;
    [Header("�������")]
    public KeyCode DownAttackKeycode = KeyCode.A;

    [Header("��ȣ�ۿ�")]
    public KeyCode InteractKeycode = KeyCode.F;
    [Header("���� Ǯ��")]
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
