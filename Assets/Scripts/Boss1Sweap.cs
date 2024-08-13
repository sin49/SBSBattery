using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Boss1Sweap : EnemyAction
{
    public Boss1Hand Lhand;
    Vector3 LHandPosition;
    Vector3 RHandPosition;
    public Boss1Hand RHand;

    Transform target;

   public Transform BossField;

    Vector3 fieldMin;
    Vector3 fieldMax;

    public float handsize = 1;

    public float SweaperPatternDealy;
    
    
    public float SweaperStartMoveTime;
    public float sweaperwaitTime;
    public float SweaperEndMoveTime;
    public float sweaperReturnTime;
    float sweapertimer;
    bool OnAction;
    public float SweapDistanceRandomWeight;
    protected override void CancelActionEvent()
    {
        StopAllCoroutines();
        StartCoroutine(InitializeHandPosition());
    }
    IEnumerator InitializeHandPosition()
    {
        var Ltuple = calculateSweapvector(LHandPosition, Lhand.transform.position, sweaperReturnTime);
        var Rtuple= calculateSweapvector(RHandPosition, RHand.transform.position, sweaperReturnTime);
        var Lvec = Ltuple.Item1;
        var Rvec = Rtuple.Item1;
       var speed = Ltuple.Item2;
        while (sweapertimer <= sweaperReturnTime)
        {
            Lhand.transform.Translate(Lvec.normalized * speed * Time.fixedDeltaTime);
            RHand.transform.Translate(Rvec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        Lhand.transform.position = LHandPosition;
        RHand.transform.position = RHandPosition;
        StartCoroutine(DisableAction(0));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (BossField != null)
        {
            Vector3 min = new Vector3(-0.5f, 0.5f, -0.5f);
            Vector3 max = new Vector3(0.5f, 0.5f, 0.5f);

            fieldMin = BossField.TransformPoint(min);
            fieldMax = BossField.TransformPoint(max);
            Gizmos.DrawWireCube(BossField.transform.position,
                 new(Mathf.Abs(fieldMax.x - fieldMin.x), fieldMin.y, Mathf.Abs(fieldMax.z - fieldMin.z))
                );
        }
    }

    public override void Invoke(Action ActionENd,Transform target = null)
    {
        this.target = target;
        registerActionHandler(ActionENd);
        StartCoroutine(SweaperPattern());
    }
    private void Awake()
    {
        if (BossField != null)
        {
            Vector3 min = new Vector3(-0.5f, 0.5f, -0.5f);
            Vector3 max = new Vector3(0.5f, 0.5f, 0.5f);

            fieldMin = BossField.TransformPoint(min);
            fieldMax = BossField.TransformPoint(max);

            SweapDistanceRandomWeight = Mathf.Abs(fieldMax.z - fieldMin.z);
        }

        if (Lhand != null)
            LHandPosition = Lhand.transform.position;
        if(RHand!=null)
            RHandPosition= RHand.transform.position;
    }
    public Tuple<Vector3, float> calculateSweapvector(Vector3 goal, Vector3 startpos, float time)
    {
   
        Vector3 vec = goal - startpos;
        float distance = vec.magnitude;
     
            float speed = distance / time;
      
        return new Tuple<Vector3, float>(vec, speed);
    }
    public Tuple<Vector3, float> calculateSweapvector(Vector3 goal, Vector3 startpos,float randomweight, float time)
    {
        goal = new Vector3(goal.x, goal.y, target.transform.position.z + randomweight);
        Vector3 vec = goal - startpos;
    
        float distance = vec.magnitude;
        float speed = distance / time;

        return new Tuple<Vector3, float>(vec, speed);
    }
    public IEnumerator SweaperPattern()
    {
        if (Lhand.active && RHand.active)
        {
            bool randombool = UnityEngine.Random.Range(0, 2) == 0;
            if (randombool)
            {
                yield return StartCoroutine(Sweaper(Lhand.transform, fieldMin, fieldMax));
                yield return new WaitForSeconds(SweaperPatternDealy);
                yield return StartCoroutine(Sweaper(RHand.transform, fieldMax, fieldMin));
            }
            else
            {
                yield return StartCoroutine(Sweaper(RHand.transform, fieldMax, fieldMin));
                yield return new WaitForSeconds(SweaperPatternDealy);
                yield return StartCoroutine(Sweaper(Lhand.transform, fieldMin, fieldMax));
            }
        }else if(Lhand.active && !RHand.active)
        {
            yield return StartCoroutine(Sweaper2(Lhand.transform, fieldMin, fieldMax));
        }else if(!Lhand.active && RHand.active)
        {
            yield return StartCoroutine(Sweaper2(RHand.transform, fieldMax, fieldMin));
        }
        yield return StartCoroutine(DisableAction(0.1f));
    }
    public IEnumerator Sweaper(Transform hand,Vector3 StartPos,Vector3 EndPos)
    {
        Vector3 HandOnepositon=hand.position;
        float randdistance = UnityEngine.Random.Range(0, SweapDistanceRandomWeight);
        int randomValue = UnityEngine.Random.Range(0, 2) * 2 - 1;
        randdistance *= randomValue;

        //시작지점으로 손이 감
        var tuple = calculateSweapvector(StartPos, hand.position, randdistance, SweaperStartMoveTime);
        Vector3 vec = tuple.Item1;
        vec.y += handsize * 0.5f;
        float speed = tuple.Item2;
        while (sweapertimer <= SweaperStartMoveTime)
        {
            hand.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        yield return new WaitForSeconds(sweaperwaitTime);
        //손이 휩쓸기 스타트
        tuple = calculateSweapvector(EndPos, hand.position,-randdistance, SweaperEndMoveTime);
        vec = tuple.Item1;
        vec.y = 0;
        speed = tuple.Item2;
        while (sweapertimer <= SweaperEndMoveTime)
        {
            hand.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        //OnePostion=handposition
        sweapertimer = 0;
        //손이 원위치로
        tuple = calculateSweapvector(HandOnepositon, hand.position, sweaperReturnTime);
        vec = tuple.Item1;
        speed = tuple.Item2;
        while (sweapertimer <= sweaperReturnTime)
        {
            hand.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        hand.position = HandOnepositon;

  
    }
    public IEnumerator Sweaper2(Transform hand, Vector3 StartPos, Vector3 EndPos)
    {
        Vector3 HandOnepositon = hand.position;
        float randdistance = UnityEngine.Random.Range(0, SweapDistanceRandomWeight);
        int randomValue = UnityEngine.Random.Range(0, 2) * 2 - 1;
        randdistance *= randomValue;

        //시작지점으로 손이 감
        var tuple = calculateSweapvector(StartPos, hand.position, randdistance, SweaperStartMoveTime);
        Vector3 vec = tuple.Item1;
        vec.y += handsize * 0.5f;
        float speed = tuple.Item2;
        while (sweapertimer <= SweaperStartMoveTime)
        {
            hand.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        yield return new WaitForSeconds(sweaperwaitTime);
        //손이 휩쓸기 스타트
        tuple = calculateSweapvector(EndPos, hand.position, -randdistance, SweaperEndMoveTime);
        vec = tuple.Item1;
        vec.y = 0;
        speed = tuple.Item2;
        while (sweapertimer <= SweaperEndMoveTime)
        {
            hand.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }

        sweapertimer = 0;
        //한번더
        tuple = calculateSweapvector(StartPos, hand.position, randdistance, SweaperEndMoveTime);
        vec = tuple.Item1;
        vec.y = 0;
        speed = tuple.Item2;
        while (sweapertimer <= SweaperEndMoveTime)
        {
            hand.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }

        sweapertimer = 0;
        //손이 원위치로
        tuple = calculateSweapvector(HandOnepositon, hand.position, sweaperReturnTime);
        vec = tuple.Item1;
        speed = tuple.Item2;
        while (sweapertimer <= sweaperReturnTime)
        {
            hand.Translate(vec.normalized * speed * Time.fixedDeltaTime);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        hand.position = HandOnepositon;


    }

}
