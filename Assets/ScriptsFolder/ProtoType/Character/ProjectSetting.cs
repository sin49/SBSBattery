using UnityEngine;
using UnityEditor;
using System;


public class ProjectSetting :MonoBehaviour
{
    Vector3 GravityValue;
    [Header("중력의 X값 조절(X값 물리 연산 가중치)")]
    public float GravityX;
    [Header("중력의 Y값 조절(Y값 물리 연산 가중치)")]
    public float GravityY;
    [Header("중력의 Z값 조절(Y값 물리 연산 가중치)")]
    public float GravityZ;

    [Header("플레이어 점프 높이 조절")]
    public float jumpforce;
    [Header("플레이어 이동 속도 조절")]
    public float movespeed;
    [Header("카메라 추격 시간")]
    public float CameraTrackingTime;

    //[MenuItem("Playerprefs/모든 저장 값 지우기")]
    //static void RemovePref()
    //{
    //    PlayerPrefs.DeleteAll();
    //}
    //[MenuItem("Playerprefs/저장 값 불려오기")]
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
