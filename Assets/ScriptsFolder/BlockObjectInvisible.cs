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
                    // ó������ �߰ߵ� ���� ������Ʈ�� �����ϰ� ����ϴ�
                    obj.ChangeTransparency(true);
                }
            }
        }

        // ���� ���� ���·� ǥ�õ� ������Ʈ�� ����Ʈ�� ������ �ٽ� �������ϰ� ����ϴ�
        foreach (TransparentObj obj in transparentObjects)
        {
            if (!currentTransparentObjects.Contains(obj))
            {
                obj.ChangeTransparency(false);
            }
        }

        // ���� ���� ���·� ǥ�õ� ������Ʈ ��� ������Ʈ
        transparentObjects = currentTransparentObjects;
    }
}
