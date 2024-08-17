using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Crane_XDIrection { Left=1,right=-1}
public class Crane_Zmove : Crane
{
    public Crane_XDIrection CraneMOveDirection;
    public override Vector3 GetMoveVector(Vector3 Target, Vector3 origin)
    {
        float f = 0;

        f = -1 * (Target - origin).x;

        if (f > 0)
            return Vector3.left;
        else if (f < 0)
        {
            return Vector3.right;
        }
        else
            return Vector3.zero;
    }
    protected override bool StopMove(Transform origin, Vector3 Target)
    {
        origin.position = new Vector3(origin.position.x, origin.position.y, Target.z);
        return base.StopMove(origin, Target);
    }
    public override bool MoveCrane(Vector3 vector, Vector3 Target, Transform origin)
    {

        if (vector.x > 0)
        {

            if (origin.position.x < Target.x)
            {
   
                origin.Translate((int)CraneMOveDirection * vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.x >= Target.x)
                    return StopMove(origin, Target);
                else
                    return true;
            }

        }
        else if (vector.x < 0)
        {
            if (origin.position.x> Target.x)
            {
   
                origin.Translate((int)CraneMOveDirection * vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.x <= Target.x)
                    return StopMove(origin, Target);
                else
                    return true;
            }
        }
        return false;
    }
}
