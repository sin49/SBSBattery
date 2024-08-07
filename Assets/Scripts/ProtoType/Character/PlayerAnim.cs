using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator animator;
    public Player player;

    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
    }

    public void AttackAnimation(bool isAttack)
    {
        animator.SetBool("IsAttack", isAttack);
    }

    public void RunAnimation(bool isRun)
    {
        animator.SetBool("IsRun", isRun);
    }

    public void JumpAnimation(bool jumpAnim)
    {
        animator.SetBool("IsJump", jumpAnim);
    }

    public void OnGround()
    {
        /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("Unity_Chan_Jump"))
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime+"\n"+animator.GetCurrentAnimatorStateInfo(0).length);
        }*/
        if (player.onGround)
        {
            return;
        }
        else
        {
            animator.speed = 0;
        }
    }

    // 점프 애니메이션 땅에 접촉했을 때
    // 정지된 애니메이션이 재실행되어
    // 착지 동작이 이어질 수 있게 테스트 중인 애니메이션 함수
    public void ContinueAnimation()
    {
        Debug.Log("정지된 애니메이션 다시 호출");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Unity_Chan_Jump"))
        {
            animator.speed = 1;

            //StartCoroutine(player.WaitAndFalseAnimation("isJump", animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
        }
    }

    // 근접 공격 콜라이더 활성화 함수
    /*public void MeleeCollider()
    {
        // 플레이어의 코루틴 함수 호출
        StartCoroutine(player.ActiveMeleeAttack());
    }*/
}
