using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
    int checkIndex;

    [Header("현재 튜토리얼")] public string currentTutorial;

    int talkIndex;
    bool interact, end;

    // Start is called before the first frame update
    //void Start()
    //{
    //    talkUI.SetActive(false);        
    //}

    private void Awake()
    {
        TutorialReadCSV();
    }

    public void TutorialReadCSV()
    {
        checkIndex = startindex;

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
                Debug.Log(value.Length);
                int index;
                string icon, text, middle;
                if (!string.IsNullOrEmpty(value[0]))
                {
                    index = int.Parse(value[0]);
                    text = value[1];
                    icon = value[2];
                    middle = value[3];
                    if (checkIndex == index && checkIndex <= endindex)
                    {
                        iconString.Add(icon);
                        talkTexts.Add(text);
                        middleText.Add(middle);
                    }
                }
                checkIndex++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interact)
        {
            if (Input.GetKeyDown(KeyCode.X))
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
                    if(TalkUI.instance != null)
                    TalkUI.instance.gameObject.SetActive(false);
                    GetCharacterKey();
                    //CharacterHandler.instance.moveRestric = false;
                    end = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !interact)
        {
            //CharacterHandler.instance.moveRestric = true;
            interact = true;
            InitTextUI();
            TalkUI.instance.gameObject.SetActive(true);
        }
    }

    public void InitTextUI()
    {
        checkIndex = startindex;
        CheckImageText();
    }

    public void CheckImageText()
    {
        TalkUI.instance.CharacterImage(iconString[talkIndex]);
        TalkUI.instance.TutorialMiddleImage(middleText[talkIndex]);
        TalkUI.instance.Text(talkTexts[talkIndex]);
    }

    public void GetCharacterKey()
    {
        switch (currentTutorial)
        {
            case "이동":
                PlayerHandler.instance.moveTuto = true;
                break;
            case "점프":
                PlayerHandler.instance.jumpTuto = true;
                break;
            case "내려가기":
                PlayerHandler.instance.downTuto = true;
                break;
            case "공격":
                PlayerHandler.instance.attackTuto = true;
                break;
            case "상호작용":
                PlayerHandler.instance.interactTuto = true;
                break;
            case "내려찍기":
                PlayerHandler.instance.downAttackTuto = true;
                break;
            case "시점전환":
                PlayerHandler.instance.dimensionTuto = true;
                break;
            default:
                break;
        }
    }
}
