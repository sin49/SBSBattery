using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyRemoveOutputEvent : OutputEvent
{
    public GameObject selectEnemy;
    public override void output()
    {
        throw new System.NotImplementedException();
    }

    public void EnemySelectAndDelete()
    {
        if (selectEnemy != null)
        {
            Destroy(selectEnemy);
            selectEnemy = null;
        }
        else
        {
            Debug.Log("적 제거 출력이벤트의 적이 지정되어있지 않습니다.");
        }
    }
}
