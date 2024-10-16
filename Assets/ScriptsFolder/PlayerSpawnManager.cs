using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
  public static PlayerSpawnManager Instance;


    public Camera CheckpointChkCamera;
    TransformType DefaultSpawntype;
    [Header("비울 시 작동 안함")]
    public Transform CheckPointTransform;
    public CheckPoint[] Checkpoints = new CheckPoint[0];
    Dictionary<int, CheckPoint> ChkPointsDic = new Dictionary<int, CheckPoint>();
    public GameObject DefaultForm;
    public int LastestCheckPointID;//세이브용 나중에 따로 보내기
    public CheckPoint CurrentCheckPoint;
    public GameObject SaveEffect;
    public GameObject CurrentPlayer;// 행동 작업
    public bool DontSave;
    public bool dontloadTransformInfo = false;
    public bool IgnoreSavedCheckPoint;
    public void ChangeCheckPoint(CheckPoint ChkPoint)
    {
        if (LastestCheckPointID >= ChkPoint.index)
            return;

        LastestCheckPointID = ChkPoint.index;
        CurrentCheckPoint.DeactiveCheckpoint();
        CurrentCheckPoint = ChkPoint;
        CurrentCheckPoint.activecheckpoint();
        Debug.Log("세이브" +ChkPoint.index);
        if (!DontSave)
        {
            SaveEffect.gameObject.SetActive(true);

            GameManager.instance.saveCheckPointIndexKey(ChkPoint.index);
            GameManager.instance.SaveCurrentStage(SceneManager.GetActiveScene().name);
            GameManager.instance.SavePlayerStatus();
            PlayerInventory.instance.SaveInventoryData();
        }
        //Debug.Log($"Playerprefs chkpointindex{GameManager.instance.LoadCheckPointIndexKey()} LastestStage{GameManager.instance.LoadLastestStage()}");

    }
    public CheckPoint GetCurrentCheckpoint()
    {
        if (!DontSave&& GameManager.instance.LoadCheckPointIndexKey()< Checkpoints.Length)
            return Checkpoints[GameManager.instance.LoadCheckPointIndexKey()];
        else
            return Checkpoints[0];
    }
    public CheckPoint LoadCheckPoint()
    {
       if(!DontSave&& GameManager.instance.LoadCheckPointIndexKey()< ChkPointsDic.Count)
        CurrentCheckPoint = ChkPointsDic[GameManager.instance.LoadCheckPointIndexKey()];
        else
            CurrentCheckPoint= ChkPointsDic[0];
        return CurrentCheckPoint;
    }
    //public void Respawn()
    //{

    //    StartCoroutine(ReSpawnPlayer(LastestCheckPointID));
        
    //}
    //IEnumerator ReSpawnPlayer(int n)
    //{
   
    //    yield return StartCoroutine(GameManager.instance.RELoadingTest());

    //    spawnCheckPoint(n);
    //}
    public void Spawn()
    {
        PlayerFormList p;
        Debug.Log("SpawnTest");
       
            if (TryGetComponent<PlayerFormList>(out p))
            {
            if (!dontloadTransformInfo)
            {
                DefaultForm = p.playerformlist[GameManager.instance.LOadPlayerTransformtype()];
            }
            else
            {
                DefaultForm = p.playerformlist[0];
            }
        }
                
        
            var a = CurrentCheckPoint.spawn(DefaultForm);
        CurrentPlayer = a;
        PlayerHandler.instance.registerPlayer(a);
    }
    public void FindCheckpoint(int n)
    {
        if (IgnoreSavedCheckPoint)
        {
            CurrentCheckPoint = ChkPointsDic[0];
        }
        else if (ChkPointsDic.ContainsKey(n))
        {
            CurrentCheckPoint = ChkPointsDic[n];
        }
        else if(ChkPointsDic.Count>0)
        {
            CurrentCheckPoint = ChkPointsDic[0];
            Debug.Log("체크포인트 사이즈 에러");
        }
        else
        {
            Debug.Log("체크포인트 Null 에러");
        }
        CurrentCheckPoint.activecheckpoint();
    }

    public void spawnCheckPoint(int n)
    {
       PlayerSpawnManager.Instance.LastestCheckPointID = n;

    }
 

    private void Awake()
    {
        Instance = this;
        if (CheckPointTransform != null) { 
        Checkpoints = CheckPointTransform.GetComponentsInChildren<CheckPoint>();
         }
        for(int n = 0; n < Checkpoints.Length; n++)
        {
            if (Checkpoints[n] == null)
                continue;
            ChkPointsDic.Add(n, Checkpoints[n]);
            Checkpoints[n].index = n;
        }
      
      
        //PlayerSpawn이 아니라 0번 체크포인트를 찿아서 스폰되도록
        //PlayerSpawn = GameObject.Find("PlayerSpawn").transform;

    }
    private void Start()
    {

        if (DevelopmentManager.instance != null)
            DontSave = DevelopmentManager.instance.dontsave;
        if (!DontSave)
        {
            PlayerInventory.instance.LoadInventoryData();
            PlayerStat.instance.hp = GameManager.instance.LoadPlayerHP();
            
        }else
        {
            PlayerStat.instance.hp = PlayerStat.instance.hpMax;
        }
        if(!dontloadTransformInfo)
        PlayerHandler.instance.CurrentType = (TransformType)GameManager.instance.LOadPlayerTransformtype();
        else
            PlayerHandler.instance.CurrentType = TransformType.Default;
        FindCheckpoint(GameManager.instance.LoadCheckPointIndexKey());
        Spawn();
    }
}
