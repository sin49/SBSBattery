using UnityEngine;

public class EnemyInter : MonoBehaviour
{
    void Onmove()
    {
        //translate우선
        //rigidbody 건
        //ZMove도 구현해놓기
        //left right arrow Xmove
        //Up Down arrow Zmove
    }
    void Jump()
    {
        //addforce
        //플랫폼에 닿았을 때 점프 가능(바닥,천장, 벽에 닿아도 점프 되지만 신경쓰지말기)
        //YMove 
    }
    GameObject PlayerMeleeAttack;
    GameObject PlayerRangeAttack;
    void MeleeAttack()
    {
        //ColliSion Prefab 활성화 시키는 걸로?

    }
    void rangeAttack()
    {
        //Collision prefab instaiate 시키는 걸로?

    }

    private void OnCollisionEnter(Collision collision)
    {
        //피해를 받음
        //if(Hp0)
        //Dead();
    }
    void Dead()
    {
        //Destroy();
    }
}
