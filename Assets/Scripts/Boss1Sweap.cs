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
    Vector3 LhandOneRotation;
    public Transform LhandDefeatTransform;
    public Transform RhandDefeatTransform;
    Vector3 RHandPosition;
    Vector3 RhandOneRotation;
    public Boss1Hand RHand;
    Boss1Hand activehand;
    Transform target;
    [Header("보스 스테이지 바닥")]
    public Transform BossField;
    Boss1SOundManager boss1SOundManager;
    Vector3 fieldMin;
    Vector3 fieldMax;
    //[Header("휩쓸기 손 회전?")]
    //public Vector3 HandRotation;
    [Header("손 크기(손 y축 오프셋 전용)")]
    public float handsize = 1;
    [Header("휩쓸기 한 번 후 다음 휩쓸기 까지의 간격")]
    public float SweaperPatternDealy;
    [Header("테스트 중 숫자가 0이되면 고장남")]
    [Header("휩쓸기 위해 손이 휩쓸기 시작 지점까지 가는 시간")]
    public float SweaperStartMoveTime;
    [Header("시작지정에서 대기하는 시간")]
    public float sweaperwaitTime;
    [Header("시작지점에서 목표지점까지 가는 시간")]
    public float SweaperEndMoveTime;
    [Header("목표지점까지 이동 후 다시 원위치하는 시간")]
    public float sweaperReturnTime;
    
    float sweapertimer;
    bool OnAction;
    [HideInInspector]
    public float SweapDistanceRandomWeight;
    protected override void CancelActionEvent()
    {
        StopAllCoroutines();
        StartCoroutine(InitializeHandPosition());

    }
    IEnumerator InitializeHandPosition()
    {
        if (activehand == Lhand)
        {
            var Ltuple = calculateSweapvector(LhandDefeatTransform.position, Lhand.transform.position, sweaperReturnTime);
            var Lvec = Ltuple.Item1;
            var Lspeed = Ltuple.Item2;
            sweapertimer = 0;
            while (sweapertimer <= sweaperReturnTime)
            {
                Lhand.transform.Translate(Lvec.normalized * Lspeed * Time.fixedDeltaTime, Space.World);
                
                sweapertimer += Time.fixedDeltaTime;
                yield return null;
            }
            Lhand.transform.position = LhandDefeatTransform.position;
        }
        else if (activehand == RHand)
        {

            var Rtuple = calculateSweapvector(RhandDefeatTransform.position, RHand.transform.position, sweaperReturnTime);

            var Rvec = Rtuple.Item1;
            var Rspeed = Rtuple.Item2;
            sweapertimer = 0;
            while (sweapertimer <= sweaperReturnTime)
            {
              
                RHand.transform.Translate(Rvec.normalized * Rspeed * Time.fixedDeltaTime, Space.World);
                sweapertimer += Time.fixedDeltaTime;
                yield return null;
            }
            RHand.transform.position = RhandDefeatTransform.position;
        }
        sweapertimer = 0;
       
      
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
        boss1SOundManager=GetComponent<Boss1SOundManager>();
        if (BossField != null)
        {
            Vector3 min = new Vector3(-0.5f, 0.5f, -0.5f);
            Vector3 max = new Vector3(0.5f, 0.5f, 0.5f);

            fieldMin = BossField.TransformPoint(min);
            fieldMax = BossField.TransformPoint(max);

            SweapDistanceRandomWeight = Mathf.Abs(fieldMax.z - fieldMin.z);
        }

        if (Lhand != null)
        {
            LHandPosition = Lhand.transform.position;
            LhandOneRotation = Lhand.transform.rotation.eulerAngles;
        }
        if (RHand != null)
        {
            RHandPosition = RHand.transform.position;
            RhandOneRotation = RHand.transform.rotation.eulerAngles;
        }
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
        //goal = new Vector3(goal.x, goal.y, target.transform.position.z + randomweight);
        goal = new Vector3(target.transform.position.x + randomweight, goal.y, target.transform.position.z + randomweight);
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
                
                yield return StartCoroutine(Sweaper(Lhand, fieldMin, fieldMax));
                
                yield return new WaitForSeconds(SweaperPatternDealy);
                yield return StartCoroutine(Sweaper(RHand, fieldMax, fieldMin));
            }
            else
            {
                yield return StartCoroutine(Sweaper(RHand, fieldMax, fieldMin));
                yield return new WaitForSeconds(SweaperPatternDealy);
                yield return StartCoroutine(Sweaper(Lhand, fieldMin, fieldMax));
            }
        }else if(Lhand.active && !RHand.active)
        {
            yield return StartCoroutine(Sweaper2(Lhand, fieldMin, fieldMax));
        }else if(!Lhand.active && RHand.active)
        {
            yield return StartCoroutine(Sweaper2(RHand, fieldMax, fieldMin));
        }
        yield return StartCoroutine(DisableAction(0.1f));
    }
    public IEnumerator Sweaper(Boss1Hand hand,Vector3 StartPos,Vector3 EndPos)
    {
        activehand = hand;
        Transform handtransform=hand.transform;
        Vector3 HandOnepositon= handtransform.position;
        float randdistance = UnityEngine.Random.Range(0, SweapDistanceRandomWeight);
        int randomValue = UnityEngine.Random.Range(0, 2) * 2 - 1;
        randdistance *= randomValue;

        //시작지점으로 손이 감
        var tuple = calculateSweapvector(StartPos, handtransform.position, randdistance, SweaperStartMoveTime);
        Vector3 vec = tuple.Item1;
        vec.y += handsize * 0.5f;
        float speed = tuple.Item2;
        if (boss1SOundManager != null)
            boss1SOundManager.HandSwerapStartClipPlay();
        while (sweapertimer <= SweaperStartMoveTime)
        {
            handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        hand.AttackState = true;
        yield return new WaitForSeconds(sweaperwaitTime);
        //손이 휩쓸기 스타트
        tuple = calculateSweapvector(EndPos, handtransform.position,-randdistance, SweaperEndMoveTime);
        vec = tuple.Item1;
        vec.y = 0;
        speed = tuple.Item2;
        if (boss1SOundManager != null)
            boss1SOundManager.HandSwerapEndClipPlay();
        while (sweapertimer <= SweaperEndMoveTime)
        {
            handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        //OnePostion=handposition
        sweapertimer = 0;
        hand.AttackState = false;
        //손이 원위치로
        tuple = calculateSweapvector(HandOnepositon, handtransform.position, sweaperReturnTime);
        vec = tuple.Item1;
        speed = tuple.Item2;
        while (sweapertimer <= sweaperReturnTime)
        {
            handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        handtransform.position = HandOnepositon;
        activehand = null;


    }
    public IEnumerator Sweaper2(Boss1Hand hand, Vector3 StartPos, Vector3 EndPos)
    {
        activehand = hand;
        Transform handtransform = hand.transform;
        Vector3 HandOnepositon = handtransform.position;
        float randdistance = UnityEngine.Random.Range(0, SweapDistanceRandomWeight);
        int randomValue = UnityEngine.Random.Range(0, 2) * 2 - 1;

        randdistance *= randomValue;

        //시작지점으로 손이 감
        var tuple = calculateSweapvector(StartPos, handtransform.position, randdistance, SweaperStartMoveTime);
        Vector3 vec = tuple.Item1;
        vec.y += handsize * 0.5f;
        float speed = tuple.Item2;
        var rotationspeed = 90 / SweaperStartMoveTime;
        
        if (boss1SOundManager != null)
            boss1SOundManager.HandSwerapStartClipPlay();
        while (sweapertimer <= SweaperStartMoveTime)
        {
            handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime,Space.World);
       
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        hand.AttackState = true;
        yield return new WaitForSeconds(sweaperwaitTime);
        //손이 휩쓸기 스타트
        tuple = calculateSweapvector(EndPos, handtransform.position, -randdistance, SweaperEndMoveTime);
        vec = tuple.Item1;
        vec.y = 0;
        speed = tuple.Item2;
        if (boss1SOundManager != null)
            boss1SOundManager.HandSwerapEndClipPlay();
        while (sweapertimer <= SweaperEndMoveTime)
        {
            handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }

        sweapertimer = 0;
        yield return new WaitForSeconds(SweaperPatternDealy);
        //한번더
        tuple = calculateSweapvector(StartPos, handtransform.position, randdistance, SweaperEndMoveTime);
        vec = tuple.Item1;
        vec.y = 0;
        speed = tuple.Item2;
        if (boss1SOundManager != null)
            boss1SOundManager.HandSwerapEndClipPlay();
        while (sweapertimer <= SweaperEndMoveTime)
        {
            handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        hand.AttackState = false;
        sweapertimer = 0;
        //손이 원위치로
        tuple = calculateSweapvector(HandOnepositon, handtransform.position, sweaperReturnTime);
        vec = tuple.Item1;
        speed = tuple.Item2;
        while (sweapertimer <= sweaperReturnTime)
        {
            handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        sweapertimer = 0;
        handtransform.position = HandOnepositon;

        activehand = null;
    }

}
