using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInteractObjectCheck : MonoBehaviour
{
    public GameObject cursorParent;

    //private void Update()
    //{
    //    RaycastHit forwardRay;
    //    RaycastHit rightRay;
    //    RaycastHit leftRay;
    //    if (Physics.Raycast(transform.position, Vector3.forward, out forwardRay, 3f,1<< 7))
    //    {
    //        if (forwardRay.collider.CompareTag("Ground"))
    //            cursorParent.transform.Translate(3f * -Vector3.forward * Time.deltaTime, Space.World);

    //        Debug.Log(forwardRay.collider.name);
    //    }
    //    if (Physics.Raycast(transform.position, Vector3.right, out rightRay, 3f, 1 << 7))
    //    {
    //        if (rightRay.collider.CompareTag("Ground"))
    //            cursorParent.transform.Translate(3f * -Vector3.right * Time.deltaTime, Space.World);

    //        Debug.Log(rightRay.collider.name);
    //    }
    //    if (Physics.Raycast(transform.position, Vector3.left, out leftRay, 3f, 1 << 7))
    //    {
    //        if (leftRay.collider.CompareTag("Ground"))
    //            cursorParent.transform.Translate(3f * -Vector3.left * Time.deltaTime,Space.World);

    //        Debug.Log(leftRay.collider.name);
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {

            Vector3 v = (other.transform.position - PlayerHandler.instance.CurrentPlayer.transform.position).normalized;
            //if (v.x > v.z)
            //    v.z = 0;
            //else
            //    v.x = 0;
            v.y = 0;
            cursorParent.transform.Translate(v*-1, Space.World);
        }
    }
    //´ê¾ÒÀ» ¶§ 
}
