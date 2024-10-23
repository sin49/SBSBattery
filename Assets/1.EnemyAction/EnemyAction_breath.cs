using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//코루틴 내용물을 끄집어내서 코드로 따로 판 후 attackcollider에 파티클+해서 붙이기
//주석 처리한거 다잡기
public class EnemyAction_breath : NormalEnemyAction
{

    firebreathattack breathattack;

    [Header("브레스 최대 범위")]
    public Vector3 breathsize;
    [Header("브레스 초기 범위")]
    public Vector3 breathinitsize;

    [Header("브레스 지속 시간")]
    public float breathtime;
    [Header("브레스 범위가 최대가 되는 시간")]
    public float breathspreadmaxtime;
    [Header("브레스 범위가 사라지는 시간")]
    public float breathendtime;

    public override void register(Enemy e)
    {
        base.register(e);
        breathattack = e.attackCollider.GetComponent<firebreathattack>();
        breathattack.breathtime = breathtime;
        breathattack.breathspreadmaxtime = breathspreadmaxtime;
        breathattack.breathendtime = breathendtime;
        breathattack.registerbreathendevent(DisableActionMethod);
    }

    IEnumerator breathattackInvoke()
    {
        breathattack.gameObject.SetActive(true);
        yield return new WaitForSeconds(breathtime+breathendtime);
        DisableActionMethod();
    }

    public override void Invoke(Transform target = null)
    {
        base.Invoke(target);
        StartCoroutine(breathattackInvoke());
    }
  
   
}
