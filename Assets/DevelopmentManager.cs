using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevelopmentManager : MonoBehaviour
{
    [Header("���̺� ����� ��� ����")]
    public bool dontsave;
    [Header("������ ������ ���⿡(���� �� ����)")]
    public List<GameObject> Object=new List<GameObject>();

    public static DevelopmentManager instance;

    //����Ʈ �κе� ������?

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
        CurrentObjectName.text ="���� ������Ʈ:"+ Object[index].name;
        if(Object.Count>1)
        NextObjectName.text = "���� ������Ʈ:" + Object[index + 1].name;
        else
            NextObjectName.text = "���� ������Ʈ: ����" ;
        invicibleText.text = "���� ���� Ȱ��ȭ";
        NextTransformName.text = "���� ��ȯ" + getNextTransformName(PlayerHandler.instance.CurrentType);
    }
    public string getNextTransformName(TransformType t)
    {
        t++;
        if (t > TransformType.remoteform)
           t= TransformType.Default;

        switch (t)
        {
            case TransformType.Default:
                return "���͸�";
            case TransformType.remoteform:
                return "������";
            default:
                return "����";
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
        NextTransformName.text = "���� ��ȯ" + getNextTransformName(PlayerHandler.instance.CurrentType);
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
            invicibleText.text = "���� ���� Ȱ��ȭ";
        }else
            invicibleText.text = "���� ���� ��Ȱ��ȭ";

       
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
       
        CreateObject.name = "������ ���� ������Ʈ";
    }
    public void Addindex()
    {

        if (Object.Count == 0)
            return;
        DestroyObject();
        index++;
        if (index >= Object.Count)
            index = 0;
        CurrentObjectName.text = "���� ������Ʈ:" + Object[index].name;
        if (Object.Count > 1)
            NextObjectName.text = "���� ������Ʈ:" + Object[index + 1].name;
        else
            NextObjectName.text = "���� ������Ʈ: ����";
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
