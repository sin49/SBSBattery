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
    [Range(0, 1), Header("ȭ�� Y�� ����")]
    public float ScreentYHeight;
    [Range(0, 1), Header("Y�൥����")]
    public float CameraYDeadZonr;
    [Range(0, 1), Header("X�� ������")]
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
            //// �ʱ�ȭ�ϰ� ���� �������� ����
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
        //Ÿ�� Ž��
        if (PlayerHandler.instance.CurrentPlayer != null)
        {
            target = PlayerHandler.instance.CurrentPlayer.transform;
            virtualcamera.Follow = target;
        }


    }

}