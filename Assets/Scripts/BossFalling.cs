using System.Collections;
using System.Collections.Generic;
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
public class BossFalling : MonoBehaviour
{
    [Header("낙하물 오브젝트")]
    public GameObject[] fallingObj;
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
