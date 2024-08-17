using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraManagerSwitchingBlendingOption : CameraManager
{

    public float transitionDuration = 1.0f; // ī�޶� ��ȯ �ð�

    private CinemachineBrain cinemachineBrain;
    private bool isTransitioning;
    protected virtual void Start()
    {
        cinemachineBrain = this.GetComponent<CinemachineBrain>();
    }
    public override void ActiveCamera(CinemachineVirtualCamera camera,Collider col)
    {
        base.ActiveCamera(camera, col);
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

    public IEnumerator SwitchCameraCoroutine(CinemachineVirtualCamera newCamera)
    {
        isTransitioning = true;

   
    
        if (activedcamera != null)
        {
     
        

            // Blend ����
            var blend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, transitionDuration);
            cinemachineBrain.m_DefaultBlend = blend;
  
            // ���� ī�޶�� �� ī�޶� ���� ��ȯ ����
            activedcamera.gameObject.SetActive(false);
            newCamera.gameObject.SetActive(true);
          
            activedcamera = newCamera;

            yield return new WaitForSeconds(transitionDuration);

            //if (currentCamera != newCamera)
            //{
            //    currentCamera.gameObject.SetActive(false);
            //}
        }

        isTransitioning = false;
    }
}
