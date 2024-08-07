using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevelopmentManager : MonoBehaviour
{
    [Header("세이브 기능을 사용 안함")]
    public bool dontsave;
    [Header("생성할 프리팹 여기에(종류 안 따짐)")]
    public List<GameObject> Object=new List<GameObject>();

    public static DevelopmentManager instance;

    //이펙트 부분도 만들자?

    public TextMeshProUGUI CurrentObjectName;

    public TextMeshProUGUI NextObjectName;

    public Essentialitem AttackItem;

    public Essentialitem DoublejumpItem;


    public TextMeshProUGUI NextTransformName;

    public TextMeshProUGUI invicibleText;

    bool Oninvicible;

    public Transform ObjectSpawnPoint;

     int index;

   GameObject CreateObject;

    bool isnormal;

    public GameObject NormalCam;
    public GameObject SwitchingCam;

    private void Awake()
    {
        instance = this;
        NormalCam.SetActive(false);
        SwitchingCam.SetActive(true);
        index = 0;

       
    }
    private void Start()
    {
        CurrentObjectName.text ="현재 오브젝트:"+ Object[index].name;
        if(Object.Count>1)
        NextObjectName.text = "다음 오브젝트:" + Object[index + 1].name;
        else
            NextObjectName.text = "다음 오브젝트: 없음" ;
        invicibleText.text = "무적 상태 활성화";
        NextTransformName.text = "변신 전환" + getNextTransformName(PlayerHandler.instance.CurrentType);
    }
    public string getNextTransformName(TransformType t)
    {
        t++;
        if (t > TransformType.remoteform)
           t= TransformType.Default;

        switch (t)
        {
            case TransformType.Default:
                return "배터리";
            case TransformType.remoteform:
                return "리모컨";
            default:
                return "없음";
        }
    }
    public void AddAttackItem()
    {
        PlayerInventory.instance.ADDEssentialItem(AttackItem);
    }
    public void AddDoubleJumpItem()
    {
        PlayerInventory.instance.ADDEssentialItem(DoublejumpItem);
    }
    public void PlayerRespawn()
    {
        Destroy(
        PlayerHandler.instance.CurrentPlayer.gameObject);
        PlayerSpawnManager.Instance.Spawn();
    }
    public void ChangeTransform()
    {
       
        TransformType t= PlayerHandler.instance.CurrentType++;
        if (t >= TransformType.mouseform)
        {
            t = TransformType.Default;
        }
        PlayerHandler.instance.transformed(t);
        NextTransformName.text = "변신 전환" + getNextTransformName(PlayerHandler.instance.CurrentType);
    }
    public void DestroyObject()
    {
        if (CreateObject != null)
        {
            Destroy(CreateObject);
            CreateObject = null;
        }
    }
    public void AddHP()
    {
        if (PlayerStat.instance != null)
        {
            PlayerStat.instance.RecoverHP(999);
   
        }
    }
    public void inivicibletoggle()
    {
        Oninvicible = !Oninvicible;
        if (!Oninvicible)
        {
            PlayerHandler.instance.CurrentPlayer.onInvincible = false;
            invicibleText.text = "무적 상태 활성화";
        }else
            invicibleText.text = "무적 상태 비활성화";

       
    }
    private void LateUpdate()
    {
        if (Oninvicible)
        {
            PlayerHandler.instance.CurrentPlayer.onInvincible = true;
        }
    }
    public void Spawn()
    {
        if (Object.Count == 0)
            return;
        DestroyObject();
        CreateObject = Instantiate(Object[index], ObjectSpawnPoint.transform.position, ObjectSpawnPoint.transform.rotation);
       
        CreateObject.name = "생성된 게임 오브젝트";
    }
    public void Addindex()
    {

        if (Object.Count == 0)
            return;
        DestroyObject();
        index++;
        if (index >= Object.Count)
            index = 0;
        CurrentObjectName.text = "현재 오브젝트:" + Object[index].name;
        if (Object.Count > 1)
            NextObjectName.text = "다음 오브젝트:" + Object[index + 1].name;
        else
            NextObjectName.text = "다음 오브젝트: 없음";
    }
    public void Changecam()
    {
        isnormal = !isnormal;

        if (isnormal)
        {
            NormalCam.SetActive(true);
            SwitchingCam.SetActive(false);
        }
        else
        {
            NormalCam.SetActive(false);
            SwitchingCam.SetActive(true);
        }
    }

}
