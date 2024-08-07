using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTest : MonoBehaviour
{
    public Transform pos; // 자신의 위치 변수
    public Vector3 target; // 추적할 타겟
    public Transform checkPlayerPos;

    public float distance; // 계산된 두 벡터 사이의 거리를 저장할 변수

    public float playerRange = 1.2f; // 플레이어 인지할 범위 지정할 변수
    public float attackRange = 0.5f; // 공격 실행 범위
    public float moveRange = 1.8f; // 정찰 범위


    public bool playerCheck; //플레이어 발견 시 true 반환    
    public bool done; // 목적지 도착 유무

    public Transform first; // 왼쪽 끝 정찰 범위
    public Transform second; // 오른쪽 끝 정찰 범위

    public float waitTimer; // 목적지에 도착했을 때 다음 목적지 설정까지 대기
    public float maxTimer; // 대기 시간
    // Start is called before the first frame update
    void Start()
    {
        //최초 생성 시, 초기화된 위치를 저장
        maxTimer = 2f;
        pos = this.transform;
        first.position = pos.position + new Vector3(-3.5f, 0, 0);
        second.position = pos.position + new Vector3(3.5f, 0, 0);
        done = true;
        //최초 위치를 기준으로 x 포지션 랜덤 탐색
        //플레이어가 감지되면 추적 함수 실행
        //플레이어가 감지되지 않으면 정찰 함수 실행
    }


    /*private void FixedUpdate()
    {
        //Move();

        // 플레이어를 탐색
        playerCheck = PlayerTarget();

        //플레이어가 근처에 존재한다면 추격
        if (playerCheck)
        {
            //플레이어 트랜스폼을 타겟 트랜스폼으로 지정
            target = checkPlayerPos.position;
            //타겟(목적지)까지 이동
            nma.SetDestination(target);

            // 공격 범위 내에 플레이어가 들어왔을 때 
            // 잠시 멈췄다가 공격 애니메이션 실행 >> 가정용 다리미는 돌격 연출(이동속도가 증가하고 부딪히면 데미지 적용)
            if (attackRange - 0.1f < Vector3.Distance(transform.position, target) && attackRange + 0.1f > Vector3.Distance(transform.position, target))
            {
                nma.isStopped = true;
            }
            else
            {
                nma.isStopped = false;
            }
        }
        else
        {
            // 목적지에 도착했을 때
            if (done)
            {
                // 타이머를 증가
                waitTimer += Time.deltaTime;
                // 타이머가 대기 시간만큼 증가했을 때 타이머를 0으로 초기화
                if (waitTimer >= maxTimer)
                {
                    waitTimer = 0f;
                }
            }

            // 타이머가 0이 되었을 때
            // 정찰 시작
            if (waitTimer == 0)
            {
                PointPatrol();
            }

        }

    }*/

    //추적 함수
    public void PlayerTracking()
    {

    }

    //정찰 함수
    /* public void PointPatrol()
     {
         Debug.Log("정찰중");
         //정찰할 때 현재 자신의 위치를 기준으로 목적지 탐색
         //목적지는 transform.position.x 값을 기준으로 오차 범위 지정
         if (done)
         {
             nma.isStopped = true;
             done = false;
             Debug.Log("목적지 재탐색");
             float posX = Random.Range(-2.5f, 2.5f);
             target = transform.position + new Vector3(posX, 0, 0);
             nma.isStopped = false;
         }

         nma.SetDestination(target);

         //Debug.Log(Vector3.Distance(transform.position, target));

         if (Vector3.Distance(transform.position, target) < 1.8f)
         {
             done = true;
             Debug.Log("도착");
         }

         //NavMeshAgent.SetDestination(target);
     }*/

    /*bool PlayerTarget()
    {
        bool find = false;

        Collider[] colliders = Physics.OverlapSphere(transform.position, playerRange);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                checkPlayerPos = colliders[i].transform;
                find = true;
                return find;
            }
        }
        return find;
    }*/

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, moveRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, playerRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }*/
}
