using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PlayerScaler : MonoBehaviour
//{
//    private Transform parentPlatform;
//    private Vector3 originalPlayerScale;
//    private Vector3 originalLocalScale;

//    void Start()
//    {
//        originalPlayerScale = transform.localScale;
//    }

//    public void SetParentPlatform(Transform platform)
//    {
//        parentPlatform = platform;
//        originalLocalScale = transform.localScale;
//        transform.SetParent(platform);
//        UpdateScale();
//    }

//    public void ClearParentPlatform()
//    {
//        transform.SetParent(null);
//        transform.localScale = originalPlayerScale;
//        parentPlatform = null;
//    }

//    void Update()
//    {
//        if (parentPlatform != null)
//        {
//            UpdateScale();
//        }
//    }

//    void UpdateScale()
//    {
//        Vector3 inversePlatformScale = new Vector3(
//            1f / parentPlatform.lossyScale.x,
//            1f / parentPlatform.lossyScale.y,
//            1f / parentPlatform.lossyScale.z
//        );

//        Vector3 newScale = Vector3.Scale(originalLocalScale, inversePlatformScale);
//        Quaternion playerrotation = transform.rotation;


//        Vector3 forward = playerrotation * Vector3.forward;
//        Vector3 right = playerrotation * Vector3.right;

//        forward = Vector3.Scale(forward, inversePlatformScale);
//        right = Vector3.Scale(right , inversePlatformScale);

//        float scaleX = newScale.x * Mathf.Abs(Vector3.Dot(Vector3.right, right)) + newScale.z * Mathf.Abs(Vector3.Dot(Vector3.right, forward));
//        float scaleZ = newScale.x * Mathf.Abs(Vector3.Dot(Vector3.forward, right)) + newScale.z * Mathf.Abs(Vector3.Dot(Vector3.forward, forward));

//        transform.localScale = new Vector3(scaleZ, newScale.y, scaleX);
//    }
//}
