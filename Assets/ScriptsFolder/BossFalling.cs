using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;




/*���� ���� ����
���� ����
������Ʈ �پ��ϰ�
(������ ���ظ� ����(ũ�� �پ��ϰ�),
������Ʈ ������Ű�� ����(�� ������ ����)
������ Ÿ�̹��� �ٴڿ� ������)
������ �е�(�ð�) ���� �����ϰ�
������Ʈ �ӵ� ���� �����ϰ�

���� ���� �����ϰ�
������Ʈ ���� ���� �����ϰ�
������Ʈ ������ �� ���⿡ �������� ��� �ٴڿ� ǥ�� ���Ѿ� �ǿ� 
(�Ҽ������� ����� �ѱ�� ������Ʈ ���� ����������� ǥ�ð� ���ϰ� )
*/
[Serializable]
public class Boss1FallObj
{
    [Header("���� ������Ʈ")]
    public GameObject fallingobj;//������Ʈ �ν��Ͻ��� ������
    [Header("������ ���� ����")]
    public int number = 0;//Ȯ�� ������� ������ �������� ���� 0�̸� Ȯ��
    [Header("Ȯ��")]
    public int possibility;//Ȯ��
}
[Serializable]
public class Boss1BoxFallCreateObj
{
    public GameObject Createobj;//������Ʈ �ν��Ͻ��� ������
    [Header("Ȯ��")]
    public int possibility;//Ȯ��
}

//����Ʈ �� �����
//for( n=0; n<possibility;n+int+)
//����Ʈ�� ������Ʈ�� �ִ´�
//����Ʈ�� �ƹ��͵� �� ����
//����Ʈ�� 10���� ���ϱ� �׸�ŭ Ȯ�� ��

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

    //[Header("���Ϲ� ������Ʈ")]
    //public List< GameObject> fallingObj;

    [Header("���Ϲ� ������Ʈ")]
    public List<Boss1FallObj> fallingObj2;

    List<GameObject> fallingobjects = new List<GameObject>();

    HashSet<GameObject> EssenetialFallObjectHashSet = new HashSet<GameObject>();

    Animator ani;

    public Boss1SOundManager soundmanager;
    
    [Header("���� ���� ������Ʈ ����")]
    public List<Boss1BoxFallCreateObj> fallingBoxCreateObj;

    List<GameObject> BoxFallCreateObjects;
    [Header("������")]
    public float damage;
    [Header("�����ð�, �ּ�/�ִ�ӵ�")]
    public float createTime;
    public float minSpeed;
    public float maxSpeed;

    int createCount;
    [Header("���Ϲ� ī��Ʈ ����, ���� ����")]
    public int createCountMax;
    public float fallingHeight;
    [Header("���� ���� ����")]
    public float fallingRange;
    [Header("���� �������� �ٴ�")]
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
        Debug.Log("Shake ����");
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
    List<T> Shuffle<T>(List<T> list)//���� ����Ʈ
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
