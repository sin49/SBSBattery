using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    public Camera camera_;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (camera_.gameObject.activeSelf)
                camera_.gameObject.SetActive(false);
            else
                camera_.gameObject.SetActive(true);
        }
    }
}
