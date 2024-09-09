using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveTest : MonoBehaviour
{
    Rigidbody rb;
    // Update is called once per frame
    void Update()
    {
        rb=GetComponent<Rigidbody>();
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = Vector3.back;
        }
        else
            rb.velocity = Vector3.zero;
    }
}
