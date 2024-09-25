using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraChangerEvent : OutputEvent
{
    public int cameraindex;
    public override void output()
    {
        base.output();
        var a = PlayerHandler.instance.CurrentCamera.GetComponent<CameraManager>();
        a.ActiveCamera(cameraindex);
    }
}
