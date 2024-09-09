using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTest : MonoBehaviour
{
    public Transform pos; // �ڽ��� ��ġ ����
    public Vector3 target; // ������ Ÿ��
    public Transform checkPlayerPos;

    public float distance; // ���� �� ���� ������ �Ÿ��� ������ ����

    public float playerRange = 1.2f; // �÷��̾� ������ ���� ������ ����
    public float attackRange = 0.5f; // ���� ���� ����
    public float moveRange = 1.8f; // ���� ����


    public bool playerCheck; //�÷��̾� �߰� �� true ��ȯ    
    public bool done; // ������ ���� ����

    public Transform first; // ���� �� ���� ����
    public Transform second; // ������ �� ���� ����

    public float waitTimer; // �������� �������� �� ���� ������ �������� ���
    public float maxTimer; // ��� �ð�
    // Start is called before the first frame update
    void Start()
    {
        //���� ���� ��, �ʱ�ȭ�� ��ġ�� ����
        maxTimer = 2f;
        pos = this.transform;
        first.position = pos.position + new Vector3(-3.5f, 0, 0);
        second.position = pos.position + new Vector3(3.5f, 0, 0);
        done = true;
        //���� ��ġ�� �������� x ������ ���� Ž��
        //�÷��̾ �����Ǹ� ���� �Լ� ����
        //�÷��̾ �������� ������ ���� �Լ� ����
    }


    /*private void FixedUpdate()
    {
        //Move();

        // �÷��̾ Ž��
        playerCheck = PlayerTarget();

        //�÷��̾ ��ó�� �����Ѵٸ� �߰�
        if (playerCheck)
        {
            //�÷��̾� Ʈ�������� Ÿ�� Ʈ���������� ����
            target = checkPlayerPos.position;
            //Ÿ��(������)���� �̵�
            nma.SetDestination(target);

            // ���� ���� ���� �÷��̾ ������ �� 
            // ��� ����ٰ� ���� �ִϸ��̼� ���� >> ������ �ٸ��̴� ���� ����(�̵��ӵ��� �����ϰ� �ε����� ������ ����)
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
            // �������� �������� ��
            if (done)
            {
                // Ÿ�̸Ӹ� ����
                waitTimer += Time.deltaTime;
                // Ÿ�̸Ӱ� ��� �ð���ŭ �������� �� Ÿ�̸Ӹ� 0���� �ʱ�ȭ
                if (waitTimer >= maxTimer)
                {
                    waitTimer = 0f;
                }
            }

            // Ÿ�̸Ӱ� 0�� �Ǿ��� ��
            // ���� ����
            if (waitTimer == 0)
            {
                PointPatrol();
            }

        }

    }*/

    //���� �Լ�
    public void PlayerTracking()
    {

    }

    //���� �Լ�
    /* public void PointPatrol()
     {
         Debug.Log("������");
         //������ �� ���� �ڽ��� ��ġ�� �������� ������ Ž��
         //�������� transform.position.x ���� �������� ���� ���� ����
         if (done)
         {
             nma.isStopped = true;
             done = false;
             Debug.Log("������ ��Ž��");
             float posX = Random.Range(-2.5f, 2.5f);
             target = transform.position + new Vector3(posX, 0, 0);
             nma.isStopped = false;
         }

         nma.SetDestination(target);

         //Debug.Log(Vector3.Distance(transform.position, target));

         if (Vector3.Distance(transform.position, target) < 1.8f)
         {
             done = true;
             Debug.Log("����");
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
