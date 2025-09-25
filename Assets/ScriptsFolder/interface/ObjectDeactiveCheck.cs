using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDeactiveCheck : MonoBehaviour
{
    public TransformPlace tp;
    bool same;

    // Update is called once per frame
    void Update()
    {

        Debug.Log("계속 불리는지?");
        if (!same)
        {
            if (PlayerHandler.instance != null)
            {
                if (PlayerHandler.instance.CurrentType == tp.type)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
