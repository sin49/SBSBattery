using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Boss1Sweap : EnemyAction
{
    public Boss1Hand Lhand;

    public Transform LhandDefeatTransform;
    public Transform RhandDefeatTransform;

    public Boss1Hand RHand;
    Boss1Hand activehand;
    Transform target;
    [Header("보스 스테이지 바닥")]
    public Transform BossField;
    Boss1SOundManager boss1SOundManager;

    public List<Transform> LhandSweapPos=new List<Transform>();
    public List<Transform> RhandSweapPos=new List<Transform>();


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
    [Header("목표지점에서 대기하는 시간")]
    public float SweaperEndWaitTime;
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
    public Color sweapColor;

    Animator ani;
    public override void Invoke(Action ActionENd,Transform target = null)
    {
        this.target = target;
        ani.enabled = false;
        registerActionHandler(ActionENd);
        StartCoroutine(SweaperPattern());
    }
   
    private void Awake()
    {
        boss1SOundManager=GetComponent<Boss1SOundManager>();
        tv = GetComponent<BossTv>();
        ani = GetComponent<Animator>();


    }
    Tuple<Vector3 ,Vector3 > GetRandPosition()
    {
        int randint = UnityEngine.Random.Range(0, LhandSweapPos.Count);

        return new Tuple<Vector3 , Vector3 >(LhandSweapPos[randint].position,
            RhandSweapPos[randint].position);
    }
    public Tuple<Vector3, float> calculateSweapvector(Vector3 goal, Vector3 startpos, float time)
    {
   
        Vector3 vec = goal - startpos;
        float distance = vec.magnitude;
     
            float speed = distance / time;
      
        return new Tuple<Vector3, float>(vec, speed);
    }
    BossTv tv;
    public IEnumerator SweaperPattern()
    {
        var a = GetRandPosition();
        if (tv.Phase2)
        {
            if (Lhand.active && RHand.active)
            {
                bool randombool = UnityEngine.Random.Range(0, 2) == 0;


                if (randombool)
                {


                    yield return StartCoroutine(Sweaper2(Lhand, a.Item1, a.Item2));
                }
                else
                {

                    yield return StartCoroutine(Sweaper2(RHand, a.Item2, a.Item1));
                }
            }
            else if (Lhand.active && !RHand.active)
            {

                yield return StartCoroutine(Sweaper2(Lhand, a.Item1, a.Item2));
            }
            else if (!Lhand.active && RHand.active)
            {

                yield return StartCoroutine(Sweaper2(RHand, a.Item2, a.Item1));
            }
        }
        else
        {
            if (Lhand.active && RHand.active)
            {
                yield return StartCoroutine(Stomb(Lhand, RHand));
            }else if (!Lhand.active)
            {
                yield return StartCoroutine(Stomb(RHand, RHand));
            }
            else if (!RHand.active)
            {
                yield return StartCoroutine(Stomb(Lhand, Lhand));
            }
        }
        yield return StartCoroutine(DisableAction(0.1f));
    }
    public int stombcount;
    public float stombYPlus;
    public float stombYEnd;
    public float stombinittime;
    public float stombtime;
    public float stombwaitTIme;
    public float stombwaitTIme2;
    public float stombendwaitTIme;
    public float stombreturntime;
    public IEnumerator Stomb(Boss1Hand Lhand,Boss1Hand Rhand)
    {
        for(int n = 0; n < stombcount; n++)
        {
            float timer=0;
            Boss1Hand hand;
            if (n % 2 == 1)
            {
                hand = Rhand;
            } else
                hand = Lhand;
            activehand = hand;
            Transform handtransform = hand.transform;
            Vector3 HandOnepositon = handtransform.position;
            Quaternion handrot = handtransform.rotation;
            Vector3 Playerpos = PlayerHandler.instance.CurrentPlayer.transform.position;
            Playerpos.y = 0;
            Playerpos += Vector3.up* stombYEnd;
            var tuple = calculateSweapvector(Playerpos + Vector3.up * stombYPlus,
                handtransform.position, stombinittime
                );
            Vector3 vec = tuple.Item1;
            float speed = tuple.Item2;
            if (boss1SOundManager != null)
                boss1SOundManager.HandSwerapStartClipPlay();
            float rotatevector = hand.transform.rotation.z * -1 / stombinittime;
            while (timer <= stombinittime)
            {

                if ((rotatevector < 0 && handtransform.localRotation.z > 0) || (rotatevector > 0 && handtransform.localRotation.z < 0))
                    handtransform.Rotate(Vector3.forward * 3.4f * rotatevector);
                else
                    handtransform.localRotation = Quaternion.identity;
                handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
                timer += Time.fixedDeltaTime;
                yield return null;
            }
            handtransform.localRotation = Quaternion.identity;
            timer = 0;
            hand.AttackState = true;

            yield return new WaitForSeconds(stombwaitTIme);
            tuple = calculateSweapvector(Playerpos ,
                handtransform.position, stombtime
                );
            vec = tuple.Item1;

            speed = tuple.Item2;

            if (boss1SOundManager != null)
                boss1SOundManager.HandSwerapEndClipPlay();
            while (timer <= stombtime)
            {
                handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
                timer += Time.fixedDeltaTime;
                yield return null;
            }
            timer = 0;
            hand.AttackState = false;
            hand.shakeonce();
            yield return new WaitForSeconds(stombendwaitTIme);

            tuple = calculateSweapvector(Playerpos + Vector3.up * stombYPlus, handtransform.position, stombendwaitTIme);
            vec = tuple.Item1;
            speed = tuple.Item2;
      
            while (timer <= stombendwaitTIme)
            {
               
                handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
                timer += Time.fixedDeltaTime;
                yield return null;
            }
 
            timer = 0;
     
            yield return new WaitForSeconds(stombwaitTIme2);
            tuple = calculateSweapvector(HandOnepositon, handtransform.position, stombreturntime);
            vec = tuple.Item1;
            speed = tuple.Item2;
            rotatevector = handrot.z
                / stombreturntime;
            while (timer <= stombreturntime)
            {
                if ((rotatevector < 0 && handtransform.localRotation.z > 0) || (rotatevector > 0 && handtransform.localRotation.z < 0))
                    handtransform.Rotate(Vector3.forward * 3.4f * rotatevector);
                else
                    handtransform.localRotation = handrot;
                handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
                timer += Time.fixedDeltaTime;
                yield return null;
            }
            handtransform.localRotation = handrot;
            timer = 0;
            handtransform.position = HandOnepositon;
            activehand = null;
        }
    }

    public IEnumerator Sweaper2(Boss1Hand hand, Vector3 StartPos, Vector3 EndPos)
    {
        Quaternion handrot = hand.transform.rotation;
        activehand = hand;
        Transform handtransform = hand.transform;
        Vector3 HandOnepositon = handtransform.position;
        float rotatevector = hand.transform.rotation.z * -1 / SweaperStartMoveTime;
        //시작지점으로 손이 감
        var tuple = calculateSweapvector(StartPos, handtransform.position, SweaperStartMoveTime);
        Vector3 vec = tuple.Item1;
        vec.y += handsize * 0.5f;
        float speed = tuple.Item2;
        var rotationspeed = 90 / SweaperStartMoveTime;
        
        if (boss1SOundManager != null)
            boss1SOundManager.HandSwerapStartClipPlay();
        while (sweapertimer <= SweaperStartMoveTime)
        {
            if ((rotatevector < 0 && handtransform.localRotation.z > 0) || (rotatevector > 0 && handtransform.localRotation.z < 0))
                handtransform.Rotate(Vector3.forward * 3.4f * rotatevector);
            else
                handtransform.localRotation = Quaternion.identity;
            handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime,Space.World);
       
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        handtransform.localRotation = Quaternion.identity;
        sweapertimer = 0;
        hand.AttackState = true;
        yield return new WaitForSeconds(sweaperwaitTime);
        //손이 휩쓸기 스타트
        tuple = calculateSweapvector(EndPos, handtransform.position, SweaperEndMoveTime);
        vec = tuple.Item1;

        speed = tuple.Item2;
        if (boss1SOundManager != null)
            boss1SOundManager.HandSwerapEndClipPlay();
        hand.makeshake();
        while (sweapertimer <= SweaperEndMoveTime)
        {
            handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }

        sweapertimer = 0;
        hand.stopShake();
        yield return new WaitForSeconds(SweaperPatternDealy);
        //한번더
        hand.makeshake();
        tuple = calculateSweapvector(StartPos, handtransform.position, SweaperEndMoveTime);
        vec = tuple.Item1;
 
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
        hand.stopShake();
        yield return new WaitForSeconds(SweaperEndWaitTime);
        //손이 원위치로
        tuple = calculateSweapvector(HandOnepositon, handtransform.position, sweaperReturnTime);
        vec = tuple.Item1;
        speed = tuple.Item2;
        rotatevector = handrot.z / sweaperReturnTime;
        while (sweapertimer <= sweaperReturnTime)
        {
            if ((rotatevector < 0 && handtransform.localRotation.z > 0) || (rotatevector > 0 && handtransform.localRotation.z < 0))
                handtransform.Rotate(Vector3.forward * 3.4f * rotatevector);
            else
                handtransform.localRotation = handrot;
            handtransform.Translate(vec.normalized * speed * Time.fixedDeltaTime, Space.World);
            sweapertimer += Time.fixedDeltaTime;
            yield return null;
        }
        handtransform.localRotation = handrot;
        sweapertimer = 0;
        handtransform.position = HandOnepositon;

        activehand = null;
    }

}
