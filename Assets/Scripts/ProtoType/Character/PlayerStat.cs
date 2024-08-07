using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public enum CurrentAttack { ground, sky }

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

    //public float rotationValue; // ����Ű �Է� �� ���� ���� ����
    //public float dashForce; // �뽬 ����ġ
    //public float dashTimer;// ��Ÿ�� 
    //public float dashCoolTime; // �뽬 �ִ� ��Ÿ��    



    public bool Trans3D;

    
   
    public void RecoverHP(float hppoint)
    {
        this.hp += hppoint;
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
        Trans3D = false;
    }
    
  


    /*private void FixedUpdate()
    {
        if (hp <= 0)
            SceneManager.LoadScene("Title");

    }*/    
}
