using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public abstract class EnemyAction : MonoBehaviour
{
    protected event Action ActionStartHandler;
    protected event Action ActionEndHandler;//행동이 끝났을 때 이벤트
 
    [Header("액션 실행 확률")]
    public int Possibility = 1;
    public float ActionLifeTIme;
    public void registerActionStartHandler(Action a)
    {
        ActionStartHandler += a;
    }
    public void registerActionHandler(Action a)//행동 끝났을때 이벤트 등록
    {
        ActionEndHandler += a;
    }
    public virtual void Invoke( Transform target = null)//행동 실행
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
    public virtual void Invoke(Action ActionENd,  Transform target = null)//행동 실행(행동 끝났을 때 이벤트 포함)
    {

        ActionStartHandler?.Invoke();

        registerActionHandler(ActionENd);

        StartCoroutine(DisableAction(ActionLifeTIme));
    }

    protected IEnumerator DisableAction(float lifetime)//행동이 끝남
    {
 
        yield return new WaitForSeconds(lifetime);

        ActionEndHandler?.Invoke();
        ActionEndHandler = null;
    }

}


