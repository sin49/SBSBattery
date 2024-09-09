using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrabolaObject : MonoBehaviour
{
    public float v; // 투사체의 속도
    public float rad; // 투사체의 각도
    public float maxH; // 포물선 계산 동안의 최고 높이
    public Vector3 distance; // 최초위치와 목표 위치 사이의 거리?
    public Vector3 gravity;

    private void Start()
    {
        //gravity = new(0, -9.81f, 0);
    }

    private void Update()
    {

    }

    public void ParabolaRange(Transform target)
    {
        distance = target.position - transform.position;


    }
}
