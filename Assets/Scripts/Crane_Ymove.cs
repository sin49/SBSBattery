using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane_Ymove : Crane
{

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
    public override void MoveCrane(Vector3 vector,Vector3 Target,Transform origin)
    {
        if (vector.y > 0)
        {
            if (origin.position.y < Target.y)
            {
                origin.Translate(vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.y >= Target.y)
                    StopMove(origin,Target);
            }
           
        }else if(vector.y < 0)
        {
            if (origin.position.y > Target.y)
            {
                origin.Translate(vector * CraneSpeed * Time.fixedDeltaTime);
                if (origin.position.y <= Target.y)
                    StopMove(origin, Target);
            }
        }

            
    }

    
}
