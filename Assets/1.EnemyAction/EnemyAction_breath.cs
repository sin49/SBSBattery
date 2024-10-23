using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//�ڷ�ƾ ���빰�� ������� �ڵ�� ���� �� �� attackcollider�� ��ƼŬ+�ؼ� ���̱�
//�ּ� ó���Ѱ� �����
public class EnemyAction_breath : NormalEnemyAction
{

    firebreathattack breathattack;

    [Header("�극�� �ִ� ����")]
    public Vector3 breathsize;
    [Header("�극�� �ʱ� ����")]
    public Vector3 breathinitsize;

    [Header("�극�� ���� �ð�")]
    public float breathtime;
    [Header("�극�� ������ �ִ밡 �Ǵ� �ð�")]
    public float breathspreadmaxtime;
    [Header("�극�� ������ ������� �ð�")]
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
