using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum enemyactiontype { none,swing}
public class EnemyAttackHandler : MonoBehaviour
{

    //public List<EnemyAction> EnemyActionList=new List<EnemyAction>();
    public enemyactiontype type;
    public Transform createtransform;
    public EnemyAction mainaction;
    public Enemy e;
   
    public void createaction()
    {
        if(createtransform==null)
            createtransform = transform;
        switch (type)
        {
            case enemyactiontype.swing:
                mainaction = createtransform.gameObject.AddComponent<EnemyAction_Swing>();
                break;
        }
    }
    public void invokemainaction()
    {
        Debug.Log($"적{gameObject.name} 액션 호출");
        mainaction.Invoke(e.InitAttackCoolTime,e.target);
    }
   

}
