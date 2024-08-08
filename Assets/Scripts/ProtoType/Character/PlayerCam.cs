using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCam : BasicCamera
{
    [Header("카메라 위치 값")]
    public Vector3 InitCamPos;
    [Header("카메라 회전 값")]
    public Vector3 InitCamrot;

   protected override void Start()
    {
        base.Start();
        CurrentCamera = GetComponent<Camera>();
        camPos = InitCamPos;
        camRot = InitCamrot;
        CurrentCamera.transform.position = PlayerSpawnManager.Instance.GetCurrentCheckpoint().transform.position+camPos;
        //CurrentCamera.transform.rotation = quaternion.Euler(camRot);
        //ZPin = true;
    }



    protected override void FixedUpdate()
    {
        camPos = InitCamPos;
        camRot = InitCamrot;

        base.FixedUpdate();
    }



}
