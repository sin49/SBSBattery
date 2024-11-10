using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
    public Button button;
    [Header("0스테이지(튜토리얼)0~3\n1스테이지(배터리)4~14\n2스테이지(리모컨+마우스)15~24\n3스테이지(다리미+보스)25~30")]
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
