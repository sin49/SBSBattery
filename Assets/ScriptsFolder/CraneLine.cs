
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;

public class CraneLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    [Range(0,1.0f)]
    public float Line;


    public Transform CraneObject;
    public Transform CranePlatform;
    private void Awake()
    {
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.startWidth = Line;
        lineRenderer.endWidth = Line;
        lineRenderer.SetPosition(0, CraneObject.transform.position);
        lineRenderer.SetPosition(1, CranePlatform.transform.position);
    }
}
