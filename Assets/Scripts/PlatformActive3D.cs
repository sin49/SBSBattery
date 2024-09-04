using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActive3D : MonoBehaviour
{
    Collider Bcollider;
    MeshRenderer renderer_;
    public float Zmove = 1;
    public bool BcolliderActive3D=true;
    private void Awake()
    {
        Bcollider = GetComponent<BoxCollider>();
        renderer_=GetComponent<MeshRenderer>();
    }
    void PlatformChange3D()
    {
        if (BcolliderActive3D)
        {
            Bcollider.enabled = true;
            renderer_.enabled = true;
        }
        else
        {
            Bcollider.enabled = false;
        renderer_.enabled = false;
        }
        transform.Translate(Vector3.back * Zmove);
    }
    void PlatformChange2D()
    {
        if (BcolliderActive3D)
        {
            Bcollider.enabled = false;
            renderer_.enabled = false;
        }
        else
        {
            Bcollider.enabled = true;
            renderer_.enabled = true;
        }
        transform.Translate(Vector3.forward * Zmove);
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
