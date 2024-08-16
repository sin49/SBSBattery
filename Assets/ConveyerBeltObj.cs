using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBeltObj : MonoBehaviour
{
   public ConvayerBelt belt;
    [Header("��Ʈ ���ǵ�")]
    public float beltSpeed;
    [Header("��Ʈ ����")]
    public Vector3 beltDirection;
    [Header("��Ʈ ���ϸ��̼� �ݴ��")]
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
