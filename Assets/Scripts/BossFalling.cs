using System.Collections;
using System.Collections.Generic;
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
public class BossFalling : MonoBehaviour
{
    [Header("���Ϲ� ������Ʈ")]
    public GameObject[] fallingObj;
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
    public Transform bossField;
    Vector3 fallingPoint;
    Vector3 fieldMin;
    Vector3 fieldMax;

    Vector3 fixMin;
    Vector3 fixMax;

    // Start is called before the first frame update
    void Start()
    {
        if (bossField != null)
        {
            Vector3 min = new Vector3(-0.5f, 0.5f, -0.5f);
            Vector3 max = new Vector3(0.5f, 0.5f, 0.5f);

            fieldMin = bossField.TransformPoint(min);
            fieldMax = bossField.TransformPoint(max);
        }        
    }

    public void CreateFallingObject()
    {
        StartCoroutine(FallingAttack());
    }

    IEnumerator FallingAttack()
    {
        yield return null;

        while (createCount < createCountMax)
        {
            int element = Random.Range(0, fallingObj.Length);
            GameObject obj = Instantiate(fallingObj[element], RandomSpawn(), Quaternion.identity);
            if (obj.GetComponent<FallingObject>() != null)
            {
                var a = obj.GetComponent<FallingObject>();
                a.fallingSpeed = Random.Range(minSpeed, maxSpeed);
                a.fieldPos = fieldMax;
                a.damage = damage;
            }
            else
            {
                var a = obj.GetComponent<BossStageBox>();
                a.fallingSpeed = Random.Range(minSpeed, maxSpeed);
                a.fieldPos = fieldMax;
            }
            
            createCount++;
            yield return new WaitForSeconds(createTime);
        }


    }    

    public Vector3 RandomSpawn()
    {
        Vector3 min = new(-0.5f / fallingRange, 0.5f, -0.5f / fallingRange);
        Vector3 max = new(0.5f / fallingRange, 0.5f, 0.5f / fallingRange);
        fixMin = bossField.TransformPoint(min);
        fixMax = bossField.TransformPoint(max);

        float posX = Random.Range(fixMin.x, fixMax.x);
        float posZ = Random.Range(fixMin.z, fixMax.z);

        fallingPoint = new(posX, transform.position.y * fallingHeight, posZ);

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
