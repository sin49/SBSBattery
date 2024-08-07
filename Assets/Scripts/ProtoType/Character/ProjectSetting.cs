using UnityEngine;
using UnityEditor;
using System;


public class ProjectSetting :MonoBehaviour
{
    Vector3 GravityValue;
    [Header("�߷��� X�� ����(X�� ���� ���� ����ġ)")]
    public float GravityX;
    [Header("�߷��� Y�� ����(Y�� ���� ���� ����ġ)")]
    public float GravityY;
    [Header("�߷��� Z�� ����(Y�� ���� ���� ����ġ)")]
    public float GravityZ;

    [Header("�÷��̾� ���� ���� ����")]
    public float jumpforce;
    [Header("�÷��̾� �̵� �ӵ� ����")]
    public float movespeed;
    [Header("ī�޶� �߰� �ð�")]
    public float CameraTrackingTime;

    //[MenuItem("Playerprefs/��� ���� �� �����")]
    //static void RemovePref()
    //{
    //    PlayerPrefs.DeleteAll();
    //}
    //[MenuItem("Playerprefs/���� �� �ҷ�����")]
    //static void LoadPref()
    //{
    //    if (PlayerPrefs.HasKey("jumpforce"))
    //    {
    //        ProjectSetting.instance.jumpforce = PlayerPrefs.GetFloat("jumpforce");
    //    }
    //    if (PlayerPrefs.HasKey("movespeed"))
    //    {
    //        ProjectSetting.instance.movespeed = PlayerPrefs.GetFloat("movespeed");
    //    }
    //    if (PlayerPrefs.HasKey("CameraTrackingTime"))
    //    {
    //        ProjectSetting.instance.CameraTrackingTime = PlayerPrefs.GetFloat("CameraTrackingTime");
    //    }
    //}
    public static ProjectSetting instance;
    private void Awake()
    {
        instance= this;
    }
    void Start()
    {
        GravityValue = Physics.gravity;
        GravityX = Physics.gravity.x;
        GravityY = Physics.gravity.y;
        GravityZ = Physics.gravity.z;
        if (jumpforce==0)
            jumpforce = PlayerStat.instance.jumpForce;
        if(movespeed==0)
             movespeed = PlayerStat.instance.moveSpeed;
     
    }
    public void SavePref()
    {
        PlayerPrefs.SetFloat("jumpforce", jumpforce);
        PlayerPrefs.SetFloat("movespeed", movespeed);
        PlayerPrefs.SetFloat("CameraTrackingTime", CameraTrackingTime);
    }
    //private void OnValidate()
    //{
    //    SavePref();
    //}

    void Update()
    {
        GravityValue=new Vector3 (GravityX, GravityY, GravityZ);
        Physics.gravity = GravityValue;
            PlayerStat.instance.jumpForce = jumpforce;
            PlayerStat.instance.initMoveSpeed = movespeed;
        SavePref();
    }
}
