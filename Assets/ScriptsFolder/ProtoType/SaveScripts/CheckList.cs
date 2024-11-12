using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
    public Button button;
    [Header("Tutorial스테이지(튜토리얼)0~3\n1-1스테이지(이전1-1)4~8\n1-2스테이지(마우스)9~13\n1-3스테이지(다리미)14~16\n1-4스테이지(이전1-2)17~22\n1-5스테이지(리모컨)23~26\n1-6스테이지(보스)27")]
    public int checkStageIndex;

    private void OnEnable()
    {
        StageButton();
    }

    public void StageButton()
    {
        if (CheckPointManager.instance != null)
        {
            CheckPointManager cm = CheckPointManager.instance;
            for(int i = 0; i < cm.checkPoints.Count; i++)
            {
                if (cm.checkPoints[i] == checkStageIndex)
                {
                    button.interactable = true;
                    return;
                }
            }

            button.interactable = false;
        }
    }
}
