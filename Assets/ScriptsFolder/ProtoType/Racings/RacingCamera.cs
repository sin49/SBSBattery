using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingCamera : MonoBehaviour
{
    public GameObject Position
        ;
    public Vector3 campos;
    private void Start()
    {
       
        this.gameObject.transform.position = Position.transform.position + campos;
    }
    
}
