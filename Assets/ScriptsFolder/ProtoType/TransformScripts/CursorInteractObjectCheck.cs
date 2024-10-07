using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInteractObjectCheck : MonoBehaviour
{
    public GameObject cursorParent;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            float value = (cursorParent.transform.position - PlayerHandler.instance.CurrentPlayer.transform.GetChild(0).position).magnitude;
            Debug.Log(value);
            if (value < 1.02f)
            {
                return;
            }
            cursorParent.transform.position -= (cursorParent.transform.forward * 0.1f);
        }
    }
}
