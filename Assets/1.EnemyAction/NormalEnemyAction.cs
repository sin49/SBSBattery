using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyAction : EnemyAction
{
    //enemy ��ũ��Ʈ�� ������� �ʿ伺�� ������ ���
    public Enemy e;

    public virtual void register(Enemy e)
    {
        this.e = e;
    }
    public override void Invoke(Transform target = null)
    {
        base.Invoke(target);
        registerActionHandler(e.DelayTime);
        registerActionHandler(e.InitAttackCoolTime);
    }
}


