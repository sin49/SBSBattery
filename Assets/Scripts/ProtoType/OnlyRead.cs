using UnityEngine;

public class EnemyInter : MonoBehaviour
{
    void Onmove()
    {
        //translate�켱
        //rigidbody ��
        //ZMove�� �����س���
        //left right arrow Xmove
        //Up Down arrow Zmove
    }
    void Jump()
    {
        //addforce
        //�÷����� ����� �� ���� ����(�ٴ�,õ��, ���� ��Ƶ� ���� ������ �Ű澲������)
        //YMove 
    }
    GameObject PlayerMeleeAttack;
    GameObject PlayerRangeAttack;
    void MeleeAttack()
    {
        //ColliSion Prefab Ȱ��ȭ ��Ű�� �ɷ�?

    }
    void rangeAttack()
    {
        //Collision prefab instaiate ��Ű�� �ɷ�?

    }

    private void OnCollisionEnter(Collision collision)
    {
        //���ظ� ����
        //if(Hp0)
        //Dead();
    }
    void Dead()
    {
        //Destroy();
    }
}
