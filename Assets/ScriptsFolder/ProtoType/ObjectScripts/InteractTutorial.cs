using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InteractTutorial : MonoBehaviour
{
    public TextAsset tutorialCSV;
    public int startindex;
    public int endindex;

    //public GameObject talkUI;



    public List<Sprite> batteryIcon = new List<Sprite>();
    [Header("대사(문자열)")] public List<string> talkTexts = new List<string>();
    [Header("대사(영문)")] public List<string> engTexts = new List<string>();
    [Header("표정(문자열)")] public List<string> iconString = new List<string>();
    [Header("튜토리얼 이미지(문자열)")] public List<string> middleText = new List<string>();
    [Header("튜토리얼 이미지")] public GameObject imageTutorial;
    [Header("한글 이름")] public List<string> kName = new List<string>();
    [Header("영문 이름")] public List<string> eName = new List<string>();

    [Header("현재 튜토리얼")] public string currentTutorial;

    public int checkIndex;
    public int talkIndex;
    public bool interact, end;
    
    // Start is called before the first frame update
    //void Start()
    //{
    //    talkUI.SetActive(false);        
    //}

    public void CallByTutoEvent()
    {
        if (SaveCheck())
        {
            Debug.Log("데이터가 레지스트리에 저장 되어있습니다");
            return;
        }

        interact = true;
        GameManager.instance.tutoInteract = true;
        if(PlayerHandler.instance != null)
        PlayerHandler.instance.CurrentPlayer.cantmove = true;
        RegisterAction();
        if(TalkUI.instance !=null)
        TalkUI.instance.gameObject.SetActive(true);
    }

    private void Awake()
    {
        if(imageTutorial !=null)
        imageTutorial.SetActive(false);
        TutorialReadCSV();
        //SaveCheck();
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Stage1-6")
            gameObject.SetActive(false);
    }

    public void TutorialReadCSV()
    {
        checkIndex = startindex;
        //Debug.Log($"initcheckindex{checkIndex}");
        StringReader CSVreader;
        bool firstPass = true;
        if (tutorialCSV != null)
        {
            CSVreader = new StringReader(tutorialCSV.text);

            while (true)
            {
                string read = CSVreader.ReadLine();

                if (read == null || string.IsNullOrEmpty(read)) return;

                if (firstPass)
                {
                    firstPass = false;
                    continue;
                }

                string[] value = read.Split(",");
                //Debug.Log(value.Length);
                int index;
                string icon, text, middle, eng, korName, engName;
                if (!string.IsNullOrEmpty(value[0]))
                {
                    index = int.Parse(value[0]); // 번호
                    //Debug.Log($"current{index}");
                    text = value[1]; // 대본
                    icon = value[2]; // 배터리 이미지
                    middle = value[3]; // 튜토리얼 이미지(선택사항)
                    eng = value[4];
                    korName = value[5];
                    engName = value[6];

                    if (checkIndex == index && checkIndex <= endindex)
                    {
                        Debug.Log("인덱스 번호 일치함");
                        iconString.Add(icon);
                        talkTexts.Add(text);
                        middleText.Add(middle);
                        engTexts.Add(eng);
                        kName.Add(korName);
                        eName.Add(engName);
                        
                        checkIndex++;
                    }                    
                }
                //Debug.Log($"plusCheckIndex{checkIndex}");
            }
        }
    }

    bool InputByPad()
    {
        bool inputPad = false;

        if(Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            inputPad = true;
        }

        return inputPad;
    }

    bool InputByKey()
    {
        bool inputKey = false;

        if (Input.GetKeyDown(KeySettingManager.instance.jumpKeycode) || 
            Input.GetKeyDown(KeyCode.Space) || 
            Input.GetKeyDown(KeyCode.Return))
        {
            inputKey = true;
        }

        return inputKey;
    }

    // Update is called once per frame
    void Update()
    {
        if (interact)
        {
            if (( InputByKey() || InputByPad() || Input.GetMouseButtonDown(0)) && !end && !textPlaying)
            {
                if (!textSkip && !textEnd)
                {
                    Debug.Log("스킵");
                    textSkip = true;
                }

                if (textEnd)
                {
                    talkIndex++;
                    if (talkIndex < talkTexts.Count)
                    {
                        Debug.Log("다음 텍스트");
                        CheckImageText();
                    }
                    else
                    {
                        Debug.Log("상호작용 끝");
                        if (TalkUI.instance != null)
                            TalkUI.instance.gameObject.SetActive(false);
                        GetCharacterKey();
                        GameManager.instance.tutoInteract = false;
                        PlayerHandler.instance.CurrentPlayer.cantmove = false;
                        end = true;
                    }
                }
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //if (SaveCheck()) return;

    //    if (other.CompareTag("Player") && !interact)
    //    {
    //        //if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentType)

    //        //CharacterHandler.instance.moveRestric = true;
    //        interact = true;
    //        GameManager.instance.tutoInteract = true;
    //        PlayerHandler.instance.CurrentPlayer.cantmove = true;
    //        RegisterAction();
    //        TalkUI.instance.gameObject.SetActive(true);
    //    }
    //}

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !interact)
        {           
            interact = true;
            GameManager.instance.tutoInteract = true;
            PlayerHandler.instance.CurrentPlayer.cantmove = true;
            RegisterAction();
            TalkUI.instance.gameObject.SetActive(true);
        }
    }

    public bool textSkip, textPlaying, textEnd;
    
    public void RegisterAction()
    {
        TalkUI.instance.RegisterTextAction(InitTextUI);
    }
    IEnumerator TextAnim()
    {
        textEnd = false;
        string str = "";
        if(LanguageManager.instance.isKor/* && talkIndex >= engTexts.Count*/)
        {
            Debug.Log("한국어가 호출됩니다");
            str = talkTexts[talkIndex];
            TalkUI.instance.cName.text = kName[talkIndex];
        }
        else
        {
            Debug.Log("영어가 호출됩니다");
            str = engTexts[talkIndex];
            TalkUI.instance.cName.text = eName[talkIndex];
        }
        TalkUI.instance.talkText.text = "";
        for (int n = 0; n < str.Length; n++)
        {

            if (textSkip)
            {
                TalkUI.instance.talkText.text = str;
                string replace = TalkUI.instance.talkText.text.Replace("|", "\n");
                replace = replace.Replace("`", ",");
                TalkUI.instance.talkText.text = replace;
                textSkip = false;
                textEnd = true;
                break;
            }
            else
            {
                TalkUI.instance.talkText.text += str[n];
                string replace = TalkUI.instance.talkText.text.Replace("|", "\n");
                replace = replace.Replace("`", ",");
                TalkUI.instance.talkText.text = replace;
                //TalkUI.instance.TextSoundPlay();
                yield return new WaitForSecondsRealtime(TalkUI.instance.textSpeed); ;
            }
        }
        textPlaying = false;
        textEnd = true;
    }

    public void InitTextUI()
    {
        checkIndex = startindex;        
        CheckImageText();
    }

    public void CheckImageText()
    {
        TalkUI.instance.CharacterImage(iconString[talkIndex]);
        CheckMiddleImage();
        //TalkUI.instance.TutorialMiddleImage(middleText[talkIndex]);
        TalkUI.instance.Text(talkTexts[talkIndex]);
        TalkUI.instance.TextSoundPlay();
        StartCoroutine(TextAnim());
    }
    #region 키 검사?
    public void GetCharacterKey()
    {
        switch (currentTutorial)
        {
            case "이동":
                GameManager.instance.moveTuto = true;
                PlayerPrefs.SetInt("MoveTuto", 1);
                break;
            case "점프":
                GameManager.instance.jumpTuto = true;
                PlayerPrefs.SetInt("JumpTuto", 1);
                break;
            case "내려가기":
                GameManager.instance.downTuto = true;
                PlayerPrefs.SetInt("DownTuto", 1);
                break;
            case "공격":
                GameManager.instance.attackTuto = true;
                PlayerPrefs.SetInt("AttackTuto", 1);
                break;
            case "상호작용":
                GameManager.instance.interactTuto = true;
                PlayerPrefs.SetInt("InteractTuto", 1);
                break;
            case "내려찍기":
                GameManager.instance.downAttackTuto = true;
                PlayerPrefs.SetInt("DownAttackTuto", 1);
                break;
            case "시점전환":
                GameManager.instance.dimensionTuto = true;
                PlayerPrefs.SetInt("DimensionTuto", 1);
                break;
            case "아이템":
                GameManager.instance.itemTuto = true;
                PlayerPrefs.SetInt("ItemTuto", 1);
                break;
            case "체크포인트":
                GameManager.instance.tutorialEnd = true;
                PlayerPrefs.SetInt("TutorialEnd", 1);
                break;
            case "변신":
                GameManager.instance.transformTuto = true;
                PlayerPrefs.SetInt("TransformTuto", 1);
                break;
            case "다리미튜토":
                GameManager.instance.ironTuto = true;
                PlayerPrefs.SetInt("IronTuto", 1);
                break;
            case "레이저튜토":
                GameManager.instance.laserTuto = true;
                PlayerPrefs.SetInt("LaserTuto", 1);
                break;
            case "강화상호작용":
                GameManager.instance.reinforcementTuto = true;
                PlayerPrefs.SetInt("Reinforcement", 1);
                break;
            case "카드키":
                GameManager.instance.cardKey = true;
                PlayerPrefs.SetInt("CardKey", 1);
                break;
            case "던지기":
                GameManager.instance.mouseThrow = true;
                PlayerPrefs.SetInt("MouseThrow", 1);
                break;
            case "클리어":
                GameManager.instance.LoadingSceneWithKariEffect("CheckTitleTest");
                break;
            default:
                break;
        }
        if(imageTutorial !=null)
        imageTutorial.SetActive(false);
    }

    public void CheckMiddleImage()
    {
        switch (middleText[talkIndex])
        {
            case "있음":
                imageTutorial.SetActive(true);
                break;
            case "없음":
                break;
            case "숨김":
                imageTutorial.SetActive(true);
                break;
            default:
                break;
        }
    }

    public bool SaveCheck()
    {
        switch (currentTutorial)
        {
            case "이동":
                if (PlayerPrefs.HasKey("MoveTuto"))
                {
                    GameManager.instance.moveTuto = true;
                    textPlaying = true;
                }
                break;
            case "점프":
                if (PlayerPrefs.HasKey("JumpTuto"))
                {
                    GameManager.instance.jumpTuto = true;
                    textPlaying = true;
                }
                break;
            case "내려가기":
                if (PlayerPrefs.HasKey("DownTuto"))
                {
                    GameManager.instance.downTuto = true;
                    textPlaying = true;
                }
                break;
            case "공격":
                if (PlayerPrefs.HasKey("AttackTuto"))
                {
                    GameManager.instance.attackTuto = true;
                    textPlaying = true;
                }
                break;
            case "상호작용":
                if (PlayerPrefs.HasKey("InteractTuto"))
                {
                    GameManager.instance.interactTuto = true;
                    textPlaying = true;
                }
                break;
            case "내려찍기":
                if (PlayerPrefs.HasKey("DownAttackTuto"))
                {
                    GameManager.instance.downAttackTuto = true;
                    textPlaying = true;
                }
                break;
            case "시점전환":
                if (PlayerPrefs.HasKey("DimensionTuto"))
                {
                    GameManager.instance.dimensionTuto = true;
                    textPlaying = true;
                }
                break;
            case "아이템":
                if (PlayerPrefs.HasKey("ItemTuto"))
                {
                    GameManager.instance.itemTuto = true;
                    textPlaying = true;
                }
                break;
            case "체크포인트":
                if (PlayerPrefs.HasKey("TutorialEnd"))
                {
                    GameManager.instance.tutorialEnd = true;
                    textPlaying = true;
                }
                break;
            case "변신":
                if(PlayerPrefs.HasKey("TransformTuto"))
                {
                    GameManager.instance.transformTuto = true;
                    textPlaying = true;
                }
                break;
            case "다리미튜토":
                if(PlayerPrefs.HasKey("IronTuto"))
                {
                    GameManager.instance.ironTuto = true;
                    textPlaying = true;
                }
                break;
            case "레이저튜토":
                if(PlayerPrefs.HasKey("LaserTuto"))
                {
                    GameManager.instance.laserTuto = true;
                    textPlaying = true;
                }
                break;
            case "강화상호작용":
                if(PlayerPrefs.HasKey("Reinforcement"))
                {
                    GameManager.instance.reinforcementTuto = true;
                    textPlaying = true;
                }
                break;
            case "카드키":
                if (PlayerPrefs.HasKey("CardKey"))
                {
                    GameManager.instance.cardKey = true;
                    textPlaying = true;
                }
                break;
            case "던지기":
                if(PlayerPrefs.HasKey("MouseThrow"))
                {
                    GameManager.instance.mouseThrow = true;
                    textPlaying = true;
                }
                break;
            default:
                break;
        }
        return textPlaying;
    }
    #endregion
}
