using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
[CreateAssetMenu(fileName = "CameraSettings", menuName = "Cinemachine/CameraSettings", order = 1)]
public class CameraSetting : ScriptableObject
{
    [Header("Lens setting")]
    public float FieldOfView;
    public float orthosize;
    public bool orthgraphics;
    public float NearClipPlane;
    public float FarClipPlane;

    [Header("Framing Transposer setting")]
    public Vector3 FollowOffset;

    public float lookaheadtime;
    public float lookaheadsmoothing;
    public bool aheadignoreY;

    public float dampingx;
    public float dampingy;
    public float dampingz;

    public float cameradistance;

    public bool targetmovementonly;

    public float DeadZoneWidth;
    public float DeadZoneHeight;

    public float ScreenX;
    public float ScreenY;

    public float SoftZoneWidth;
    public float SoftZoneHeight;

    public float BiasX;
    public float BiasY;

    public bool centerActive;
}

public class CineMachineBasicCamera : MonoBehaviour
{
  public  CinemachineVirtualCamera virtualcamera;

    public string SettingName;

    [HideInInspector]
    public int CameraIndex;

    public CameraSetting setting;

    Transform target;
    private void Awake()
    {
        virtualcamera = GetComponent<CinemachineVirtualCamera>();
  
    }
    public void SaveCamSetting()
    {
        if (virtualcamera != null)
        {
            // Lens settings 저장
            var lens = virtualcamera.m_Lens;
            setting.FieldOfView = lens.FieldOfView;
            setting.orthosize = lens.OrthographicSize;
            setting.orthgraphics = lens.Orthographic;
            setting.NearClipPlane = lens.NearClipPlane;
            setting.FarClipPlane = lens.FarClipPlane;

            // Framing Transposer settings 저장
            var framingTransposer = virtualcamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                setting.FollowOffset = framingTransposer.m_TrackedObjectOffset;

                setting.lookaheadtime = framingTransposer.m_LookaheadTime;
                setting.lookaheadsmoothing = framingTransposer.m_LookaheadSmoothing;
                setting.aheadignoreY = framingTransposer.m_LookaheadIgnoreY;

                setting.dampingx = framingTransposer.m_XDamping;
                setting.dampingy = framingTransposer.m_YDamping;
                setting.dampingz = framingTransposer.m_ZDamping;

                setting.cameradistance = framingTransposer.m_CameraDistance;

                setting.targetmovementonly = framingTransposer.m_TargetMovementOnly;

                setting.DeadZoneWidth = framingTransposer.m_DeadZoneWidth;
                setting.DeadZoneHeight = framingTransposer.m_DeadZoneHeight;

                setting.SoftZoneWidth = framingTransposer.m_SoftZoneWidth;
                setting.SoftZoneHeight = framingTransposer.m_SoftZoneHeight;

                setting.ScreenX = framingTransposer.m_ScreenX;
                setting.ScreenY = framingTransposer.m_ScreenY;

                setting.BiasX = framingTransposer.m_BiasX;
                setting.BiasY = framingTransposer.m_BiasY;
                setting.centerActive = framingTransposer.m_CenterOnActivate;
            }
        }
    }
   public void ApplySettings()
    {
        if (setting != null && virtualcamera != null)
        {
            // Lens setting 적용
            virtualcamera.m_Lens.Orthographic = setting.orthgraphics;
            if (!setting.orthgraphics)
            {
                virtualcamera.m_Lens.FieldOfView = setting.FieldOfView;
            }
            else
            {
                virtualcamera.m_Lens.OrthographicSize = setting.orthosize;

            }
            virtualcamera.m_Lens.NearClipPlane = setting.NearClipPlane;
            virtualcamera.m_Lens.FarClipPlane = setting.FarClipPlane;

            // Framing Transposer setting 적용
            var framingTransposer = virtualcamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {



                framingTransposer.m_TrackedObjectOffset = setting.FollowOffset;

                framingTransposer.m_LookaheadIgnoreY = setting.aheadignoreY;
                framingTransposer.m_LookaheadSmoothing = setting.lookaheadsmoothing;
                framingTransposer.m_LookaheadTime = setting.lookaheadtime;

                framingTransposer.m_XDamping = setting.dampingx;
                framingTransposer.m_YDamping = setting.dampingy;
                framingTransposer.m_ZDamping = setting.dampingz;

                framingTransposer.m_CameraDistance = setting.cameradistance;

                framingTransposer.m_TargetMovementOnly = setting.targetmovementonly;

                framingTransposer.m_DeadZoneWidth = setting.DeadZoneWidth;
                framingTransposer.m_DeadZoneHeight = setting.DeadZoneHeight;

                framingTransposer.m_SoftZoneWidth = setting.SoftZoneWidth;
                framingTransposer.m_SoftZoneHeight = setting.SoftZoneHeight;

                framingTransposer.m_ScreenX = setting.ScreenX;
                framingTransposer.m_ScreenY = setting.ScreenY;

                framingTransposer.m_BiasX = setting.BiasX;
                framingTransposer.m_BiasY = setting.BiasY;
                framingTransposer.m_CenterOnActivate = setting.centerActive;

                // 필요한 경우 다른 CinemachineFramingTransposer 설정을 추가합니다.
            }
        }
    }
    protected virtual void FixedUpdate()
    {
        TargetIsPlayer();
    }
    protected virtual void TargetIsPlayer()
    {
        //타겟 탐색
        if (PlayerHandler.instance!=null&&PlayerHandler.instance.CurrentPlayer != null)
        {
            target = PlayerHandler.instance.CurrentPlayer.transform;
            virtualcamera.Follow = target;
        }


    }

}