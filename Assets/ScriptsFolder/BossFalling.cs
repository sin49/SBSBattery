using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;




/*보스 낙하 패턴
범위 램덤
오브젝트 다양하게
(닿으면 피해를 입음(크기 다양하게),
오브젝트 생성시키는 상자(적 나오는 상자)
나오는 타이밍이 바닥에 닿으면)
나오는 밀도(시간) 조절 가능하게
오브젝트 속도 조절 가능하게

범위 조절 가능하게
오브젝트 갯수 조절 가능하게
오브젝트 떨어질 때 여기에 떨어진다 라고 바닥에 표시 시켜야 되요 
(할수있으면 힘드면 넘기고 오브젝트 땅에 가까워질수록 표시가 강하게 )
*/
[Serializable]
public class Boss1FallObj
{
    [Header("생성 오브젝트")]
    public GameObject fallingobj;//오브젝트 인스턴스한 프리팹
    [Header("무조건 생성 갯수")]
    public int number = 0;//확률 상관없이 무조건 이정도만 나옴 0이면 확률
    [Header("확률")]
    public int possibility;//확률
}
[Serializable]
public class Boss1BoxFallCreateObj
{
    public GameObject Createobj;//오브젝트 인스턴스한 프리팹
    [Header("확률")]
    public int possibility;//확률
}

//리스트 더 만들고
//for( n=0; n<possibility;n+int+)
//리스트에 오브젝트를 넣는다
//리스트에 아무것도 안 들어가고
//리스트에 10개가 들어가니깐 그만큼 확률 온

public class BossFalling : EnemyAction
{

    public float shakertimer;
    public Transform LhandTransform;
    public Transform RhandTransform;
    Vector3 LhandOriginPosition;
    Vector3 RhandOriginPosition;
    public float handreturntime = 2;
    IEnumerator handreturn()
    {
   
        float timer = 0;
        float rotationspeed = 60 / handreturntime;
        Vector3 Lhandvector = (-LhandTransform.position + LhandOriginPosition);
        float LSpeed = Lhandvector.magnitude / handreturntime;
        Vector3 Rhandvector = (-RhandTransform.position + RhandOriginPosition);
        float RSpeed = Rhandvector.magnitude / handreturntime;
        while (timer < handreturntime)
        {
            LhandTransform.Rotate(Vector3.forward  * rotationspeed * Time.fixedDeltaTime);
            RhandTransform.Rotate(Vector3.forward*-1 * rotationspeed * Time.fixedDeltaTime);
            LhandTransform.Translate(LSpeed * Lhandvector.normalized * Time.fixedDeltaTime, Space.World);
            RhandTransform.Translate(RSpeed * Rhandvector.normalized * Time.fixedDeltaTime, Space.World);
            timer += Time.fixedDeltaTime;
            yield return null;
        }
        LhandTransform.position = LhandOriginPosition;
        RhandTransform.position = RhandOriginPosition;
        LhandTransform.rotation = Quaternion.Euler(0, 0, 30);
        RhandTransform.rotation = Quaternion.Euler(0, 0, -30);
        yield return new WaitForSeconds(0.5f);
        DisableActionMethod();


    }
    public override void StopAction()
    {
        base.StopAction();
        StopAllCoroutines();
        ani.Play("FallingAttack");
        ani.enabled = false;
        StartCoroutine(handreturn());
    }
    public CinemachineImpulseSource fallingshaker;

    //[Header("낙하물 오브젝트")]
    //public List< GameObject> fallingObj;

    [Header("낙하물 오브젝트")]
    public List<Boss1FallObj> fallingObj2;

    List<GameObject> fallingobjects = new List<GameObject>();

    HashSet<GameObject> EssenetialFallObjectHashSet = new HashSet<GameObject>();

    Animator ani;

    public Boss1SOundManager soundmanager;
    
    [Header("낙하 상자 오브젝트 생성")]
    public List<Boss1BoxFallCreateObj> fallingBoxCreateObj;

    List<GameObject> BoxFallCreateObjects;
    [Header("데미지")]
    public float damage;
    [Header("생성시간, 최소/최대속도")]
    public float createTime;
    public float minSpeed;
    public float maxSpeed;

    int createCount;
    [Header("낙하물 카운트 제한, 낙하 높이")]
    public int createCountMax;
    public float fallingHeight;
    [Header("낙하 범위 조정")]
    public float fallingRange;
    [Header("보스 스테이지 바닥")]
    public Transform bossField;
    Vector3 fallingPoint;
    Vector3 fieldMin;
    Vector3 fieldMax;

    Vector3 fixMin;
    Vector3 fixMax;
    // Start is called before the first frame update
    void Start()
    {
        ani=GetComponent<Animator>();
        soundmanager =GetComponent<Boss1SOundManager>();
        if (bossField != null)
        {
            Vector3 min = new Vector3(-0.5f, 0.5f, -0.5f);
            Vector3 max = new Vector3(0.5f, 0.5f, 0.5f);

            fieldMin = bossField.TransformPoint(min);
            fieldMax = bossField.TransformPoint(max);
        }
        MakeBossFallingObjectsPossibility();
        //Falling();
    }

