using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;

public class CineMachineBasicCamera : MonoBehaviour
{
  public  CinemachineVirtualCamera virtualcamera;

    public string SettingName;

    [Header("ī�޶� �÷��̾� �ȷο�")]
    public bool CameraFollowPlayer = true;
    [Header("ī�޶� �÷��̾� ��")]
    public bool CameraLookPlayer;
    [HideInInspector]
    public int CameraIndex;


    Transform target;
    private void Awake()
    {
        virtualcamera = GetComponent<CinemachineVirtualCamera>();
  
    }

    protected virtual void FixedUpdate()
    {
        TargetIsPlayer();
    }
    protected virtual void TargetIsPlayer()
    {
        //Ÿ�� Ž��
        if (PlayerHandler.instance!=null&&PlayerHandler.instance.CurrentPlayer != null)
        {
            target = PlayerHandler.instance.CurrentPlayer.transform;

            if(CameraFollowPlayer)
                virtualcamera.Follow = target;
            if (CameraLookPlayer)
                virtualcamera.LookAt = target;
        }


    }

}