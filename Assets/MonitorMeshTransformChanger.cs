using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorMeshTransformChanger : MonoBehaviour
{
    public Vector3 Local2DPosition;
    public Vector3 Local2DScale;
    public Vector3 Local3DPosition;
    public Vector3 Local3DScale;


    // Update is called once per frame
    private void Start()
    {
    
        PlayerHandler.instance.registerCameraChangeAction(changeMonitortransform);
    }
    void changeMonitortransform()
    {
        if ((int)PlayerStat.instance.MoveState < 4)
        {
            transform.localPosition = Local2DPosition;
            transform.localScale = Local2DScale;
        }
        else
        {
            transform.localPosition = Local3DPosition;
            transform.localScale = Local3DScale;
        }
    }
    
    //void Update()
    //{
       
    //}
}
