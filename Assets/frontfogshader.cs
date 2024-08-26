using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frontfogshader : MonoBehaviour
{
    public Vector3 localtransform;
    public Vector3 localrotation;
    private void Update()
    {
        this.transform.localPosition = localtransform;
        this.transform.localRotation=Quaternion.Euler(localrotation);
    }
}
