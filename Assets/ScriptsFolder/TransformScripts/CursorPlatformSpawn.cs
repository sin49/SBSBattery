using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPlatformSpawn : MonoBehaviour
{
    public GameObject platformPrefab;

    public GameObject platform;

    private void Update()
    {
        if (platform == null)
        {
             platform = Instantiate(platformPrefab, transform.position, Quaternion.identity).gameObject;            
        }
    }
}
