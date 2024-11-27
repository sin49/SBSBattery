using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [Header("표정(문자열)")] public List<string> iconString = new List<string>();
    [Header("튜토리얼 이미지(문자열)")] public List<string> middleText = new List<string>();
    [Header("튜토리얼 이미지")] public GameObject imageTutorial;

    [Header("현재 튜토리얼")] public string currentTutorial;

    public int checkIndex;
    public int talkIndex;
    public bool interact, end;
    
    // Start is called before the first frame update
    //void Start()
    //{
    //    talkUI.SetActive(false);        
    //}

    private void Awake()
    {
        if(imageTutorial !=null)
        imageTutorial.SetActive(false);
        TutorialReadCSV();
        //SaveCheck();
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Stage1-6 1")
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
                string icon, text, middle;
                if (!string.IsNullOrEmpty(value[0]))
                {
                    index = int.Parse(value[0]); // 번호
                    //Debug.Log($"current{index}");
                    text = value[1]; // 대본
                    icon = value[2]; // 배터리 이미지
                    middle = value[3]; // 튜토리얼 이미지(선택사항)
                    if (checkIndex == index && checkIndex <= endindex)
                    {
                        Debug.Log("인덱스 번호 일치함");
                        iconString.Add(icon);
                        talkTexts.Add(text);
                        middleText.Add(middle);
                        checkIndex++;
                    }                    
                }
                //Debug.Log($"plusCheckIndex{checkIndex}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interact)
        {
            if ((Input.GetKeyDown(KeySettingManager.instance.AttackKeycode) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !end && !textPlaying)
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
                        //CharacterHandler.instance.moveRestric = false;
                        GameManager.instance.tutoInteract = false;
                        PlayerHandler.instance.CurrentPlayer.cantmove = false;
                        end = true;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (SaveCheck()) return;

        if (other.CompareTag("Player") && !interact)
        {
            //if (PlayerHandler.instance != null && PlayerHandler.instance.CurrentType)

            //CharacterHandler.instance.moveRestric = true;
            interact = true;
            //Time.timeScale = 0;
            GameManager.instance.tutoInteract = true;
            PlayerHandler.instance.CurrentPlayer.cantmove = true;
            TalkUI.instance.gameObject.SetActive(true);
            InitTextUI();
        }
    }

    public bool textSkip, textPlaying, textEnd;

    IEnumerator TextAnim()
    {
        textEnd = false;

        TalkUI.instance.talkText.text = "";
        for (int n = 0; n < talkTexts[talkIndex].Length; n++)
        {

            if (textSkip)
            {
                TalkUI.instance.talkText.text = talkTexts[talkIndex];
                textSkip = false;
                textEnd = true;
                break;
            }
            else
            {
                TalkUI.instance.talkText.text += talkTexts[talkIndex][n];
                TalkUI.instance.TextSoundPlay();
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
        //TalkUI.instance.TextSoundPlay();
        StartCoroutine(TextAnim());
    }

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
            default:
                break;
        }
        return textPlaying;
    }
}
