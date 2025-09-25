using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class CharacterUIText : MonoBehaviour
{
    public string s_CharDesc;    

    public float charSpacing;
    public float wordSpacing;
    public float lineSpacing;

    public RectTransform desc;
    public TextMeshProUGUI descTmp;

    public TextMeshProUGUI charInfoTMP;
    public TextMeshProUGUI statInfoTMP;
    public TextMeshProUGUI okTMP;

    public GameObject keyMappingGroup;
    public List<KeyCodeType> KeyMappingList;

    private void Awake()
    {
        ResisterLang();
        RegisterMappingAction();
        InitializeKeyList();
    }

    public void InitializeKeyList()
    {
        KeyMappingList = keyMappingGroup.GetComponentsInChildren<KeyCodeType>().ToList();
        for (int i = 0; i < KeyMappingList.Count; i++)
            KeyMappingList[i].gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ChangeLanguage();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeLanguage();
        CheckConsistencyKeyMapping();
    }

    public void ResisterLang()
    {
        LanguageManager.instance.LanguageEventResister(ChangeLanguage);
    }

    public void ChangeLanguage()
    {
        int index = (int)PlayerHandler.instance.CurrentType;

        if (LanguageManager.instance.isKor)
        {
            //s_CharDesc = LanguageManager.instance.characterKor[index];
            //charSpacing = LanguageManager.instance.charSpacingKor[index];
            //wordSpacing = LanguageManager.instance.wordSpacingKor[index];
            //lineSpacing = LanguageManager.instance.lineSpacing[index];
            //desc.anchoredPosition= LanguageManager.instance.tmpPosKor[index];
            //descTmp.rectTransform.anchoredPosition = LanguageManager.instance.tmpPosKor[index];
            descTmp.text = LanguageManager.instance.characterKor[index];
            descTmp.characterSpacing = LanguageManager.instance.charSpacingKor[index];
            descTmp.wordSpacing = LanguageManager.instance.wordSpacingKor[index];
            descTmp.lineSpacing = LanguageManager.instance.lineSpacing[index];
            desc.anchoredPosition = LanguageManager.instance.tmpPosKor[index];

            charInfoTMP.text = LanguageManager.instance.characterKor[4];
            charInfoTMP.characterSpacing = LanguageManager.instance.charSpacingKor[4];
            charInfoTMP.wordSpacing = LanguageManager.instance.wordSpacingKor[4];
            charInfoTMP.lineSpacing = LanguageManager.instance.lineSpacing[4];
            charInfoTMP.rectTransform.anchoredPosition = LanguageManager.instance.tmpPosKor[4];

            statInfoTMP.text = LanguageManager.instance.characterKor[5];
            statInfoTMP.characterSpacing = LanguageManager.instance.charSpacingKor[5];
            statInfoTMP.wordSpacing = LanguageManager.instance.wordSpacingKor[5];
            statInfoTMP.lineSpacing = LanguageManager.instance.lineSpacing[5];
            statInfoTMP.rectTransform.anchoredPosition = LanguageManager.instance.tmpPosKor[5];

            okTMP.text = LanguageManager.instance.characterKor[6];
            okTMP.characterSpacing = LanguageManager.instance.charSpacingKor[6];
            okTMP.wordSpacing = LanguageManager.instance.wordSpacingKor[6];
            okTMP.lineSpacing = LanguageManager.instance.lineSpacing[6];
            okTMP.rectTransform.anchoredPosition = LanguageManager.instance.tmpPosKor[6];
        }
        else
        {
            descTmp.text = LanguageManager.instance.characterEng[index];
            descTmp.characterSpacing = LanguageManager.instance.charSpacingEng[index];
            descTmp.wordSpacing = LanguageManager.instance.wordSpacingEng[index];
            descTmp.lineSpacing = LanguageManager.instance.lineSpacing[index];
            desc.anchoredPosition = LanguageManager.instance.tmpPosEng[index];

            charInfoTMP.text = LanguageManager.instance.characterEng[4];
            charInfoTMP.characterSpacing = LanguageManager.instance.charSpacingEng[4];
            charInfoTMP.wordSpacing = LanguageManager.instance.wordSpacingEng[4];
            charInfoTMP.lineSpacing = LanguageManager.instance.lineSpacing[4];
            charInfoTMP.rectTransform.anchoredPosition = LanguageManager.instance.tmpPosEng[4];

            statInfoTMP.text = LanguageManager.instance.characterEng[5];
            statInfoTMP.characterSpacing = LanguageManager.instance.charSpacingEng[5];
            statInfoTMP.wordSpacing = LanguageManager.instance.wordSpacingEng[5];
            statInfoTMP.lineSpacing = LanguageManager.instance.lineSpacing[5];
            statInfoTMP.rectTransform.anchoredPosition = LanguageManager.instance.tmpPosEng[5];

            okTMP.text = LanguageManager.instance.characterEng[6];
            okTMP.characterSpacing = LanguageManager.instance.charSpacingEng[6];
            okTMP.wordSpacing = LanguageManager.instance.wordSpacingEng[6];
            okTMP.lineSpacing = LanguageManager.instance.lineSpacing[6];
            okTMP.rectTransform.anchoredPosition = LanguageManager.instance.tmpPosEng[6];
        }
    }

    public void CheckConsistencyKeyMapping()
    {
        for (int i = 0; i < KeyMappingList.Count; i++)
        {
            KeyMappingList[i].gameObject.SetActive(false);
        }

        ApplyKeyCode(KeySettingManager.instance.AttackKeycode);
        ApplyKeyCode(KeySettingManager.instance.DownAttackKeycode);
        ApplyKeyCode(KeySettingManager.instance.InteractKeycode);
        ApplyKeyCode(KeySettingManager.instance.upKeycode);
        ApplyKeyCode(KeySettingManager.instance.downKeycode);
        ApplyKeyCode(KeySettingManager.instance.rightKeycode);
        ApplyKeyCode(KeySettingManager.instance.leftKeycode);
        ApplyKeyCode(KeySettingManager.instance.DimensionChangeKeycode);
    }

    public void ApplyKeyCode(KeyCode Key)
    {
        for (int i = 0; i < KeyMappingList.Count; i++)
        {
            if (Key == KeyMappingList[i].KeyData)
            {
                KeyMappingList[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void RegisterMappingAction()
    {
        KeySettingManager.instance.RegisterMappingAction(CheckConsistencyKeyMapping);
    }
}