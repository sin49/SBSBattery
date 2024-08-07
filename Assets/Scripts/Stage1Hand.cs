using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Hand : MonoBehaviour
{
    public bool Bossawake;
    Vector3 OnePosition;

    public event Action ActionEnd;


    public Transform SweaperStartTransform;
    public Transform SweaperEndTransform;
    public float SweaperStartMoveTime;
    public float sweaperwaitTime;
    public float SweaperEndMoveTime;
    public float sweaperReturnTime;
    float sweapertimer;
  

    private void Start()
    {
        OnePosition = transform.position;
    }
    public Tuple<Vector3,float> calculateSweapvector(Vector3 position1,Vector3 postion2,float time)
    {
        Vector3 vec = position1 - postion2;
        float distance = vec.magnitude;
        float speed = distance / time;

        return new Tuple<Vector3, float>(vec, speed);
    }
    public IEnumerator Sweaper()
    {
      
     var tuple=   calculateSweapvector(SweaperStartTransform.position, transform.position, SweaperStartMoveTime);
        Vector3 vec = tuple.Item1;
        float speed = tuple.Item2;
        while (sweapertimer<= SweaperStartMoveTime)
        {
            transform.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        yield return new WaitForSeconds(sweaperwaitTime);
   
        tuple = calculateSweapvector(SweaperEndTransform.position, transform.position, SweaperEndMoveTime);
        vec = tuple.Item1;
        speed = tuple.Item2;
        while (sweapertimer <= SweaperEndMoveTime)
        {
            transform.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        tuple = calculateSweapvector(OnePosition, transform.position, sweaperReturnTime);
        vec = tuple.Item1;
        speed = tuple.Item2;
        while (sweapertimer <= sweaperReturnTime)
        {
            transform.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        transform.position = OnePosition;
        ActionEnd?.Invoke();
    }
}
