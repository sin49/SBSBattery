using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveByDimensionWithPlayer : PlatformMoveByDimesion
{
    Transform Player;

    public override void PlatformChange2D()
    {
        if(Player!=null)
        Player.transform.SetParent(transform);
        base.PlatformChange2D();
        if (Player != null)
            Player.transform.SetParent(null);
    }
    public override void PlatformChange3D()
    {
        if (Player != null)
            Player.transform.SetParent(transform);
        base.PlatformChange3D();
        if (Player != null)
            Player.transform.SetParent(null);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {

            Player = collision.transform;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {

            Player = null;

        }
    }
}
