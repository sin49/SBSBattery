using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class folder : MonoBehaviour
{
    public folderPortal portal;
    public Transform enemylist;

    private void Awake()
    {
        portal.gameObject.SetActive(false);
    }
    public void activeportal()
    {
        portal.gameObject.SetActive(true);
    }
}
