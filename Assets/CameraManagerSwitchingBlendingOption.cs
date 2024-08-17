using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraManagerSwitchingBlendingOption : CameraManager
{

    

    private CinemachineBrain cinemachineBrain;
    private bool isTransitioning;
    protected virtual void Start()
    {
        cinemachineBrain = this.GetComponent<CinemachineBrain>();
        cam = GetComponent<Camera>();
    }
    public override void ActiveCamera(CinemachineVirtualCamera camera,Collider col)
    {
        base.ActiveCamera(camera, col);
        SwitchToCamera(camera);
    }
    public void settingccamera(CinemachineVirtualCamera camera2D, CinemachineVirtualCamera camera3D,
        Collider col)
    {
        camera2D.GetComponent<CinemachineConfiner>().m_BoundingVolume = col;
        camera3D.GetComponent<CinemachineConfiner>().m_BoundingVolume = col;
    }
    public override void ActiveCamera(int n,Collider col)
    {
        if (n >= VirtualCameras.Length)
            return;

        SwitchToCamera(VirtualCameras[n]);
    }
    public void SwitchToCamera(CinemachineVirtualCamera newCamera)
    {
        if (isTransitioning) return;

        StartCoroutine(SwitchCameraCoroutine(newCamera));
    }
    Camera cam;
    public IEnumerator SwitchCameraCoroutine(CinemachineVirtualCamera newCamera)
    {
        isTransitioning = true;

   
    
        if (activedcamera != null)
        {



            // Blend 설정
   
            var blend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, transitionDuration);
            cinemachineBrain.m_DefaultBlend = blend;
            cam.orthographic = false;
            // 현재 카메라와 새 카메라 간의 전환 시작
            activedcamera.gameObject.SetActive(false);
            newCamera.gameObject.SetActive(true);
        
             activedcamera = newCamera;
            Time.timeScale = 0;
            Debug.Log("timescale" + Time.timeScale);
            yield return new WaitForSecondsRealtime(transitionDuration);
            Time.timeScale = 1;
            Debug.Log("timescale" + Time.timeScale);
            //if (currentCamera != newCamera)
            //{
            //    currentCamera.gameObject.SetActive(false);
            //}
        }

        isTransitioning = false;
    }
}
