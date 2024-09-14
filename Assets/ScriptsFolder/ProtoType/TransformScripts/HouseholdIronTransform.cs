using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HouseholdIronTransform : Player
{
    [Header("���� �غ�ð�")]
    public float rushReady;
    [Header("���� �̵��ӵ�")]
    public float rushSpeed, speedValue;
    float saveSpeed;
    [Header("���� ���ӽð�")]
    public float rushTimeMax;
    float rushTimer;
    [HideInInspector] public bool canRushAttack; // ���� ������ �޴� �ֱ�?�� ���� ����
    bool onRush, startRush, endRush;
    [Header("���� ���� ���ð�")]
    public float rushCoolTimeMax;
    float rushCoolTimer;
    [Header("���� ���� ���ط�")]
    public float rushDamage;
    [Header("���� ���� �ֱ�")]
    public float rushCycleMax, rushAtkTimeMax;
    float rushCycleTimer, rushAtkTimer;
    [Header("���� ��ȯ ������ �ð�")]
    public float rotateTimeMax;
    float rotateTimer;

    public float rushRay;

    bool readyRush, downEnd, rushEnd;
    bool ironAttack = true;

    [Header("Ư�� �ɷ� ���� ����\n")]
    [Header("�ٸ��� ��� �ӵ�")]public float downAtkSpeed;
    [Header("�ٸ��� ü�� �ð�")]public float ironFlyTime;
    [Header("��� �� ���� �Ұ� �ð�\n(�� ��, ���� �����)")]
    public float downAtkEndTimeMax;
    float downAtkEndTimer;
    [Header("�ٸ��� ���ݷ�")]public float ironDownDamage;
    [Header("�ٸ��� ���� ��� �ð�")]public float ironDownCoolTimeMax;
    float downCoolTimer;
    bool onDownCoolTime;

    protected override void Awake()
    {
        base.Awake();
        InitTimer();        
    }

    private void Start()
    {
        saveSpeed = PlayerStat.instance.moveSpeed;
        rushSpeed = PlayerStat.instance.moveSpeed + speedValue;
    }

    public void InitTimer()
    {
        rushCoolTimer = rushCoolTimeMax;
        rotateTimer = rotateTimeMax;
        rushCycleTimer = rushCycleMax;
        rushAtkTimer = rushAtkTimeMax;
        downAtkEndTimer = downAtkEndTimeMax;
        downCoolTimer = ironDownCoolTimeMax;
        rushTimer = rushTimeMax;
    }

    private void Update()
    {
        BaseBufferTimer();
        CheckRushTime();
        IronDownAttackTimeCheck();
    }

    public override void DownAttack()
    {
        if (onDownCoolTime)
            return;
        else
        {
            if (!downAttack)
            {
                downAttack = true;
                StartCoroutine(IronDownAttack());
            }
        }
    }
    IEnumerator IronDownAttack()
    {
        playerRb.useGravity = false;
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(transform.up * 2.5f);

        yield return new WaitForSeconds(ironFlyTime);

        playerRb.useGravity = true;
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(-transform.up * downAtkSpeed, ForceMode.Impulse);
        downAttackCollider.SetActive(true);
    }

    // �ٸ��� �ð� ���� üũ
    public void IronDownAttackTimeCheck()
    {
        //�ٸ��� ��� �� �����ð� -> ������ �Ұ����ϵ��� ��
        if (downEnd)
        {
            if (downAtkEndTimer > 0)
                downAtkEndTimer -= Time.deltaTime;
            else
            {
                downEnd = false;
                downAtkEndTimer = downAtkEndTimeMax;
                PlayerHandler.instance.CantHandle = false;
                onDownCoolTime = true;
            }
        }

        //�ٸ��� ��� �����ð� ���ĺ���
        //���� ���ð� ������
        if (onDownCoolTime)
        {
            if (downCoolTimer > 0)
                downCoolTimer -= Time.deltaTime;
            else
            {
                onDownCoolTime = false;
                downCoolTimer = ironDownCoolTimeMax;
            }
        }
    }

    public override void Move()
    {
        if (onRush)
        {
            hori = 0;
            Vert = 0;
            switch (PlayerStat.instance.MoveState)
            {
                case PlayerMoveState.Xmove:
                    hori = Input.GetAxisRaw("Horizontal");
                    if (PlayerHandler.instance.ladderInteract)
                        Vert = Input.GetAxisRaw("Vertical");
                    break;
                case PlayerMoveState.XmoveReverse:
                    hori = -1 * Input.GetAxisRaw("Horizontal");
                    break;

                case PlayerMoveState.Zmove:
                    Vert = Input.GetAxisRaw("Horizontal");
                    break;
                case PlayerMoveState.ZmoveReverse:
                    Vert = -1 * Input.GetAxisRaw("Horizontal");
                    break;
                case PlayerMoveState.XZMove3D:
                    hori = Input.GetAxisRaw("Vertical");
                    Vert = -1 * Input.GetAxisRaw("Horizontal");
                    break;
                case PlayerMoveState.XZMove3DReverse:
                    hori = -1 * Input.GetAxisRaw("Vertical");
                    Vert = Input.GetAxisRaw("Horizontal");
                    break;
                case PlayerMoveState.ZXMove3D:
                    Vert = Input.GetAxisRaw("Vertical");
                    hori = Input.GetAxisRaw("Horizontal");
                    break;
                case PlayerMoveState.ZXMove3DReverse:
                    Vert = -1 * Input.GetAxisRaw("Vertical");
                    hori = -1 * Input.GetAxisRaw("Horizontal");
                    break;
            }

            Vector3 moveInput = new Vector3(hori, 0, Vert);
            if (hori != 0 || Vert != 0)
            {
                rotate(moveInput.x, moveInput.z);
                //SoundPlayer.PlayMoveSound();
            }

            Vector3 moveVelocity = Vector3.zero;
            Vector3 vector = moveInput.normalized * rushSpeed;
            moveVelocity = vector - playerRb.velocity.x * Vector3.right - playerRb.velocity.z * Vector3.forward;
            if (!wallcheck)
                playerRb.AddForce(moveVelocity, ForceMode.VelocityChange);
            else
                playerRb.AddForce(new Vector3(0, playerRb.velocity.y, 0), ForceMode.VelocityChange);

            if (moveVelocity == Vector3.zero)
            {
                Vector3 CurrentVelocity = playerRb.velocity;

                var newDecelateVector = Vector3.Lerp(CurrentVelocity, Vector3.zero, Decelatate * Time.fixedDeltaTime);


                playerRb.velocity = new Vector3(newDecelateVector.x, CurrentVelocity.y, newDecelateVector.z);
                //else
                //           playerRb.velocity = new Vector3(0,playerRb.velocity.y, playerRb.velocity.z);

            }

            if (!isJump)
            {
                if (MoveCheck(hori, Vert))
                {
                    isRun = true;
                }
                else
                {
                    isRun = false;
                }
                //Humonoidanimator.RunAnimation(isRun);
            }
        }
        else
            base.Move();
    }
    bool MoveCheck(float hori, float vert)
    {
        bool moveResult = false;

        if (hori != 0 || vert != 0)
        {
            moveResult = true;
        }

        return moveResult;
    }

    public void CheckRushTime()
    {
        if (onRush && !rushEnd)
        {
            //���� ���� �ð�
            if (rushTimer > 0)
            {
                rushTimer -= Time.deltaTime;
            }
            else
            {
                RushEnd();
                rushTimer = rushTimeMax;
            }

            //���� ���� �ֱ�?
            if (canRushAttack && rushAtkTimer > 0)
            {
                rushAtkTimer -= Time.deltaTime;
            }
            else
            {
                canRushAttack = false;
                rushAtkTimer = rushAtkTimeMax;
            }
        }

        // ���� ���� ���ð�
        if (!readyRush)
        {
            if (rushCoolTimer > 0)
                rushCoolTimer -= Time.deltaTime;
            else
            {
                readyRush = true;
                rushCoolTimer = rushCoolTimeMax;
            }
        }
    }

    public override void Damaged(float damage)
    {
        base.Damaged(damage);
        if (onRush && PlayerHandler.instance.CantHandle)
        {            
            InitRush();
        }
    }

    void InitRush()
    {
        Humonoidanimator.ResetTrigger("RushStart");
        onRush = false;
        rushTimer = rushTimeMax;
        canRushAttack = false;
        rushAtkTimer = rushAtkTimeMax;
    }

    public override void PlayerJumpEvent()
    {
        if(!onRush)
            base.PlayerJumpEvent();
    }

    //���� ����
    public override void Attack()
    {
        if (ironAttack && attackInputValue < 1)
        {
            if (attackBufferTimer > 0 && readyRush)
            {
                Debug.Log("�����Լ� ������");
                attackBufferTimer = 0;
                attackInputValue = 1;
                ironAttack = false;
                if (!onRush)
                {
                    RushStart();
                }
                else
                {
                    RushEnd();
                }
            }
        }
    }

    public void RushStart()
    {
        PlayerHandler.instance.CantHandle = true;
        onRush = true;
        rushEnd = false;
        readyRush = true;
        Humonoidanimator.SetTrigger("RushStart");
    }

    public void RushEnd()
    {
        PlayerHandler.instance.CantHandle = true;
        //onRush = false;
        rushEnd = true;
        readyRush = false;
        Humonoidanimator.Play("RushEnd");
        StartCoroutine(RushEndCheck());
    }    

    IEnumerator RushEndCheck()
    {
        playerRb.velocity = new(0, playerRb.velocity.y, 0);
        yield return new WaitForSeconds(0.5f);

        AnimatorClipInfo[] clipGroup = Humonoidanimator.GetCurrentAnimatorClipInfo(0);
        Debug.Log($"rushend ���� ����ǰ��ִ� �ִϸ��̼�{clipGroup[0].clip.name}");
        if (Humonoidanimator.GetCurrentAnimatorStateInfo(0).IsName("RushEnd"))
        {
            while (Humonoidanimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                //Debug.Log($"���� ���� üũ{Humonoidanimator.GetCurrentAnimatorStateInfo(0).normalizedTime}");
                yield return null;
            }
            PlayerHandler.instance.CantHandle = false;
            ironAttack = true;
            onRush = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && downAttack)
        {
            PlayerHandler.instance.CantHandle = true;
            downEnd = true;
        }
    }
}
