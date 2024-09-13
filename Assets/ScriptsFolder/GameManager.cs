using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    //1,�κ��丮�� ���� �޴����� ���� �ű��
    //2.�ε��Ҷ����� �ҷ�����
    //���̺� �����ϱ�
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
        if(LoadingEffect!=null)
        LoadingEffect.gameObject.SetActive(false);
        // currentscenename�� �ε� ���� �����մϴ�.
        currentscenename = SceneManager.GetActiveScene().name;
    }

    public string loadingscenename = "LoadingTest";
    public string currentscenename;


    public bool TimerTest=true;
    public float GameTimer;
    public TextMeshProUGUI TimerText;
    public bool pauseActive;
    string GetTimerText(float f)
    {
        int hour = (int)f / 3600;
        int min= ((int)f%3600) / 60;
        int sec = (int)f % 60;
        return $"{hour:00}:{min:00}:{sec:00}";
    }
    private void LateUpdate()
    {
        if (TimerTest)
        {
            if (SceneManager.GetActiveScene().name == "TitleTest" || pauseActive || SceneManager.GetActiveScene().name == name)
            {
                TimerText.gameObject.SetActive(false);
            }
            else
            {
                TimerText.gameObject.SetActive(true);
                GameTimer += Time.unscaledDeltaTime;
                TimerText.text ="Time: "+ GetTimerText(GameTimer);
            }
        }
    
    }

    public void DeleteSaveSetting()
    {
        PlayerPrefs.DeleteAll();
        DeleteInventoryData();
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
        if (PlayerStat.instance != null && PlayerHandler.instance != null)
        {
            PlayerPrefs.SetFloat("PlayerHp", PlayerStat.instance.hp);
            PlayerPrefs.SetInt("TransformType", (int)PlayerHandler.instance.CurrentType);
        }
    }
   
    public float LoadPlayerHP() { if (PlayerPrefs.HasKey("PlayerHP")) return PlayerPrefs.GetFloat("PlayerHP"); else return 3; }
    public int LOadPlayerTransformtype() { if (PlayerPrefs.HasKey("TransformType")) return PlayerPrefs.GetInt("TransformType"); else return 0; }
    public void saveCheckPointIndexKey(int index)
    {
        PlayerPrefs.SetInt("CheckPointIndex", index);
    }
    public void SaveCurrentStage(string stage)
    {
        PlayerPrefs.SetString("LastestStageName", stage);
    }
    public int LoadCheckPointIndexKey()
    {
        if (PlayerPrefs.HasKey("CheckPointIndex"))
            return PlayerPrefs.GetInt("CheckPointIndex");
        else
            return 0;//üũ����Ʈ 0���� �ҷ��´�
    }
    public string LoadLastestStage()
    {
        if(PlayerPrefs.HasKey("LastestStageName"))
        return PlayerPrefs.GetString("LastestStageName");
        else
        return null;//ù��° ���������� �ҷ��´�
    }
    public void ReLoadingScene()
    {
        currentscenename = LoadLastestStage();
        
        StartCoroutine(RELoadingTest());
    }
    public void LoadingScene(string scenename)
    {
      

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
        yield return new WaitForSeconds(LoadingEffect.EffectTime );
        //PlayerHandler.instance.CantHandle = false;

    }
    public void LoadingEffectDeActive()
    {
        LoadingEffect.LoadingComplete = true;
    }
    public void LoadingSceneWithKariEffect(string scenename)
    {
        if (PlayerHandler.instance != null)
        {
            PlayerHandler.instance.CurrentPlayer = null;
            SavePlayerStatus();
        }
        if (        PlayerInventory.instance != null)
             PlayerInventory.instance.SaveInventoryData();
   
            LoadingEffect.EffectEnd += LoadingScene;
        LoadingEffect.LoadSceneName = scenename;
        LoadingEffect.gameObject.SetActive(true);

    }
    public IEnumerator LoadingTest(string scenename)
    {

        AsyncOperation loadingSceneOperation = SceneManager.LoadSceneAsync(loadingscenename);
        loadingSceneOperation.allowSceneActivation = true;

      

        AsyncOperation syncoperation = SceneManager.LoadSceneAsync(scenename);

        Debug.Log(LoadLastestStage() + scenename);
        if (GameManager.instance.LoadLastestStage() != scenename&& scenename != "TitleTest")
        {
            Debug.Log("�� ��ȭ�� ������(�� �����̴ϱ� üũ����Ʈ �ε����� 0���� ���� �ʱ�ȭ)\n ���࿡ �պ����� ����� ������ PD���� ����");
            GameManager.instance.saveCheckPointIndexKey(0);
        }
        if (scenename != "TitleTest")
            SaveCurrentStage(scenename);
        syncoperation.allowSceneActivation = false;

        Debug.Log($"�ε� �� ����(�ּ� {MinimumLoadingTime}�� �Ҹ�....)");
        yield return new WaitForSeconds(MinimumLoadingTime); // �ε� ���� ���� �ð� (�ʿ信 ���� ����)
  
     
        syncoperation.allowSceneActivation = true;
        LoadingEffect.LoadingComplete = true;
        //if(SceneManager.GetActiveScene().name== LoadLastestStage())�ε� ������ �ݹ� ������ ���߿� üũ�ϱ�
        // ���� ������ �´� üũ����Ʈ ��ġ�� �÷��̾ �����մϴ�.
        Debug.Log("�ε� ��");
        Debug.Log("���� ��");

    }
    public float MinimumLoadingTime;
        public IEnumerator RELoadingTest()
    {

        AsyncOperation loadingSceneOperation = SceneManager.LoadSceneAsync(loadingscenename);
        loadingSceneOperation.allowSceneActivation = true;

        //while (!loadingSceneOperation.isDone)
        //{

        //    Debug.Log($"�ε� �� ����: {loadingSceneOperation.progress * 100}%");
        //    yield return null;
        //}

        //Debug.Log("�ε� �� ȣ��");


        AsyncOperation syncoperation = SceneManager.LoadSceneAsync(currentscenename);
        syncoperation.allowSceneActivation = false;

        Debug.Log($"�ε� �� ����(�ּ� {MinimumLoadingTime}�� �Ҹ�....)");
        //while (!syncoperation.isDone)
        //{

        //    Debug.Log($"�ε� �� ����: {syncoperation.progress * 100}%");

        //    yield return null;
        //}
        yield return new WaitForSeconds(MinimumLoadingTime); // �ε� ���� ���� �ð� (�ʿ信 ���� ����)


        syncoperation.allowSceneActivation = true;

        // ���� ������ �´� üũ����Ʈ ��ġ�� �÷��̾ �����մϴ�.
        Debug.Log("�ε� ��");
        Debug.Log("���� ��");
    }
}
// public void ReLoadingScene()
// {

// }
