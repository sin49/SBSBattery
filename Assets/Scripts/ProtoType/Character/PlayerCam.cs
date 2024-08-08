using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCam : BasicCamera
{
    [Header("ī�޶� ��ġ ��")]
    public Vector3 InitCamPos;
    [Header("ī�޶� ȸ�� ��")]
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
