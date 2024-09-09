using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrackingEvent : MonoBehaviour
{
    public BoxTestt boxEnemy;

    private void Awake()
    {
        boxEnemy = transform.parent.GetComponent<BoxTestt>();
    }

    public void StopMove()
    {
        boxEnemy.tracking = false;
    }

    public void StartMove()
    {
        boxEnemy.tracking = true;
    }
}
