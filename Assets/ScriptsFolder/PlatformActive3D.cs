using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActive3D : MonoBehaviour
{
    Collider Bcollider;
    Renderer renderer_;
    public bool BcolliderActive3D=true;
    private void Awake()
    {
        Bcollider = GetComponent<BoxCollider>();
        renderer_=GetComponent<Renderer>();
    }
    void PlatformChange3D()
    {
        if (BcolliderActive3D)
        {
            Bcollider.enabled = true;
            if(renderer_ != null)
                renderer_.enabled = true;
        }
        else
        {
            Bcollider.enabled = false;
            if (renderer_ != null)
                renderer_.enabled = false;
        }
      

    }
    void PlatformChange2D()
    {
        if (BcolliderActive3D)
        {
            Bcollider.enabled = false;
            if (renderer_ != null)
                renderer_.enabled = false;
        }
        else
        {
            Bcollider.enabled = true;
            if (renderer_ != null)
                renderer_.enabled = true;
        }

    }

   
    void Update()
    {
        if (BcolliderActive3D)
        {
            if (!Bcollider.enabled && (int)PlayerStat.instance.MoveState >= 4)
            {
                PlatformChange3D();
            }
            else if (Bcollider.enabled && (int)PlayerStat.instance.MoveState < 4)
            {
                PlatformChange2D();
            }
        }
        else
        {
            if (Bcollider.enabled && (int)PlayerStat.instance.MoveState >= 4)
            {
                PlatformChange3D();
            }
            else if (!Bcollider.enabled && (int)PlayerStat.instance.MoveState< 4)
            {
                PlatformChange2D();
            }
        }
    }
}
