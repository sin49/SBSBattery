using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCenterPointTest : MonoBehaviour
{
    public Vector3 GetCalculateCenterPos()
    {
        Vector3 TotalPosition = Vector3.zero;
        foreach(Transform child in transform)
        {
            TotalPosition += child.position;
        }
        Vector3 Center = TotalPosition / transform.childCount;

        return Center;

    }
    void Start()
    {
        Debug.Log("이 밉의 중심점" + GetCalculateCenterPos());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
