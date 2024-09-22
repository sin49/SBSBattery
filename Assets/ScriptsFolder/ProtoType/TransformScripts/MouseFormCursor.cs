using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MouseFormCursor : MonoBehaviour
{
    public bool onCatch;
    public GameObject cursorParent;
    public CursorInteractObject interactObj;
    public float forwardThrowForce;
    public float upThrowForce;

    private void Update()
    {
        if (interactObj != null)
        {
            interactObj.transform.position = cursorParent.transform.position + transform.forward * interactObj.ColliderEndPoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("InteractivePlatform"))
        {
            if (!onCatch)
            {
                CursorInteractObject cursorInteract;
                if(other.TryGetComponent<CursorInteractObject>(out cursorInteract))
                {
                    onCatch = true;
                    interactObj = cursorInteract;
                    interactObj.GetComponent<Rigidbody>().useGravity = false;
                    interactObj.GetComponent<Rigidbody>().isKinematic = true;
                    interactObj.GetComponent<Collider>().isTrigger = true;
                    other.transform.position = cursorParent.transform.position;
                    other.transform.rotation = Quaternion.identity;

                    if (cursorInteract.CompareTag("InteractivePlatform"))
                    {
                        cursorInteract.gameObject.layer = LayerMask.NameToLayer("DontMoveIgnore");
                    }
                }
            }
        }
    }

    public void InteractTypeCheck()
    {
        Debug.Log("Ã¼Å©ÇÏ·¯ ¸¶½Ç ³ª¿Ô½À´Ï´Ù");
        CursorInteractObject cursorInteract;
        if (interactObj != null && interactObj.TryGetComponent<CursorInteractObject>(out cursorInteract))
        {
            Enemy enemy;
            if (interactObj.TryGetComponent<Enemy>(out enemy))
            {
                Debug.Log("¸ó½ºÅÍ °í°´´ÔÀÌ½Ã³×¿ä");
                ThrowMonster();
            }
            else
            {
                Debug.Log("ÇÃ·§Æû °í°´´ÔÀÌ½Ã³×¿ä");
                DropPlatformObject();
            }
        }
    }

    public void ThrowMonster()
    {
        Enemy enemy;
        if (interactObj.TryGetComponent<Enemy>(out enemy))
        {
            enemy.GetComponent<Collider>().isTrigger = false;
            enemy.GetComponent<Rigidbody>().useGravity = true;
            enemy.GetComponent<Rigidbody>().isKinematic = false;
            enemy.GetComponent<Rigidbody>().AddForce(transform.forward * forwardThrowForce + transform.up * upThrowForce, ForceMode.VelocityChange);
            interactObj = null;
            onCatch = false;
        }
    }

    public void DropPlatformObject()
    {
        interactObj.gameObject.layer = LayerMask.NameToLayer("Default");
        interactObj.GetComponent<Collider>().isTrigger = false;
        interactObj.GetComponent<Rigidbody>().useGravity = true;
        interactObj.GetComponent<Rigidbody>().isKinematic = false;
        interactObj = null;
        onCatch = false;
    }
}
