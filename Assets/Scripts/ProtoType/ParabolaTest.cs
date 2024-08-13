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

        // m*k*g*h = m*v^2/2 (단, k == gravityScale) <= 역학적 에너지 보존 법칙 적용
        float v_y = Mathf.Sqrt(2 * -Physics.gravity.y * maxHeightDisplacement.y);
        // 포물선 운동 법칙 적용
        float v_x = maxHeightDisplacement.x * v_y / (2 * maxHeightDisplacement.y);

        Vector3 force = rigid.mass * (new Vector3(v_x, v_y, 0) - rigid.velocity);
        rigid.AddForce(force, ForceMode.Impulse);
    }
}
