using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //1,인벤토리를 게임 메니저랑 같이 옮기기
    //2.로딩할때마다 불려오기
    //세이브 정리하기
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (LoadingEffect != null)
            LoadingEffect.gameObject.SetActive(false);
        // currentscenename을 로딩 전에 설정합니다.
        //??뭐지 이코드 이거 정상 작동함?
        currentscenename = SceneManager.GetActiveScene().name;
        //LoadTutorialKey();

        lockDoor = "잠겨있음";
        unlockDoor = "상호작용";

        InitMouseTimer();
        mouseTimeMove = true;
    }

    public string loadingscenename = "LoadingTest";
    public string currentscenename;


    public bool TimerTest = true;
    public float GameTimer;

    public bool pauseActive;

    public bool attackTuto, jumpTuto, moveTuto, downTuto, 
        interactTuto, downAttackTuto, 
        dimensionTuto, itemTuto, transformTuto;
    public bool tutoInteract, tutorialEnd;


    public int LoadCheckpointindex;
    public string LoadCheckpointSceneName;
    public int loadcheckpointTransformType;

    [Header("다리미 돌진 지속시간 표시 UI")]
    public GameObject ironUIobject;
    public Image ironRushIcon;

    [HideInInspector] public string lockDoor;
    [HideInInspector] public string unlockDoor;

    public void GetCheckpointData(int n)
    {
        Debug.Log("GetCheckpointData실행");
        var data = CheckTableManager.instance.ReturnCheckCSVData(n);
        Debug.Log($"scenename 값 {data.scenename} | index 값 {data.index}");
        LoadCheckpointindex = data.index;
        LoadCheckpointSceneName = data.scenename;
        loadcheckpointTransformType = data.PlayerTransformtype;
    }

    private void Update()
    {
        if (PlayerHandler.instance != null)
        {
            if (PlayerHandler.instance.CurrentType == TransformType.ironform)
                ironUIobject.SetActive(true);
            else
                ironUIobject.SetActive(false);
        }
        else
        {
            ironUIobject.SetActive(false);
        }

        CheckMouseActiveTime();
    }

    public void loadscenebycheckpoint(int n)
    {
        Debug.Log("loadscenebycheckpoint실행");
        GetCheckpointData(n);

        LoadingSceneWithKariEffect(LoadCheckpointSceneName);
    }
    public void LoadLastCheckPoint()
    {
        Debug.Log("LoadLastCheckPoint실행");
        GetCheckpointData(PlayerPrefs.GetInt("CheckPointIndex"));
        LoadingSceneWithKariEffect(LoadCheckpointSceneName);
    }

    public void LoadChoiceCheckPoint(int n)
    {
        Debug.Log("선택한 체크포인트로 이동");
        GetCheckpointData(n);
        LoadingSceneWithKariEffect(LoadCheckpointSceneName);
    }

    public void DeleteTutorialKey()
    {
        if (PlayerPrefs.HasKey("AttackTuto")) PlayerPrefs.DeleteKey("AttackTuto");
        if (PlayerPrefs.HasKey("JumpTuto")) PlayerPrefs.DeleteKey("JumpTuto");
        if (PlayerPrefs.HasKey("MoveTuto")) PlayerPrefs.DeleteKey("MoveTuto");
        if (PlayerPrefs.HasKey("DownTuto")) PlayerPrefs.DeleteKey("DownTuto");
        if (PlayerPrefs.HasKey("InteractTuto")) PlayerPrefs.DeleteKey("InteractTuto");
        if (PlayerPrefs.HasKey("DownAttackTuto")) PlayerPrefs.DeleteKey("DownAttackTuto");
        if (PlayerPrefs.HasKey("DimensionTuto")) PlayerPrefs.DeleteKey("DimensionTuto");
        if (PlayerPrefs.HasKey("TutorialEnd")) PlayerPrefs.DeleteKey("TutorialEnd");
        if (PlayerPrefs.HasKey("ItemTuto")) PlayerPrefs.DeleteKey("ItemTuto");
        if (PlayerPrefs.HasKey("TransformTuto")) PlayerPrefs.DeleteKey("TransformTuto");
    }

    public void ActiveGameOver()
    {
        LoadingEffect.gameover = true;
        LoadingEffect.gameObject.SetActive(true);
        LoadingEffect.GAmeOverPostProcessing();
    }


    public void LoadTutorialKey()
    {
        if(SceneManager.GetActiveScene().name != "CheckTitleTest" && SceneManager.GetActiveScene().name != "Tutorial")
        {
            attackTuto = true; jumpTuto = true; moveTuto = true;
            downTuto = true; interactTuto = true; downAttackTuto = true;
            dimensionTuto = true; tutorialEnd = true;

            return;
        }

        if (PlayerPrefs.HasKey("AttackTuto")) attackTuto = true;
        if (PlayerPrefs.HasKey("JumpTuto")) jumpTuto = true;
        if (PlayerPrefs.HasKey("MoveTuto")) moveTuto = true;
        if (PlayerPrefs.HasKey("DownTuto")) downTuto = true;
        if (PlayerPrefs.HasKey("InteractTuto")) interactTuto = true;
        if (PlayerPrefs.HasKey("DownAttackTuto")) downAttackTuto = true;
        if (PlayerPrefs.HasKey("DimensionTuto")) dimensionTuto = true;
        if (PlayerPrefs.HasKey("TutorialEnd")) tutorialEnd = true;
        if (PlayerPrefs.HasKey("ItemTuto")) itemTuto = true;
        if (PlayerPrefs.HasKey("TransformTuto")) transformTuto = true;
    }

    public void DeleteSaveSetting()
    {

        PlayerPrefs.DeleteKey("PlayerHp");
        PlayerPrefs.DeleteKey("TransformType");
        PlayerPrefs.DeleteKey("CheckPointIndex");
        PlayerPrefs.DeleteKey("LastestStageName");
        DeleteInventoryData();
        DeleteTutorialKey();
        DeleteCheckPointData();
    }
    public void DeleteInventoryData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "InventorySave.json");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

    }
    public void SavePlayerStatus()
    {
        Debug.Log("SavePlayerStatus 호출됨");
        if (PlayerStat.instance != null && PlayerHandler.instance != null)
        {
            PlayerPrefs.SetFloat("PlayerHp", PlayerStat.instance.hp);
            PlayerPrefs.SetInt("TransformType", (int)PlayerHandler.instance.CurrentType);
        }
    }

    public float LoadPlayerHP() { if (PlayerPrefs.HasKey("PlayerHP")) return PlayerPrefs.GetFloat("PlayerHP"); else return PlayerStat.instance.hpMax; }

    public void saveCheckPointIndexKey(int index)
    {
        if (LoadCheckpointindex <= index)
        {
            PlayerPrefs.SetInt("CheckPointIndex", index);
            if (CheckPointManager.instance != null)
                CheckPointManager.instance.SaveCheckPointData(index);
        }
    }

    public void DeleteCheckPointData()
    {
        string path = Path.Combine(Application.persistentDataPath, "CheckPointData.json");
        if (File.Exists(path))
        {
            File.Delete(path);
        }        
    }

    public void LoadingScene(string scenename)
    {
        Debug.Log($"LoadingScene 호출{scenename}");

        StartCoroutine(LoadingTest(scenename));
    }
    public LoadingEffectKari LoadingEffect;



    public void LoadingEffectToAction(Action<string> act)
    {

        StartCoroutine(LoadingEffectActionCorutine(MinimumLoadingTime, act));
    }
    IEnumerator LoadingEffectActionCorutine(float timer, Action<string> act)
    {
        LoadingEffect.EffectEnd += act;
        LoadingEffect.gameObject.SetActive(true);
        //PlayerHandler.instance.CantHandle = true;
        yield return new WaitForSeconds(LoadingEffect.EffectTime + LoadingEffect.IntesityTime);
        yield return new WaitForSeconds(timer);
        LoadingEffect.LoadingComplete = true;
        yield return new WaitForSeconds(LoadingEffect.EffectTime);
        //PlayerHandler.instance.CantHandle = false;

    }
    public void LoadingEffectDeActive()
    {
        LoadingEffect.LoadingComplete = true;
    }
    public void LoadingSceneWithKariEffect(string scenename)
    {
        Debug.Log("로딩 씬으로 이동");
        if (PlayerHandler.instance != null)
        {
            PlayerHandler.instance.CurrentPlayer = null;
            SavePlayerStatus();
        }
        if (PlayerInventory.instance != null)
            PlayerInventory.instance.SaveInventoryData();
        Debug.Log("인벤토리 데이터 저장한 후 실행되는 코드들");
        LoadingEffect.gameover = false;
        LoadingEffect.EffectEnd += LoadingScene;
        LoadingEffect.LoadSceneName = scenename;
        LoadingEffect.gameObject.SetActive(true);
    }
    public IEnumerator LoadingTest(string scenename)
    {

        AsyncOperation loadingSceneOperation = SceneManager.LoadSceneAsync(loadingscenename);
        loadingSceneOperation.allowSceneActivation = true;



        AsyncOperation syncoperation = SceneManager.LoadSceneAsync(scenename);




        syncoperation.allowSceneActivation = false;

        Debug.Log($"로딩 씬 연출(최소 {MinimumLoadingTime}초 소모....)");
        yield return new WaitForSeconds(MinimumLoadingTime); // 로딩 종료 연출 시간 (필요에 따라 조정)


        syncoperation.allowSceneActivation = true;
        LoadingEffect.LoadingComplete = true;
        //if(SceneManager.GetActiveScene().name== LoadLastestStage())로딩 지금은 금방 끝나니 나중에 체크하기
        // 다음 씬에서 맞는 체크포인트 위치에 플레이어를 생성합니다.
        Debug.Log("로딩 끝");
        Debug.Log("연출 끝");

    }
    public float MinimumLoadingTime;

    public string Keycard;

    public void GetKeyCard(string s)
    {
        Keycard = s;
    }

    public FullScreenMode screenMode;

    public void ChangeWindowed()
    {
        Screen.SetResolution(1024, 720, false);
    }

    public void ChangeFullscreen()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    #region 마우스 관련
    float mouseTimer;
    float mouseInitTime = 3.0f;

    float mouseX, mouseY;
    public void InitMouseTimer() // 마우스 타이머 초기화
    {
        mouseTimer = mouseInitTime;
    }
    [HideInInspector]public bool mouseTimeMove = true;
    public void CheckMouseActiveTime()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        if (mouseTimeMove) // 일시정지 UI가 활성화 되지 않았을 때
        {
            if (mouseX == 0 && mouseY == 0)
            {
                if (mouseTimer > 0)
                {
                    mouseTimer -= Time.unscaledDeltaTime;
                }
                else
                {
                    //Debug.Log("마우스 비활성화");
                    Cursor.visible = false;
                }
            }
            else
            {
                //Debug.Log("마우스 움직임");
                Cursor.visible = true;
                InitMouseTimer();
            }
        }
        else
        {
            InitMouseTimer();
        }
    }
    #endregion
}
// public void ReLoadingScene()
// {

// }
