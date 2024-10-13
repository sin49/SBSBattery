using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;




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
    

    public TextMeshProUGUI TEstTExt;
    public void switch2Dcamera(CinemachineVirtualCamera c)
    {
        activedcamera.enabled = false;
        c.enabled = true;
        activedcamera = c;
        if(trans3D)

        trans3D = false;
    }

   public bool trans3D;
    public void Set2DCamerabinding(Collider col)
    {
        this.camera2D.GetComponent<CinemachineConfiner>().m_BoundingVolume = col;
      
    }
    public void Set3DCamerabinding(Collider col)
    {
        this.camera3D.GetComponent<CinemachineConfiner>().m_BoundingVolume = col;
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
            camera2D.enabled = true;
        }
        else
        {
            camera2D.enabled = (false);
            camera3D.enabled = true;
        }
    }
    protected override void Start()
    {
        base.Start();
        cam = this.GetComponent<Camera>();
        PlayerHandler.instance.registerCorutineRegisterEvent(RegiserCameraChangeHandler);
        camera2D.m_Lens.Orthographic = true;
        if (camera2D!=null)
        orthosize = camera2D.m_Lens.OrthographicSize;
        fovview = camera3D.m_Lens.FieldOfView;
        camera3D.enabled = true;
        camera2D.enabled = false;
        updatecamera();


        cam.cullingMask = ~(1 << 23);
     

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

    public renderpassmanager renderpassmanager_;
  public  IEnumerator SwitchCameraForTransDimensionCorutine()

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
        renderpassmanager_.changepixel(trans3D);
        if (trans3D)
        {
            cam.cullingMask = ~(1 << 24);
            //camera3D.transform.position = camera2D.transform.position;
            yield return StartCoroutine(SwitchCameraCoroutine(camera3D));
          
        }
        else
        {
            cam.cullingMask = ~(1 << 23);
            //camera2D.transform.position = camera3D.transform.position;
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
            if (camera2D != null) 
                camera2D.enabled = (false);
            camera3D.enabled = true;
        }
        else
        {
            SwapDefaultCamera(camera2D);
            if (camera3D != null)
                camera3D.enabled = (false);
            camera2D.enabled = true;
        }
    }
    public bool noorthochange;
    public IEnumerator SwitchCameraForTransDimensionCorutinenoblending()

    {
        if (camera2D == null || camera3D == null)
            yield break;
        //trans3D = !trans3D;

        //UpdatePlayerMovestate();
        PlayerHandler.instance.CurrentPlayer.rotateBy3Dto2D();
        PlayerHandler.instance.CantHandle = true;
        if(!noorthochange)
        camera2D.m_Lens.Orthographic = false;
        camera2D.m_Lens.FieldOfView = fovview;
  
        renderpassmanager_.changepixel(trans3D);
        if (trans3D)
        {
            //camera3D.transform.position = camera2D.transform.position;
            yield return StartCoroutine(SwitchCameraCoroutine(camera3D));

        }
        else
        {
            //camera2D.transform.position = camera3D.transform.position;
            yield return StartCoroutine(SwitchCameraCoroutine(camera2D));
        }
        Time.timeScale = 1;
        if (!noorthochange)
            camera2D.m_Lens.Orthographic = true;
        camera2D.m_Lens.OrthographicSize = orthosize;
        PlayerHandler.instance.CantHandle = false;
        GetCameraSettingByTrans3D();
    }
    
    public IEnumerator SwitchCameranoblending(CinemachineVirtualCamera newCamera)
    {
        if (activedcamera == newCamera)
            yield break;

       
        if (activedcamera != null)
        {



            // Blend 설정

            //var blend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, transitionDuration);
            //cinemachineBrain.m_DefaultBlend = blend;
            cam.orthographic = false;
            // 현재 카메라와 새 카메라 간의 전환 시작
            Vector3 newcameraposition = newCamera.transform.position;
            newCamera.enabled = (true);
            activedcamera.enabled = (false);
           
            
            newCamera.transform.position = newcameraposition;
   
            activedcamera = newCamera;

           

            //if (currentCamera != newCamera)
            //{
            //    currentCamera.gameObject.SetActive(false);
            //}
        }
        yield return null;
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
