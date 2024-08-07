using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlatformMoveByDimesion : MonoBehaviour
{
    public float Zmove = 1;
    public bool Move;

    private void Start()
    {
        PlayerHandler.instance.RegisterChange3DEvent(changePlatform);
    }
    public virtual void PlatformChange3D()
    {

        transform.Translate(Vector3.back * Zmove);
        Move = true;
    }
 public virtual  void PlatformChange2D()
{

    transform.Translate(Vector3.forward * Zmove);
        Move = false;
}
    void changePlatform()
    {
        if (!Move)
        {
            PlatformChange3D();
        }
        else if (Move)
        {
            PlatformChange2D();
        }
    }

}
