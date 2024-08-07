using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CameraMoveRange : MonoBehaviour
{
    [Header("맵 중심점")]
    public Vector3 Center;
    [Header("카메라 이동 범위")]
    public Vector3 Range;

    Camera c;

    [Header("X값 고정")]
    public bool Xpin;
    [Header("X값 고정")]
    public bool YPin;
    [Header("Z값 고정")]
    public bool Zpin;
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireCube((Vector3)Center, Range);
    }


    void Start()
    {
        c = GetComponent<Camera>();
       
    }
    //perspective: float height = 2.0f * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad) * distance;
    //orthographic: height = Camera.main.orthographicSize;
    public void BindingCamera(Camera c)
    {
        if (c == null&&Range==Vector3.zero)
            return;
        float height = 0;
        float width = 0;
   
        if (!c.orthographic )
        {
            float distance=0;
            if (!PlayerStat.instance.Trans3D)
            distance = Mathf.Abs(this.transform.position.z - Center.z);
            else if(PlayerHandler.instance.CurrentPlayer)
                distance = Mathf.Abs(this.transform.position.z -
                    PlayerHandler.instance.CurrentPlayer.transform.position.z);
            height = 2 * Mathf.Tan(c.fieldOfView * 0.5f * Mathf.Deg2Rad) * distance;

        }
        else
        {
            height = 2 * c.orthographicSize;
        }
        width = height * Screen.width / Screen.height;
        if (!PlayerStat.instance.Trans3D)
        {
            ApplyCameraBinding(Center, Range, width, height);
        }
        else
        {
            ApplyCameraZBinding(Center, Range, width);
        }
    }
    void ApplyCameraBinding(Vector3 center,Vector3 range,float width,float height)
    {
        float clampX = Center.x;
        if (!Xpin)
        {
            float lx = Range.x * 0.5f - width * 0.5f;
            clampX = Mathf.Clamp(transform.position.x, -lx + Center.x,
               lx + Center.x);
 
        }
        float clampY = Center.y;
        if (!YPin)
        {
            float ly = Range.y * 0.5f - height * 0.5f;
            clampY = Mathf.Clamp(transform.position.y, -ly + Center.y,
                ly + Center.y);
            if (clampY == ly + Center.y || clampY == -ly + Center.y)
                Debug.Log("Y좌표가 한계에 걸림");
        }
        transform.position = new Vector3(clampX, clampY, transform.position.z);
    }
   void ApplyCameraZBinding(Vector3 center, Vector3 range, float width)
    {
        float clampZ = Center.z;
        if (!Zpin)
        {
            float lz = Range.z * 0.5f - width * 0.5f;
            clampZ = Mathf.Clamp(transform.position.z, -lz + Center.z,
                lz + Center.z);
            if (clampZ == lz + Center.z || clampZ == -lz + Center.z)
                Debug.Log("Z좌표가 한계에 걸림");
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, clampZ);
    }
}