   public void Falling()
    {
        StartCoroutine(FallingAttack());
    }

    void MakeBossFallingObjectsPossibility()
    {
        fallingobjects = new List<GameObject>();
        foreach (var a in fallingObj2)
        {
     
            if (a.number != 0)
            {
                for (int n = 0; n < a.number; n++) {
                    EssenetialFallObjectHashSet.Add(a.fallingobj);
                }
                continue;
            }
            for (int n = 0; n < a.possibility; n++)
            {

                fallingobjects.Add(a.fallingobj);
            }
        }
        BoxFallCreateObjects = new List<GameObject>();
        foreach (var a in fallingBoxCreateObj)
        {
            for (int n = 0; n < a.possibility; n++)
            {
                BoxFallCreateObjects.Add(a.Createobj);
            }
        }

    }

    public override void Invoke(Action ActionENd,  Transform target = null)
    {
        LhandOriginPosition = LhandTransform.position;
        RhandOriginPosition = RhandTransform.position;
        registerActionHandler(ActionENd);
        ani.enabled = true;
   ani.Play("FallingAttack");
       
    }
    IEnumerator ShakeLoop()
    {
        float timer = 0;
        Debug.Log("Shake 실행");
        while (timer < createTime * createCountMax + 0.12f)
        {

            fallingshaker.GenerateImpulse();
            timer += shakertimer;
            yield return new WaitForSeconds(shakertimer);
        }
    }
 public   void StartfallingAttack()
    {
       
        StartCoroutine(FallingAttack());
   
   
    }
    Queue<Tuple<GameObject, Vector3>> ReturnFallObjectList()
    {
        List<GameObject> list = new List<GameObject>();
        foreach(var a in EssenetialFallObjectHashSet)
        {
            list.Add(a);
        }
        for (int n = 0; n < createCountMax- EssenetialFallObjectHashSet.Count; n++)
        {
            int element = UnityEngine.Random.Range(0, fallingobjects.Count);
            list.Add(fallingobjects[element]);
        }
        list = Shuffle<GameObject>(list);
        Queue<Tuple<GameObject,Vector3>> RQueue=new Queue<Tuple<GameObject,Vector3>>();
        
        foreach (var a in list)
        {
         
           
            RQueue.Enqueue(new Tuple<GameObject, Vector3>(a, RandomSpawn()));
        }
        return RQueue;
    }
    List<T> Shuffle<T>(List<T> list)//셔플 리스트
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
    protected override void CancelActionEvent()
    {
        base.CancelActionEvent();
    }
    public void animatorFalse()
    {
        ani.enabled = false;
    }
    IEnumerator FallingAttack()
    {
       
        StartCoroutine(ShakeLoop());
     
        //}

        yield return null;
        var queue = ReturnFallObjectList();
        while (queue.Count != 0)
        {
            var tuple = queue.Dequeue();
            GameObject obj = Instantiate(tuple.Item1, tuple.Item2, Quaternion.identity);
           if(soundmanager!=null)
            soundmanager.OBjectFallClipPlay() ;
            if (obj.GetComponent<FallingObject>() != null)
            {
                var a = obj.GetComponent<FallingObject>();
                a.fallingSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
                a.fieldPos = fieldMax;
                a.damage = damage;
                if (soundmanager != null)
                    a.ObjectgroundedSoundEvent = soundmanager.OBjectlandingClipPlay;
            }
            else
            {
                var a = obj.GetComponent<BossStageBox>();
                int rand2 = UnityEngine.Random.Range(0, BoxFallCreateObjects.Count);
                a.enemyPrefab = BoxFallCreateObjects[rand2];
                a.fallingSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
                a.fieldPos = fieldMax;
                if (soundmanager != null)
                    a.ObjectgroundedSoundEvent = soundmanager.OBjectlandingClipPlay;
            }




            createCount++;
            yield return new WaitForSeconds(createTime);
        }
        yield return StartCoroutine(DisableAction(0.1f));

    }    

    public Vector3 RandomSpawn()
    {
        Vector3 min = new(-0.5f / fallingRange, 0.5f, -0.5f / fallingRange);
        Vector3 max = new(0.5f / fallingRange, 0.5f, 0.5f / fallingRange);
        fixMin = bossField.TransformPoint(min);
        fixMax = bossField.TransformPoint(max);

        float posX = UnityEngine.Random.Range(fixMin.x, fixMax.x);
        float posZ = UnityEngine.Random.Range(fixMin.z, fixMax.z);

        fallingPoint = new(posX, bossField.position.y+fallingHeight, posZ);

        return fallingPoint;
    }
    public Color GizmoColor;
   
}
