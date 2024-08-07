using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlatformMoveByDimesion : MonoBehaviour
{
    public float Zmove = 1;

    private void Start()
    {
        PlayerHandler.instance.RegisterChange3DEvent(changePlatform);
    }
    public virtual void PlatformChange3D()
    {

        transform.Translate(Vector3.back * Zmove);
    }
 public virtual  void PlatformChange2D()
{

    transform.Translate(Vector3.forward * Zmove);
}
    void changePlatform()
    {
        if (PlayerStat.instance.Trans3D)
        {
            PlatformChange3D();
        }
        else if (!PlayerStat.instance.Trans3D)
        {
            PlatformChange2D();
        }
    }

}
