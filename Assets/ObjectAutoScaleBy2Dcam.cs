using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAutoScaleBy2Dcam : MonoBehaviour
{
    Camera c;
    Vector3 BasicScale;
    Vector3 calculateScale;
    float basicdistance;
    float distance;
    public float speed;

    private void Start()
    {
        BasicScale = transform.localScale;
    }
    void Update()
    {
        if (c == null && c != PlayerHandler.instance.CurrentCamera)
        {
            c = PlayerHandler.instance.CurrentCamera;
            basicdistance = Mathf.Abs( c.transform.position.z - transform.position.z);
        }
        if (c != null)
        {
            if (PlayerStat.instance.MoveState!=PlayerMoveState.Trans3D&&c.orthographic)
            {
                distance = Mathf.Abs(c.transform.position.z - transform.position.z);
                calculateScale = BasicScale - ((distance - basicdistance) * speed *Time.fixedDeltaTime * BasicScale);
                transform.localScale = calculateScale;
            }
            else
            {
                transform.localScale = BasicScale;
            }
        }
    }
}
