using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActive3D : MonoBehaviour
{
    BoxCollider Bcollider;
    public float Zmove = 1;
    public bool BcolliderActive3D=true;
    private void Awake()
    {
        Bcollider = GetComponent<BoxCollider>();
    }
    void PlatformChange3D()
    {
        if (BcolliderActive3D) 
        Bcollider.enabled = true;
        else
            Bcollider.enabled = false;
        transform.Translate(Vector3.back * Zmove);
    }
    void PlatformChange2D()
    {
        if (BcolliderActive3D)
            Bcollider.enabled = false;
        else
            Bcollider.enabled = true;
        transform.Translate(Vector3.forward * Zmove);
    }

   
    void Update()
    {
        if (BcolliderActive3D)
        {
            if (!Bcollider.enabled && PlayerStat.instance.Trans3D)
            {
                PlatformChange3D();
            }
            else if (Bcollider.enabled && !PlayerStat.instance.Trans3D)
            {
                PlatformChange2D();
            }
        }
        else
        {
            if (Bcollider.enabled && PlayerStat.instance.Trans3D)
            {
                PlatformChange3D();
            }
            else if (!Bcollider.enabled && !PlayerStat.instance.Trans3D)
            {
                PlatformChange2D();
            }
        }
    }
}
