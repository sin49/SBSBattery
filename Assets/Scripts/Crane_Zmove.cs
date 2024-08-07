using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane_Zmove : Crane
{
    public override Vector3 GetMoveVector(Vector3 Target, Vector3 origin)
    {
        float f = (Target - origin).z;
        if (f > 0)
            return Vector3.forward;
        else if (f < 0)
        {
            return Vector3.back;
        }
        else
            return Vector3.zero;
    }
    public override void MoveCrane(Vector3 vector, Vector3 Target, Transform origin)
    {
        if (vector.z > 0)
        {
            if (origin.position.z < Target.z)
            {
                origin.Translate(vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.z >= Target.z)
                    origin.position = new Vector3(origin.position.x, origin.position.y, Target.z);
            }

        }
        else if (vector.z < 0)
        {
            if (origin.position.z > Target.z)
            {
                origin.Translate(vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.z <= Target.z)
                    origin.position = new Vector3(origin.position.x, origin.position.y, Target.z);
            }
        }
    }
}
