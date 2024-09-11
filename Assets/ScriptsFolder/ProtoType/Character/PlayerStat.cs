using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public enum CurrentAttack { ground, sky }
public enum PlayerMoveState {
     Xmove, XmoveReverse, Zmove, ZmoveReverse, ZXMove3D, XZMove3D,
 XZMove3DReverse, ZXMove3DReverse
}

public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;
    [HideInInspector]
    public direction direction = direction.Right;
    [HideInInspector]
    public PlayerState pState = PlayerState.idle;
    [HideInInspector]
    public CurrentAttack currentAttack;

    [Header("#�̴�����, �����ð� ���� ����"),    HideInInspector]
    public bool doubleJump; // �̴� ���� üũ
    [HideInInspector]
    public bool ableJump;
    [Header("�÷��̾� ���� �ð�")]
    public float invincibleCoolTime; // ���� ���ӽð�
    [Header("���ݽ� ���� �Ÿ�")]
    public float attackForce; // ���� ���� �� addforce�� ������ ��
    [Header("������")]
    public float jumpForce; // velocity.y ���� ������ �ִ� ���� ����ġ            
    [Header("������� �ӵ�")]
    public float downForce; // ������� ��   
    [Header("������� �� ü�� �ð�")]
    public float downAttackFlyTime;
    [HideInInspector]
    public bool formInvincible; // ���� ����
    [Header("��ȣ�ۿ� ������")]
    public float InteractDelay;
    [Header("�ǰ� �� ����")]
    public float HittedStopTime = 0.2f;
    //public float rotationValue; // ����Ű �Է� �� ���� ���� ����
    //public float dashForce; // �뽬 ����ġ
    //public float dashTimer;// ��Ÿ�� 
    //public float dashCoolTime; // �뽬 �ִ� ��Ÿ��    


    [Header("�̵� ����")]
    public PlayerMoveState MoveState;

    event Action recoverevent;
   public void registerRecoverAction(Action a)
    {
        recoverevent += a;
    }
    public void RecoverHP(float hppoint)
    {
        this.hp += hppoint;
        recoverevent?.Invoke();
        if (this.hp > hpMax)
        {
            this.hp = hpMax;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        MoveState = PlayerMoveState.Xmove;
    }
    
  


    /*private void FixedUpdate()
    {
        if (hp <= 0)
            SceneManager.LoadScene("Title");

    }*/    
}
