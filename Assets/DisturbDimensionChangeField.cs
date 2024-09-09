using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DisturbDimensionMoveDirection { left,right,up,down ,forward,back}
//[ExecuteAlways]
public class DisturbDimensionChangeField : MonoBehaviour, colliderDisplayer
{
    [Header("기존의 전환을 막는 옵션(끄면 순간이동 옵션)")]
    public bool restrictBool;
    [Header("전환 금지 옵션")]
    public bool RestirctDimension;

    public DisturbDimensionMoveDirection movedirection;
    public Renderer renderer_;
    Collider col;
    public void ActiveColliderDisplay()
    {
        if(renderer_!=null) 
        renderer_.enabled = true;
    }

    public void DeactiveColliderDisplay()
    {
        if (renderer_ != null)
            renderer_.enabled = false;
    }

    public void registerColliderDIsplay()
    {
        if (renderer_ != null)
            ColliderDisplayManager.Instance.register(this);
    }
    private void Start()
    {
        if (renderer_ != null)
            registerColliderDIsplay();
        col = (GetComponent<Collider>());
    }

    private void Update()
    {
        if ((int)PlayerStat.instance.MoveState >= 4)
            col.enabled = true;
        else
            col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if(restrictBool)
            PlayerHandler.instance.DImensionChangeDisturb = RestirctDimension;
            else
                  if (!restrictBool && (int)PlayerStat.instance.MoveState >= 4)
            {
                Transform t = PlayerHandler.instance.CurrentPlayer.transform;

                Vector3 dir = Vector3.zero;
                switch (movedirection)
                {
                    case DisturbDimensionMoveDirection.left:
                        dir = Vector3.left; break;
                    case DisturbDimensionMoveDirection.right: dir = Vector3.right; break;
                    case DisturbDimensionMoveDirection.up: dir = Vector3.up; break;
                    case DisturbDimensionMoveDirection.down: dir = Vector3.down; break;
                    case DisturbDimensionMoveDirection.forward: dir = Vector3.forward; break;
                    case DisturbDimensionMoveDirection.back: dir = Vector3.back; break;
                }
                float distance = powerVec3(dir,(col.bounds.size + other.bounds.size) / 2).magnitude;

                t.transform.Translate(dir * distance);
            }
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
          


    //    }
    //}
    Vector3 powerVec3(Vector3 v1,Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

}
