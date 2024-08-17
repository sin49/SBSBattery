using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;



//�����ϱ�
//1.ī�޶� �ɼ� �ݿ��ϱ� 2D/3D
//����.CineMacihneBasicCamera ��� Ȯ���ϱ�?
//2.ī�޶� ��ȯ �� ortho/Pers ��ȯ �ڿ������� �ϱ�
//3.ī�޶� ��ȯ ��� Blend/NotBlend �� �����Ҽ��ְ�
public class CameraManager_Switching2D3D : CameraManagerSwitchingBlendingOption
{
   public CinemachineVirtualCamera camera2D;
    public CinemachineVirtualCamera camera3D;
    public Vector3 Camera2Drotation;
    public Vector3 Camera3Drotation;

    //[Header("2D ī�޶� orthographic ������")]
    //public float orthographicSize2D = 5f;
    //[Header("2D ī�޶� near/far clipping planes")]
    //public float nearClipPlane2D = -10f;
    //public float farClipPlane2D = 10f;
    //[Header("3D ī�޶� field of view")]
    //public float fieldOfView3D = 60f;
    //[Header("3D ī�޶� near/far clipping planes")]
    //public float nearClipPlane3D = 0.1f;
    //public float farClipPlane3D = 1000f;

    bool trans3D;
    protected override void initializeCamera()
    {
        var a = VirtualCameraTransform.GetComponentsInChildren<CinemachineVirtualCamera>();
        VirtualCameras = new CinemachineVirtualCamera[a.Length - 1];
        GetCameraSettingByTrans3D();
        camera3D.GetComponent<CineMachineBasicCamera>().CameraIndex = 0;
        var confiner = camera3D.GetComponent<CinemachineConfiner>();
        confiner.m_BoundingVolume = BasicCameraConfiner;
        camera2D.GetComponent<CineMachineBasicCamera>().CameraIndex = 0;
        confiner = camera2D.GetComponent<CinemachineConfiner>();
        confiner.m_BoundingVolume = BasicCameraConfiner;
        int i = 0;
        for (int n = 0; n < a.Length; n++)
        {
            if (n == 0)
                continue;
            if (a[n] == camera2D || a[n] == camera3D)
            {
                i--;
                continue;
            }

            a[n+i].gameObject.SetActive(false);
            
            a[n+i].GetComponent<CineMachineBasicCamera>().CameraIndex = n+i;
            VirtualCameras[n+i] = a[n+i];
        }
        VirtualCameras[0].gameObject.SetActive(true);
        activedcamera = VirtualCameras[0];
    }
    protected override void Start()
    {
        base.Start();
        PlayerHandler.instance.registerCorutineRegisterEvent(RegiserCameraChangeHandler);
    }
    void RegiserCameraChangeHandler()
    {
        PlayerHandler.instance.RegisterCameraRotateCorutine(SwitchCameraForTransDimensionCorutine());
    }
    IEnumerator SwitchCameraForTransDimensionCorutine()
    {
        trans3D = !trans3D;

        if (trans3D)
        {
            PlayerStat.instance.MoveState =PlayerMoveState.Trans3D;
        }
        else
        {
            PlayerStat.instance.MoveState = PlayerMoveState.SideX;
        }
        PlayerHandler.instance.CurrentPlayer.rotateBy3Dto2D();
        PlayerHandler.instance.CantHandle = true;
        if (trans3D)
        {
            yield return StartCoroutine(SwitchCameraCoroutine(camera3D));
        }
        else
        {
            yield return StartCoroutine(SwitchCameraCoroutine(camera2D));
        }
        PlayerHandler.instance.CantHandle = false;
        GetCameraSettingByTrans3D();
    }
    void GetCameraSettingByTrans3D()
    {
        if (trans3D)
        {
            SwapDefaultCamera(camera3D);
            camera2D.gameObject.SetActive(false);
        }
        else
        {
            SwapDefaultCamera(camera2D);
            camera3D.gameObject.SetActive(false);
        }
    }

    public void SwapDefaultCamera(CinemachineVirtualCamera camera)
    {
        VirtualCameras[0] = camera;
    }
}
