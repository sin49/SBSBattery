using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum KeyType { move, attack, downattack, jump, dimension, interact}

public class KeyMatching : MonoBehaviour
{
    public List<TextMeshProUGUI> fontList = new List<TextMeshProUGUI>();
    public KeyType setKey;
    

    private void OnEnable()
    {
        if (KeySettingManager.instance != null)
        {
            switch (setKey)
            {
                case KeyType.move:
                    Move();
                    break;
                case KeyType.attack:
                    Attack();
                    break;
                case KeyType.downattack:
                    DownAttack();
                    break;
                case KeyType.jump:
                    Jump();
                    break;
                case KeyType.dimension:
                    Dimension();
                    break;
                case KeyType.interact:
                    Interact();
                    break;
                default:
                    break;
            }
        }
    }

    string s = "Arrow";

    public void Attack()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            fontList[0].text = KeySettingManager.instance.AttackKeycode.ToString();
            KeyCheck();
        }
        else
        {
            if (LanguageManager.instance.isKor)
                fontList[0].text = "공격";
            else
                fontList[0].text = "Attack";
        }
    }

    public void Jump()
    {
        if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            fontList[0].text = KeySettingManager.instance.jumpKeycode.ToString();
            KeyCheck();
        }
        else
        {
            if (LanguageManager.instance.isKor)
                fontList[0].text = "점프";
            else
                fontList[0].text = "Jump";
        }
    }

    public void DownAttack()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            fontList[0].text = KeySettingManager.instance.jumpKeycode.ToString();
            fontList[1].text = KeySettingManager.instance.DownAttackKeycode.ToString();
            KeyCheck();
        }
        else
        {
            if (LanguageManager.instance.isKor)
            {
                fontList[0].text = "점프";
                fontList[1].text = "내려찍기";
            }
            else
            {
                fontList[0].text = "Jump";
                fontList[1].text = "Down";
            }
        }
    }

    public void Move()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            fontList[0].text = KeySettingManager.instance.upKeycode.ToString();
            fontList[1].text = KeySettingManager.instance.downKeycode.ToString();
            fontList[2].text = KeySettingManager.instance.leftKeycode.ToString();
            fontList[3].text = KeySettingManager.instance.rightKeycode.ToString();
            KeyCheck();
        }
        else
        {
            for (int i = 0; i < fontList.Count; i++)
            {
                fontList[i].gameObject.SetActive(false);
            }
        }

    }

    public void Dimension()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            fontList[0].text = KeySettingManager.instance.DimensionChangeKeycode.ToString();
            KeyCheck();
        }
        else
        {
            if (LanguageManager.instance.isKor)
                fontList[0].text = "시점전환";
            else
                fontList[0].text = "View";
        }
    }

    public void Interact()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            fontList[0].text = KeySettingManager.instance.InteractKeycode.ToString();
            KeyCheck();
        }
        else
        {
            if (LanguageManager.instance.isKor)
                fontList[0].text = "상호작용";
            else
                fontList[0].text = "Interact";
        }
    }

    public void KeyCheck()
    {
        for (int i = 0; i < fontList.Count; i++)
        {
            if (fontList[i].text == "none")
            {
                fontList[i].text = "";
            }
            else if (fontList[i].text.Contains(s))
            {
                fontList[i].text = fontList[i].text.Substring(0, fontList[i].text.Length - s.Length);
            }
        }
    }
}
