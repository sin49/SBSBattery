using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
  public static PlayerSpawnManager Instance;


    public Camera CheckpointChkCamera;
    TransformType DefaultSpawntype;
    public CheckPoint[] Checkpoints = new CheckPoint[0];
    Dictionary<int, CheckPoint> ChkPointsDic = new Dictionary<int, CheckPoint>();
    public GameObject DefaultForm;
    public int LastestCheckPointID;//���̺�� ���߿� ���� ������
    public CheckPoint CurrentCheckPoint;
    public GameObject SaveEffect;
    public GameObject CurrentPlayer;// �ൿ �۾�
    public bool DontSave;
    public bool dontloadTransformInfo = false;
    public void ChangeCheckPoint(CheckPoint ChkPoint)
    {
        if (LastestCheckPointID >= ChkPoint.index)
            return;
        LastestCheckPointID = ChkPoint.index;
        CurrentCheckPoint = ChkPoint;
        Debug.Log("���̺�" +ChkPoint.index);
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
    public void LoadCheckPoint()
    {
        CurrentCheckPoint = ChkPointsDic[GameManager.instance.LoadCheckPointIndexKey()];
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
            if (!DontSave)
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
        if (DontSave)
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
            Debug.Log("üũ����Ʈ ������ ����");
        }
        else
        {
            Debug.Log("üũ����Ʈ Null ����");
        }
    }

    public void spawnCheckPoint(int n)
    {
       PlayerSpawnManager.Instance.LastestCheckPointID = n;

    }
 

    private void Awake()
    {
        Instance = this;
        foreach(CheckPoint obj in Checkpoints)
        {
            ChkPointsDic.Add(obj.index, obj);
        }
      
        //PlayerSpawn�� �ƴ϶� 0�� üũ����Ʈ�� �O�Ƽ� �����ǵ���
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
