using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableActiveObject : MonoBehaviour
{
    public GameObject obj;

    private void Start()
    {
        if (obj != null)
            obj.SetActive(false);
    }

    private void OnDisable()
    {
        obj.SetActive(true);
    }
}
