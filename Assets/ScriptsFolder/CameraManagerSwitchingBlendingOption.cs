using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraManagerSwitchingBlendingOption : CameraManager
{

    

    private CinemachineBrain cinemachineBrain;
    protected bool isTransitioning;
    protected override void Start()
    {
        base.Start();
        cinemachineBrain = this.GetComponent<CinemachineBrain>();
        cam = GetComponent<Camera>();
    }
    public override void ActiveCamera(CinemachineVirtualCamera camera,Collider col)
    {
        base.ActiveCamera(camera, col);
        SwitchToCamera(camera);
    }
    public override void ActiveCamera(CinemachineVirtualCamera camera)
    {
        base.ActiveCamera(camera);
        SwitchToCamera(camera);
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
   protected Camera cam;
   
    public IEnumerator SwitchCameraCoroutine(CinemachineVirtualCamera newCamera)
    {
        if (activedcamera == newCamera)
            yield break;

        isTransitioning = true;

   
    
        if (activedcamera != null)
        {



            // Blend 설정
   
            var blend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, transitionDuration);
            cinemachineBrain.m_DefaultBlend = blend;
            cam.orthographic = false;
            // 현재 카메라와 새 카메라 간의 전환 시작
            Vector3 newcameraposition = newCamera.transform.position;

            activedcamera.enabled = (false);
            Debug.Log(activedcamera.name + "activedcamera");
            newCamera.enabled = (true);
            newCamera.transform.position = newcameraposition;
            Debug.Log(newCamera.name + "activedcamera");
            activedcamera = newCamera;
     
            yield return new WaitForSecondsRealtime(transitionDuration);
  
            //if (currentCamera != newCamera)
            //{
            //    currentCamera.gameObject.SetActive(false);
            //}
        }

        isTransitioning = false;
    }
}
