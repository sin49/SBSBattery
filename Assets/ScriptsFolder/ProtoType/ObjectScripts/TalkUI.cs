using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TalkUI : MonoBehaviour
{
    public static TalkUI instance;
    public TextMeshProUGUI talkText;
    public float textSpeed;

    [Header("좌측 이미지")] public Image left;
    [Header("우측 이미지")] public Image right;
    [Header("중간 이미지")] public Image middle;

    [Header("배터리 기본")] public Sprite normalBattery;
    [Header("배터리 빡침")] public Sprite angryBattery;
    [Header("배터리 기분좋음")] public Sprite happyBattery;
    [Header("배터리 의문")] public Sprite curiousBattery;

    [Header("이동 튜토리얼 이미지")] public GameObject moveTutorial;
    [Header("점프 튜토리얼 이미지")] public GameObject jumpTutorial;
    [Header("내려가기 튜토리얼 이미지")] public GameObject downTutorial;
    [Header("공격 튜토리얼 이미지")] public GameObject attackTutorial;
    [Header("내려찍기 튜토리얼 이미지")] public GameObject downAttackTutorial;
    [Header("시점 전환 튜토리얼 이미지")] public GameObject dimensionTutorial;

    [Header("변신 튜토리얼 이미지")] public GameObject formChangeTutorial;
    [Header("특수능력 튜토리얼 이미지")] public GameObject abilityTutorial;

    [Header("상호작용 튜토리얼 이미지")] public GameObject interactTutorial;

    GameObject currentMiddle;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #region 대화 UI
    public void Text(string s)
    {
        Debug.Log("대사 적용");
        talkText.text = s;
    }

    public void CharacterImage(string s)
    {
        Debug.Log("캐릭터 표정 적용");
        switch (s)
        {
            case "기본":
                left.sprite = normalBattery;
                break;
            case "신남":
                left.sprite = happyBattery;
                break;
            case "화남":
                left.sprite = angryBattery;
                break;
            case "의문":
                left.sprite = curiousBattery;
                break;
            default:
                break;
        }
    }

    //public void TutorialMiddleImage(string s)
    //{
    //    Debug.Log("중간 이미지 적용");
    //    switch (s)
    //    {
    //        case "없음":
    //            break;
    //        case "이동":
    //            SetCurrentMiddleImage(moveTutorial);
    //            break;
    //        case "점프":
    //            SetCurrentMiddleImage(jumpTutorial);
    //            break;
    //        case "내려가기":
    //            SetCurrentMiddleImage(downTutorial);
    //            break;
    //        case "공격":
    //            SetCurrentMiddleImage(attackTutorial);
    //            break;
    //        case "상호작용":
    //            SetCurrentMiddleImage(interactTutorial);
    //            break;
    //        case "시점전환":
    //            SetCurrentMiddleImage(dimensionTutorial);
    //            break;
    //        case "숨김":
    //            currentMiddle.SetActive(false);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public void SetCurrentMiddleImage(GameObject obj)
    {
        currentMiddle = obj;
        currentMiddle.SetActive(true);
    }
    #endregion
}
