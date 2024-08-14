using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaTest : MonoBehaviour
{
    Rigidbody rigid;
    public float maxValue;
    public Vector3 maxHeightDisplacement;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        JumpForce(maxHeightDisplacement);
    }


    private void JumpForce(Vector3 maxHeightDisplacement)
    {
        Rigidbody rigid = this.rigid;

        // m*k*g*h = m*v^2/2 (��, k == gravityScale) <= ������ ������ ���� ��Ģ ����
        float v_y = Mathf.Sqrt(2 * -Physics.gravity.y * maxHeightDisplacement.y);
        // ������ � ��Ģ ����
        float v_x = maxHeightDisplacement.x * v_y / (2 * maxHeightDisplacement.y);

        Vector3 force = rigid.mass * (new Vector3(v_x, v_y, 0) - rigid.velocity);
        rigid.AddForce(force, ForceMode.Impulse);
    }
}
