using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePlayer : MonoBehaviour,FormInterface, canthandle, EventHandle
{
    //public event Action ShootingStart;


    //private void OnEnable()
    //{
    //    ShootingStart?.Invoke();

    //}
    //private void OnDisable()
    //{
    //    ShootingStart = null;
    //}

    public void GetEvent(Action a)
    {
      
        StartCoroutine(cor(a));
        //ShootingStart += a;
    }
   IEnumerator cor(Action a)
    {
        yield return new WaitForSeconds(0.5f);
        a.Invoke();
    }
}
public interface canthandle
{
}
public interface EventHandle
{
    public void GetEvent(Action a);
}