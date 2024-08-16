using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObjectInvisible : MonoBehaviour
{
    public Transform target;
    public LayerMask platformLayer;
    private HashSet<TransparentObj> transparentObjects = new HashSet<TransparentObj>();

    void Update()
    {
        if (PlayerHandler.instance.CurrentPlayer != null)
            target = PlayerHandler.instance.CurrentPlayer.transform;
        else
            target = null;
        if (target != null /*&& PlayerStat.instance.MoveState == PlayerMoveState.Trans3D*/)
        {
            CheckRaycast();
        }
    }
    private void Start()
    {
        PlayerHandler.instance.RegisterChange3DEvent(InitializeTransparent);
    }
    public void InitializeTransparent()
    {
        //if (!PlayerStat.instance.Trans3D)
        //{

            foreach (TransparentObj obj in transparentObjects)
            {

                obj.ChangeTransparency(false);

            }
            transparentObjects.Clear();
        //}
    }
    void CheckRaycast()
    {
        Vector3 dir = (target.position - transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir.normalized, dir.magnitude, platformLayer);


        HashSet<TransparentObj> currentTransparentObjects = new HashSet<TransparentObj>();
        foreach (RaycastHit hit in hits)
        {
            TransparentObj obj = null;
            if (hit.collider.TryGetComponent<TransparentObj>(out obj))
            {
                currentTransparentObjects.Add(obj);
                if (!transparentObjects.Contains(obj))
                {
                    // 처음으로 발견된 투명 오브젝트는 투명하게 만듭니다
                    obj.ChangeTransparency(true);
                }
            }
        }

        // 현재 투명 상태로 표시된 오브젝트가 리스트에 없으면 다시 불투명하게 만듭니다
        foreach (TransparentObj obj in transparentObjects)
        {
            if (!currentTransparentObjects.Contains(obj))
            {
                obj.ChangeTransparency(false);
            }
        }

        // 현재 투명 상태로 표시된 오브젝트 목록 업데이트
        transparentObjects = currentTransparentObjects;
    }
}
