using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane_Ymove : Crane
{
    protected override bool StopMove(Transform origin, Vector3 Target)
    {
        origin.position = new Vector3(origin.position.x,Target.y, origin.position.z);
        return base.StopMove(origin, Target);
    }
    public override Vector3 GetMoveVector(Vector3 Target,Vector3 origin)
    {
        float f = (Target - origin).y;
        if (f > 0)
            return Vector3.up;
        else if (f < 0)
        {
            return Vector3.down;
        }
        else
            return Vector3.zero;
    }
    public override bool MoveCrane(Vector3 vector,Vector3 Target,Transform origin)
    {
        if (vector.y > 0)
        {
            if (origin.position.y < Target.y)
            {
                origin.Translate(vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.y >= Target.y)
                  return  StopMove(origin,Target);
                else
                    return true;
            }
           
        }else if(vector.y < 0)
        {
            if (origin.position.y > Target.y)
            {
                origin.Translate(vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.y <= Target.y)
                    return StopMove(origin, Target);
                else
                    return true;
            }
        }
        return false;
            
    }

    
}
