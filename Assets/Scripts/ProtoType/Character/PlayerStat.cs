using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public enum CurrentAttack { ground, sky }
public enum PlayerMoveState { SideX,SideZ,Trans3D}

public class PlayerStat : CharacterStat
{
    public static PlayerStat instance;
    [HideInInspector]
    public direction direction = direction.Right;
    [HideInInspector]
    public PlayerState pState = PlayerState.idle;
    [HideInInspector]
    public CurrentAttack currentAttack;

    [Header("#이단점프, 무적시간 등의 변수"),    HideInInspector]
    public bool doubleJump; // 이단 점프 체크
    [HideInInspector]
    public bool ableJump;
    [Header("플레이어 무적 시간")]
    public float invincibleCoolTime; // 무적 지속시간
    [Header("공격시 전진 거리")]
    public float attackForce; // 근접 공격 시 addforce에 적용할 값
    [Header("점프력")]
    public float jumpForce; // velocity.y 값에 영향을 주는 점프 가중치            
    [Header("내려찍는 속도")]
    public float downForce; // 내려찍는 힘   
    [Header("내려찍기 전 체공 시간")]
    public float downAttackFlyTime;
    [HideInInspector]
    public bool formInvincible; // 변신 무적
    [Header("상호작용 딜레이")]
    public float InteractDelay;

    //public float rotationValue; // 방향키 입력 시 받을 방향 변수
    //public float dashForce; // 대쉬 가중치
    //public float dashTimer;// 쿨타임 
    //public float dashCoolTime; // 대쉬 최대 쿨타임    


    [Header("이동 상태")]
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
        MoveState = PlayerMoveState.SideX;
    }
    
  


    /*private void FixedUpdate()
    {
        if (hp <= 0)
            SceneManager.LoadScene("Title");

    }*/    
}
