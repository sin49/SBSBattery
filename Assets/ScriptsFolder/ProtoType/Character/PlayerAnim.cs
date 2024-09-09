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

    // ���� �ִϸ��̼� ���� �������� ��
    // ������ �ִϸ��̼��� �����Ǿ�
    // ���� ������ �̾��� �� �ְ� �׽�Ʈ ���� �ִϸ��̼� �Լ�
    public void ContinueAnimation()
    {
        Debug.Log("������ �ִϸ��̼� �ٽ� ȣ��");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Unity_Chan_Jump"))
        {
            animator.speed = 1;

            //StartCoroutine(player.WaitAndFalseAnimation("isJump", animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
        }
    }

    // ���� ���� �ݶ��̴� Ȱ��ȭ �Լ�
    /*public void MeleeCollider()
    {
        // �÷��̾��� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(player.ActiveMeleeAttack());
    }*/
}
