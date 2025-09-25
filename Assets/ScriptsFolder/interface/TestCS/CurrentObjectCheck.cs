using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentObjectCheck : MonoBehaviour
{
    TutorialCallEvent callEvent;

    private void Awake()
    {
        callEvent= GetComponent<TutorialCallEvent>();
    }
}
