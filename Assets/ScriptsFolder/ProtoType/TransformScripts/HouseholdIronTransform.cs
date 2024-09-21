using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HouseholdIronTransform : Player
{
    [Header("�ٸ��� 2����")]
    public GameObject ironSecondForm;
    [Header("���� �̵��ӵ�")]
    public float rushSpeed, speedValue;
    float saveSpeed;
    [Header("���� ���ӽð�")]
    public float rushTimeMax;
    float rushTimer;
    [HideInInspector] public bool canRushAttack; // ���� ������ �޴� �ֱ�?�� ���� ����
    bool onRush;
    [Header("���� ���� ���ð�")]
    public float rushCoolTimeMax;
    float rushCoolTimer;
    [Header("���� ���� ���ط�")]
    public float rushDamage;
    [Header("���� ���� �ֱ�")]
    public float rushCycleMax;
    public float rushAtkTimeMax;
    public float rushAtkWaitTime;
    float rushCycleTimer, rushAtkTimer, rushAtkWaitTiemer;
    [Header("���� ��ȯ ������ �ð�")]
    public float rotateTimeMax;
    float rotateTimer;

    [Header("���� ĵ���� ���� ����")]
    public float rushRay;
    public float rayHeightValue;
    public float rayUpValue, rayMiddleValue, rayDownValue;

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

    public void RushRayCheck()
    {
        if (!Application.isPlaying || Application.isPlaying)
        {
            Debug.DrawRay(transform.GetChild(0).position + transform.GetChild(0).up * rayHeightValue, transform.GetChild(0).forward * rayUpValue, Color.red, 0.2f);
            Debug.DrawRay(transform.GetChild(0).position, transform.GetChild(0).forward * rayMiddleValue, Color.red, 0.2f);
            Debug.DrawRay(transform.GetChild(0).position - transform.GetChild(0).up * rayHeightValue, transform.GetChild(0).forward * rayDownValue, Color.red, 0.2f);
        }
        RaycastHit upRay;
        RaycastHit middleRay;
        RaycastHit downRay;
        if (onRush)
        {


            if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).forward, out middleRay, rayMiddleValue, LayerMask.GetMask("Platform")))
            {
                Debug.Log("�߰� ���� �浹");
                RushCancel();
            }
            else if (Physics.Raycast(transform.GetChild(0).position + transform.GetChild(0).up * rayHeightValue, transform.GetChild(0).forward, out upRay, rayUpValue, LayerMask.GetMask("Platform")))
            {
                Debug.Log($"���� ���� �浹, �浹�� ������Ʈ:{upRay.collider}");
                RushCancel();
            }
            else if (Physics.Raycast(transform.GetChild(0).position - transform.GetChild(0).up * rayHeightValue, transform.GetChild(0).forward, out downRay, rayDownValue, LayerMask.GetMask("Platform")))
            {
                Debug.Log("�Ʒ��� ���� �浹");
                RushCancel();
            }
        }
    }

    public void RushCancel()
    {
        Humonoidanimator.SetTrigger("RushCancel");
        SecondFormDeactive();
        onRush = false;
        rushEnd = true;
        rushTimer = rushTimeMax;
        rushAtkTimer = rushAtkTimeMax;
        rushCoolTimer = rushCoolTimeMax;
        readyRush = false;
    }

    private void OnDrawGizmos()
    {
        RushRayCheck();
    }
    private void Update()
    {
        BaseBufferTimer();
        CheckRushTime();
        RushRayCheck();
        IronDownAttackTimeCheck();
    }

    public override void DownAttack()
    {
        if (onDownCoolTime || onRush)
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
        SecondFormActive();

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
                SecondFormDeactive();
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
                    Vert = Input.GetAxisRaw("Vertical");
                    hori = Input.GetAxisRaw("Horizontal");

                    break;
                case PlayerMoveState.XZMove3DReverse:
                    Vert = -1 * Input.GetAxisRaw("Vertical");
                    hori = -1 * Input.GetAxisRaw("Horizontal");

                    break;
                case PlayerMoveState.ZXMove3D:
                    hori = Input.GetAxisRaw("Vertical");
                    Vert = -1 * Input.GetAxisRaw("Horizontal");
                    break;
                case PlayerMoveState.ZXMove3DReverse:
                    hori = -1 * Input.GetAxisRaw("Vertical");
                    Vert = Input.GetAxisRaw("Horizontal");
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
                playerRb.AddForce(Vector3.zero, ForceMode.VelocityChange);

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
            }

            //���� ���� �ֱ�?
            if (rushAtkTimer > 0)
            {
                rushAtkTimer -= Time.deltaTime;
            }
            else
            {
                meleeCollider.SetActive(true);
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
    public override void TransformDamagedEvent()
    {
        InitRush();
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
        if (downAttack || PlayerStat.instance.pState == PlayerState.hitted)
            return;
        else
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
    }

    public void RushStart()
    {
        PlayerHandler.instance.CantHandle = true;
        onInvincible = true;        
        readyRush = true;
        Humonoidanimator.SetTrigger("RushStart");
        StartCoroutine(RushStartCheck());
    }

    IEnumerator RushStartCheck()
    {
        yield return new WaitForSeconds(0.5f);

        if (Humonoidanimator.GetCurrentAnimatorStateInfo(0).IsName("RushStart"))
        {
            while (Humonoidanimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            SecondFormActive();
            onRush = true;
            rushEnd = false;
            ironAttack = true;
            readyRush = true;
        }
    }

    public void RushEnd()
    {
        PlayerHandler.instance.CantHandle = true;
        //onRush = false;
        meleeCollider.SetActive(false);
        rushEnd = true;
        readyRush = false;
        rushTimer = rushTimeMax;
        SecondFormDeactive();
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
            onInvincible = false;
            ironAttack = true;
            onRush = false;            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            if (downAttack)
            {
                PlayerHandler.instance.CantHandle = true;
                downEnd = true;
            }
        }
    }

    #region ���� 2���¿� ���� ó��
    public void SecondFormActive()
    {
        Destroy(Instantiate(changeEffect, transform.position, Quaternion.identity), 2f);
        for (int i = 0; i < Humonoidanimator.transform.childCount; i++)
        {
            Humonoidanimator.transform.GetChild(i).gameObject.SetActive(false);
        }
        ironSecondForm.SetActive(true);
    }

    public void SecondFormDeactive()
    {
        Destroy(Instantiate(changeEffect, transform.position, Quaternion.identity), 2f);
        for (int i = 0; i < Humonoidanimator.transform.childCount; i++)
        {
            Humonoidanimator.transform.GetChild(i).gameObject.SetActive(true);
        }
        ironSecondForm.SetActive(false);
    }
    #endregion
}
