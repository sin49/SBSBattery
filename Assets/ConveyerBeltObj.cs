using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBeltObj : MonoBehaviour
{
   public ConvayerBelt belt;
    [Header("벨트 스피드")]
    public float beltSpeed;
    [Header("벨트 방향")]
    public Vector3 beltDirection;
    [Header("벨트 에니메이션 반대로")]
    public bool ReverseScroll;
    public MeshRenderer beltrenderer;
    Material beltmaterial;
    private void Start()
    {
        beltmaterial = beltrenderer.material;
    }
    private void FixedUpdate()
    {
        belt.conveyorSpeed = beltSpeed;
        belt.conveyorDirection = beltDirection;
        if(!ReverseScroll)
        beltmaterial.SetFloat("_ScrollSpeed", beltSpeed*  Time.timeScale);
        else
            beltmaterial.SetFloat("_ScrollSpeed", beltSpeed *-1* Time.timeScale);
    }
  
}
