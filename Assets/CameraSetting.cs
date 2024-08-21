using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

