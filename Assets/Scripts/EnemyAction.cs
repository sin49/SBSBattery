using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public abstract class EnemyAction : MonoBehaviour
{
    protected event Action ActionStartHandler;
    protected event Action ActionEndHandler;//�ൿ�� ������ �� �̺�Ʈ
 
    [Header("�׼� ���� Ȯ��")]
    public int Possibility = 1;
    public float ActionLifeTIme;
    public void registerActionStartHandler(Action a)
    {
        ActionStartHandler += a;
    }
    public void registerActionHandler(Action a)//�ൿ �������� �̺�Ʈ ���
    {
        ActionEndHandler += a;
    }
    public virtual void Invoke( Transform target = null)//�ൿ ����
    {
        ActionStartHandler?.Invoke();
        StartCoroutine(DisableAction(ActionLifeTIme));
    }
    public virtual void StopAction()
    {
       
        CancelActionEvent();
    }
    protected virtual void CancelActionEvent()
    {
        StopAllCoroutines();
        ActionEndHandler?.Invoke();
        ActionEndHandler = null;
    }
    public virtual void Invoke(Action ActionENd,  Transform target = null)//�ൿ ����(�ൿ ������ �� �̺�Ʈ ����)
    {

        ActionStartHandler?.Invoke();

        registerActionHandler(ActionENd);

        StartCoroutine(DisableAction(ActionLifeTIme));
    }

    protected IEnumerator DisableAction(float lifetime)//�ൿ�� ����
    {
 
        yield return new WaitForSeconds(lifetime);

        ActionEndHandler?.Invoke();
        ActionEndHandler = null;
    }

}


