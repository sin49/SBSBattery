using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CraneZMOVE {back=-1,forward=1 }
public class Crane_Xmove : Crane
{
    public CraneZMOVE CraneMOveDirection;
    public override Vector3 GetMoveVector(Vector3 Target, Vector3 origin)
    {
        float f = 0;

            f = -1* (Target - origin).z;
      
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
         
            if (origin.position.z< Target.z)
            {
                origin.Translate((int)CraneMOveDirection * vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.z >= Target.z)
                    StopMove(origin, Target);
            }

        }
        else if (vector.x < 0)
        {
            if (origin.position.z > Target.z)
            {
                origin.Translate((int)CraneMOveDirection * vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.z <= Target.z)
                    StopMove(origin, Target);
            }
        }
    }
}
