using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineMachineBasicCamera : MonoBehaviour
{
    CinemachineVirtualCamera virtualcamera;
    public Vector3 campos;

    [HideInInspector]
    public int CameraIndex;

    public Vector3 camrot;
    [Range(0, 1), Header("화면 Y축 범위")]
    public float ScreentYHeight;
    [Range(0, 1), Header("Y축데드존")]
    public float CameraYDeadZonr;
    [Range(0, 1), Header("X축 데드존")]
    public float CameraXViewportPos;

    Transform target;
    private void Awake()
    {
        virtualcamera = GetComponent<CinemachineVirtualCamera>();
        //InitCamSetting();
    }
    void InitCamSetting()
    {
        var transposer = virtualcamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (transposer != null)
        {
            //// 초기화하고 싶은 오프셋을 설정
            //transposer.m_FollowOffset = campos;
            transposer.m_ScreenY = ScreentYHeight;

            transposer.m_DeadZoneHeight = CameraYDeadZonr;
            transposer.m_DeadZoneWidth = CameraXViewportPos;
        }


        virtualcamera.transform.rotation = Quaternion.Euler(camrot);
    }
    protected virtual void FixedUpdate()
    {
        TargetIsPlayer();
        //InitCamSetting();
    }
    protected virtual void TargetIsPlayer()
    {
        //타겟 탐색
        if (PlayerHandler.instance.CurrentPlayer != null)
        {
            target = PlayerHandler.instance.CurrentPlayer.transform;
            virtualcamera.Follow = target;
        }


    }

}