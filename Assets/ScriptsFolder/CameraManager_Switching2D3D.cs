using Cinemachine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;



//개선하기
//1.카메라 옵션 반영하기 2D/3D
//선택.CineMacihneBasicCamera 기능 확장하기?
//2.카메라 전환 중 ortho/Pers 전환 자연스럽게 하기
//3.카메라 전환 방식 Blend/NotBlend 로 선택할수있게
public class CameraManager_Switching2D3D : CameraManagerSwitchingBlendingOption
{
   public CinemachineVirtualCamera camera2D;
    public CinemachineVirtualCamera camera3D;
    public Vector3 Camera2Drotation;
    public Vector3 Camera3Drotation;
    public PlayerMoveState movestate2D;
    public PlayerMoveState movestate3D;
    float orthosize;
    float fovview;
    //[Header("2D 카메라 orthographic 사이즈")]
    //public float orthographicSize2D = 5f;
    //[Header("2D 카메라 near/far clipping planes")]
    //public float nearClipPlane2D = -10f;
    //public float farClipPlane2D = 10f;
    //[Header("3D 카메라 field of view")]
    //public float fieldOfView3D = 60f;
    //[Header("3D 카메라 near/far clipping planes")]
    //public float nearClipPlane3D = 0.1f;
    //public float farClipPlane3D = 1000f;

   public bool trans3D;
    public void Set2DCamerabinding(Collider col)
    {
        this.camera2D.GetComponent<CinemachineConfiner>().m_BoundingVolume = col;
      
    }
    public void Set3DCamerabinding(Collider col)
    {
        this.camera3D.GetComponent<CinemachineConfiner>().m_BoundingVolume = col;
    }
    public void settingBoss1ccamera(CinemachineVirtualCamera camera2D, CinemachineVirtualCamera camera3D,
        Collider col, PlayerMoveState movestate)
    {
        camera3D.enabled = (false); 
        this.camera2D= camera2D;
        this.camera3D = camera3D;
        camera2D.enabled = (false);
        camera3D.enabled = (true);
        if (col != null)
        {
            this.camera2D.GetComponent<CinemachineConfiner>().m_BoundingVolume = col;
            this.camera3D.GetComponent<CinemachineConfiner>().m_BoundingVolume = col;
        }
        camera2D.m_Lens.Orthographic = true;
        orthosize = camera2D.m_Lens.OrthographicSize;
        fovview = camera3D.m_Lens.FieldOfView;
      
        trans3D = true;
        //this. movestate = movestate;
        //PlayerStat.instance.MoveState = this.movestate;
        VirtualCameras[0] = camera3D;
        activedcamera = camera3D;
        //GetCameraSettingByTrans3D();
    }
    protected override void initializeCamera()
    {
      
    
        GetCameraSettingByTrans3D();
        camera3D.GetComponent<CineMachineBasicCamera>().CameraIndex = 0;
        var confiner = camera3D.GetComponent<CinemachineConfiner>();
        if (camera2D != null)
        {
            camera2D.GetComponent<CineMachineBasicCamera>().CameraIndex = 0;
            confiner = camera2D.GetComponent<CinemachineConfiner>();
        }
        int i = 0;
       
        VirtualCameras[0].enabled = (true);
        activedcamera = VirtualCameras[0];
   
    }
    public void updatecamera()
    {



        if ((int)PlayerStat.instance.MoveState < 4)
        {
            camera3D.enabled=(false);
        }
        else
        {
            camera2D.enabled = (false);
        }
    }
    protected override void Start()
    {
        base.Start();
        PlayerHandler.instance.registerCorutineRegisterEvent(RegiserCameraChangeHandler);
        camera2D.m_Lens.Orthographic = true;
        if (camera2D!=null)
        orthosize = camera2D.m_Lens.OrthographicSize;
        fovview = camera3D.m_Lens.FieldOfView;

        updatecamera();
       
    }
    void RegiserCameraChangeHandler()
    {
        PlayerHandler.instance.RegisterCameraRotateCorutine(SwitchCameraForTransDimensionCorutine());
    }
    public void UpdatePlayerMovestate()
    {
        if (trans3D)
        {
            PlayerStat.instance.MoveState = movestate3D;
        }
        else
        {
            PlayerStat.instance.MoveState = movestate2D;
        }
    }
    IEnumerator SwitchCameraForTransDimensionCorutine()
    {
        if (camera2D == null || camera3D == null)
            yield break;
        //trans3D = !trans3D;

        //UpdatePlayerMovestate();
        PlayerHandler.instance.CurrentPlayer.rotateBy3Dto2D();
        PlayerHandler.instance.CantHandle = true;
        camera2D.m_Lens.Orthographic = false;
        camera2D.m_Lens.FieldOfView = fovview;
        Time.timeScale = 0;
        if (trans3D)
        {
            camera3D.transform.position = camera2D.transform.position;
            yield return StartCoroutine(SwitchCameraCoroutine(camera3D));
          
        }
        else
        {
            camera2D.transform.position = camera3D.transform.position;
            yield return StartCoroutine(SwitchCameraCoroutine(camera2D));
        }
        Time.timeScale = 1;
        camera2D.m_Lens.Orthographic = true;
        camera2D.m_Lens.OrthographicSize = orthosize;
        PlayerHandler.instance.CantHandle = false;
        GetCameraSettingByTrans3D();
    }
    void GetCameraSettingByTrans3D()
    {
        if (trans3D)
        {
            SwapDefaultCamera(camera3D);
            if(camera2D!=null)
            camera2D            (false);
        }
        else
        {
            SwapDefaultCamera(camera2D);
            if (camera3D != null)
                camera3D.enabled = (false);
        }
    }

    public void SwapDefaultCamera(CinemachineVirtualCamera camera)
    {
        if (VirtualCameras.Length == 0)
        {
            VirtualCameras=new CinemachineVirtualCamera[1];
        }
        VirtualCameras[0] = camera;
    }
}
