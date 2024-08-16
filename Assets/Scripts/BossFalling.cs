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

    //[Header("���Ϲ� ������Ʈ")]
    //public List< GameObject> fallingObj;

    [Header("���Ϲ� ������Ʈ")]
    public List<Boss1FallObj> fallingObj2;

    List<GameObject> fallingobjects = new List<GameObject>();

    HashSet<GameObject> EssenetialFallObjectHashSet = new HashSet<GameObject>();

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

    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        soundmanager=GetComponent<Boss1SOundManager>();
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
        registerActionHandler(ActionENd);
        StartCoroutine (FallingAttack());
   
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
    IEnumerator FallingAttack()
    {
        //while (createCount < createCountMax)
        //{
        //    GameObject obj = Instantiate(enemy, RandomSpawn(), Quaternion.identity);

        //    if (obj.GetComponent<FallingObject>() != null)
        //    {
        //        var a = obj.GetComponent<FallingObject>();
        //        a.fallingSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        //        a.fieldPos = fieldMax;
        //        a.damage = damage;
        //    }
        //    else
        //    {
        //        var a = obj.GetComponent<BossStageBox>();
        //        a.fallingSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        //        a.fieldPos = fieldMax;
        //    }

        //    createCount++;

        //    yield return new WaitForSeconds(createTime);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 center = (fieldMin + fieldMax)/2;
        Vector3 size = new(Mathf.Abs(fieldMax.x - fieldMin.x) / fallingRange, fieldMin.y, Mathf.Abs(fieldMax.z - fieldMin.z) / fallingRange);
        Gizmos.DrawWireCube(center, size);
    }
}
