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
    public override void MoveCrane(Vector3 vector, Vector3 Target, Transform origin)
    {

        if (vector.x > 0)
        {

            if (origin.position.x < Target.x)
            {
   
                origin.Translate((int)CraneMOveDirection * vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.x >= Target.x)
                    origin.position = new Vector3(Target.x, origin.position.y, Target.z);
            }

        }
        else if (vector.x < 0)
        {
            if (origin.position.x> Target.x)
            {
   
                origin.Translate((int)CraneMOveDirection * vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.x <= Target.x)
                    origin.position = new Vector3(Target.x, origin.position.y, Target.z);
            }
        }
    }
}
