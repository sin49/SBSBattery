 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] VirtualCameras;
    public CinemachineVirtualCamera activedcamera;
    public BoxCollider BasicCameraConfiner;
 protected  virtual void initializeCamera()
    {
        VirtualCameras=this.transform.GetComponentsInChildren<CinemachineVirtualCamera>();
        for(int n=0;n<VirtualCameras.Length;n++)
        {
            VirtualCameras[n].gameObject.SetActive(false);
            VirtualCameras[n].GetComponent<CineMachineBasicCamera>().CameraIndex = n;
        }
        VirtualCameras[0].gameObject.SetActive(true);
       var a=  VirtualCameras[0].GetComponent<CinemachineConfiner>();
        a.m_BoundingVolume = BasicCameraConfiner;
        activedcamera = VirtualCameras[0];
    }

    protected virtual void Awake()
    {
        initializeCamera();
    }
    private void Update()
    {
        if (PlayerHandler.instance != null)
            PlayerHandler.instance.CurrentCamera = GetComponent<Camera>();
    }
    public virtual void ActiveCamera(int n)
    {
        if (n >= VirtualCameras.Length)
            return;
        activedcamera.gameObject.SetActive(false);
        VirtualCameras[n].gameObject.SetActive(true);
        activedcamera = VirtualCameras[n];
    }
}
