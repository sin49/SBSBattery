using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public float movespeed;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (transform.position.x > 30)
        {
            transform.Translate(Vector3.forward * (transform.position.x - 30));
        }else if(transform.position.x < 15)
        {
            transform.Translate(Vector3.back * (-transform.position.x + 15));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.back * movespeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.forward * movespeed * Time.deltaTime);
        }
    }
    
}
