using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class TransformSaver : MonoBehaviour
{
    public List<Transform> transformlist=new List<Transform>();
    List<TransformData> transformDatas = new List<TransformData>();
    private const string folderPath = "Assets/TransformSave/";

#if UNITY_EDITOR

    private void Awake()
    {
        DeleteTransformdata();
    }
    public void SaveTransform()
    {

        for (int n = 0; n < transformlist.Count; n++)
        {
            TransformData t = ScriptableObject.CreateInstance<TransformData>();
            t.position = transformlist[n].position;
            t.rotation = transformlist[n].rotation;
            t.scale = transformlist[n].localScale;
 
            UnityEditor.AssetDatabase.CreateAsset(t, folderPath + $"transformsave{n}.asset");
            if (transformDatas.Count <= n)
                transformDatas.Add(t);
            else
                transformDatas[n] = t;
        }


        UnityEditor.AssetDatabase.SaveAssets();
    }
    public void DeleteTransformdata()
    {
        for(int n = transformDatas.Count; n >= 0; n--)
        {
            UnityEditor.AssetDatabase.DeleteAsset(folderPath + $"transformsave{n}.asset");
        }
        UnityEditor.AssetDatabase.SaveAssets();
    }
    public void LoadTransform()
    {
     
        for (int n = 0; n < transformDatas.Count; n++)
        {
            TransformData t = transformDatas[n];
            if (transformlist.Count <= n)
                break;
            transformlist[n].position = t.position;
            transformlist[n].rotation = t.rotation;
            transformlist[n].localScale = t.scale;
        }
        DeleteTransformdata();
    }
#endif
}
