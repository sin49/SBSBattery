using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrabolaObject : MonoBehaviour
{
    public float v; // ����ü�� �ӵ�
    public float rad; // ����ü�� ����
    public float maxH; // ������ ��� ������ �ְ� ����
    public Vector3 distance; // ������ġ�� ��ǥ ��ġ ������ �Ÿ�?
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
