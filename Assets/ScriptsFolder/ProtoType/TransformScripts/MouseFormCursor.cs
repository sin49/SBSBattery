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
    public GameObject interactObj;
    public float forwardThrowForce;
    public float upThrowForce;

    private void Update()
    {
        if (interactObj != null)
        {
            interactObj.transform.position = cursorParent.transform.position;
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
                    interactObj = other.gameObject;
                    interactObj.GetComponent<Rigidbody>().useGravity = false;
                    interactObj.GetComponent<Rigidbody>().isKinematic = true;
                    other.transform.position = cursorParent.transform.position;
                }
            }
        }
    }

    public void InteractTypeCheck()
    {
        Debug.Log("üũ�Ϸ� ���� ���Խ��ϴ�");
        CursorInteractObject cursorInteract;
        if (interactObj != null && interactObj.TryGetComponent<CursorInteractObject>(out cursorInteract))
        {
            Enemy enemy;
            if (interactObj.TryGetComponent<Enemy>(out enemy))
            {
                Debug.Log("���� �����̽ó׿�");
                ThrowMonster();
            }
            else
            {
                Debug.Log("�÷��� �����̽ó׿�");
                DropPlatformObject();
            }
        }
    }

    public void ThrowMonster()
    {
        Enemy enemy;
        if (interactObj.TryGetComponent<Enemy>(out enemy))
        {
            enemy.GetComponent<Rigidbody>().useGravity = true;
            enemy.GetComponent<Rigidbody>().isKinematic = false;
            enemy.GetComponent<Rigidbody>().AddForce(transform.forward * forwardThrowForce + transform.up * upThrowForce, ForceMode.VelocityChange);
            interactObj = null;
            onCatch = false;
        }
    }

    public void DropPlatformObject()
    {
        interactObj.GetComponent<Rigidbody>().useGravity = true;
        interactObj.GetComponent<Rigidbody>().isKinematic = false;
        interactObj = null;
        onCatch = false;
    }
}
