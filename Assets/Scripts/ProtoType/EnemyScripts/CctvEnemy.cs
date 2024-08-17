using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CctvEnemy : Enemy
{    
    public CctvNeckRotate cctvNeck;    
    [Header("���� ����Ʈ")]
    public Transform[] patrolPoint;
    int pointElement;    
    public float waitTime;
    public bool pointCheck, endWait;
    public Light ccTvLight;

    [Header("ȣ�� �� ���� ���� �ð�")]
    public float callTime;
    public float stopTime;

    public Quaternion rot;

    public float headValue;
    public float neckValue;

    private void Start()
    {
        pointCheck = true;
        endWait = true;
    }

    public void EnemyCall()
    {
        
        Collider[] colliders = Physics.OverlapBox(transform.position + searchCubePos, searchCubeRange);        
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Enemy"))
            {
                Debug.Log($"ȣ������ ����{colliders[i].gameObject}");
                Enemy enemy = colliders[i].GetComponent<Enemy>();
                enemy.onPatrol = false;
                enemy.callCheck = true;
                enemy.tracking = true;
                enemy.target = target; 
                enemy.searchPlayer = true;
            }                  
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + searchCubePos, searchCubeRange * 2f);
        //Gizmos.DrawWireSphere(transform.position, searchRange);
    }

    public override void Move()
    {
        if (eStat.eState != EnemyState.dead || eStat.eState != EnemyState.hitted)
        {

            /*if (tracking)
            {
                if (!activeAttack && !onAttack)
                {
                    TrackingMove();
                }
            }*/

            
            if (!tracking)
                CctvPatrol();
        }
    }

    public void TrackingPlayer()
    {
        cctvNeck.target = target;
    }

    void CctvPatrol()
    {
        if (pointCheck && endWait)
        {
            if (pointElement >= patrolPoint.Length)
            {
                pointElement = 0;
            }
            target = patrolPoint[pointElement++];
            cctvNeck.target = target;
            pointCheck = false;
            endWait = false;
        }

        /*if (!pointCheck)
        {
            testTarget = target.position - cctvHead.transform.position;
            
            Quaternion lookRot = Quaternion.LookRotation(testTarget);
            rot = lookRot;
            //float angleValue = Quaternion.Angle(cctvHead.transform.rotation, lookRot);
            //float finalSpeed = rotationSpeed * (angleValue / 180f);
            cctvHead.transform.rotation = Quaternion.RotateTowards(cctvHead.transform.rotation, lookRot, eStat.rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(cctvHead.transform.rotation, lookRot) < 0.2f)
            {
                pointCheck = true;
                StartCoroutine(CctvWaitTime());              
            }
        }*/
    }    

    IEnumerator CctvWaitTime()
    {
        yield return new WaitForSeconds(waitTime);
        endWait = true;
    }

    void CctvTrackingMove()
    {


        //testTarget = target.position - cctvHead.transform.position;
        //testTarget.y = 0;
        //cctvHead.transform.LookAt(testTarget);
        //Quaternion lookRot = Quaternion.LookRotation(testTarget);
        //lookRot = Quaternion.Euler(lookRot.eulerAngles.x, cctvHead.transform.eulerAngles.y, lookRot.eulerAngles.z);        
        //cctvHead.transform.rotation = Quaternion.Lerp(cctvHead.transform.rotation, lookRot,rotationSpeed * Time.deltaTime);
        //cctvHead.transform.rotation = Quaternion.RotateTowards(cctvHead.transform.rotation, lookRot, eStat.rotationSpeed * Time.deltaTime);
        //cctvHead.transform.rotation = Vector3.RotateTowards();
    }
}
