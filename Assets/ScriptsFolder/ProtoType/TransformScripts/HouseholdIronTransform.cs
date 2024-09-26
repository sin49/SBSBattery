 using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HouseholdIronTransform : Player
{
    [Header("다리미 2형태")]
    public GameObject ironSecondForm;
    [Header("돌진 이동속도")]
    public float rushSpeed, speedValue;
    float saveSpeed;
    [Header("돌진 지속시간")]
    public float rushTimeMax;
    float rushTimer;
    [HideInInspector] public bool canRushAttack; // 돌진 공격을 받는 주기?를 위한 변수
    bool onRush;
    [Header("돌진 재사용 대기시간")]
    public float rushCoolTimeMax;
    float rushCoolTimer;
    [Header("돌진 공격 피해량")]
    public float rushDamage;
    [Header("돌진 공격 주기")]
    public float rushCycleMax;
    public float rushAtkTimeMax;
    public float rushAtkWaitTime;
    float rushCycleTimer, rushAtkTimer, rushAtkWaitTiemer;
    [Header("방향 전환 딜레이 시간")]
    public float rotateTimeMax;
    float rotateTimer;
    [Header("돌진 이펙트")]
    public ParticleSystem ironDashEffect;
    [Header("돌진 캔슬을 위한 레이")]
    public float rushRay;
    public float rayHeightValue;
    public float rayUpValue, rayMiddleValue, rayDownValue;

    bool readyRush, downEnd, rushEnd;
    bool ironAttack = true;

    [Header("특수 능력 관련 변수\n")]
    [Header("다림질 찍기 속도")]public float downAtkSpeed;
    [Header("다림질 체공 시간")]public float ironFlyTime;
    [Header("찍기 후 조작 불가 시간\n(이 때, 무적 적용됨)")]
    public float downAtkEndTimeMax;
    float downAtkEndTimer;
    [Header("다림질 공격력")]public float ironDownDamage;
    [Header("다림질 재사용 대기 시간")]public float ironDownCoolTimeMax;
    [Header("다림질 이펙트")] public ParticleSystem ironDownAtkEffect;
    float downCoolTimer;
    bool onDownCoolTime;

    float rushHori, rushVert;
    public direction saveDirection = direction.none;
    public directionZ saveDirectionZ = directionZ.none;
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
                Debug.Log("중간 레이 충돌");
                RushCancel();
            }
            else if (Physics.Raycast(transform.GetChild(0).position + transform.GetChild(0).up * rayHeightValue, transform.GetChild(0).forward, out upRay, rayUpValue, LayerMask.GetMask("Platform")))
            {
                Debug.Log($"위쪽 레이 충돌, 충돌한 오브젝트:{upRay.collider}");
                RushCancel();
            }
            else if (Physics.Raycast(transform.GetChild(0).position - transform.GetChild(0).up * rayHeightValue, transform.GetChild(0).forward, out downRay, rayDownValue, LayerMask.GetMask("Platform")))
            {
                Debug.Log("아래쪽 레이 충돌");
                RushCancel();
            }
        }
    }

    public void RushCancel()
    {
        Humonoidanimator.SetTrigger("RushCancel");
        ironDashEffect.Stop();
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
                PlayerHandler.instance.CantHandle = true;
                StartFreeze();
                StartCoroutine(IronDownAttack());
            }
        }
    }
    IEnumerator IronDownAttack()
    {
        SecondFormActive();

        playerRb.useGravity = false;
        while (playerRb.velocity != Vector3.zero)
        {
            playerRb.velocity = Vector3.zero;
            yield return null;
        }
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(transform.up * 30f);

        yield return new WaitForSeconds(ironFlyTime);

        EndFreeze();
        playerRb.useGravity = true;
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(-transform.up * downAtkSpeed, ForceMode.Impulse);
        downAttackCollider.SetActive(true);
    }

    public void StartFreeze()
    {
        playerRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    public void EndFreeze()
    {
        playerRb.constraints = RigidbodyConstraints.FreezeRotation;
    }


    // 다림질 시간 관련 체크
    public void IronDownAttackTimeCheck()
    {
        //다림질 찍기 후 유지시간 -> 조작이 불가능하도록 함
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

        //다림질 찍기 유지시간 이후부터
        //재사용 대기시간 적용함
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
                    rushVert = 0;
                    if (Input.GetKey(KeyCode.RightArrow))
                        rushHori = 1;
                    else if (Input.GetKey(KeyCode.LeftArrow))
                        rushHori = -1;
                    break;
                case PlayerMoveState.XmoveReverse:
                    hori = -1 * Input.GetAxisRaw("Horizontal");

                    rushVert = 0;
                    if (Input.GetKey(KeyCode.RightArrow))
                        rushHori = -1;
                    else if (Input.GetKey(KeyCode.LeftArrow))
                        rushHori = 1;
                    break;

                case PlayerMoveState.Zmove:
                    Vert = Input.GetAxisRaw("Horizontal");
                    ZmoveRushVert();
                    break;
                case PlayerMoveState.ZmoveReverse:
                    Vert = -1 * Input.GetAxisRaw("Horizontal");

                    rushHori = 0;
                    ZmoveRushVert();
                    rushVert = -rushVert;
                    break;
                case PlayerMoveState.XZMove3D:
                    Vert = Input.GetAxisRaw("Vertical");
                    hori = Input.GetAxisRaw("Horizontal");
                    XZmoveRushHorizontal();
                    XZmoveRushVertical();
                    break;
                case PlayerMoveState.XZMove3DReverse:
                    Vert = -1 * Input.GetAxisRaw("Vertical");
                    hori = -1 * Input.GetAxisRaw("Horizontal");

                    XZmoveRushHorizontal();
                    rushHori = -rushHori;
                    XZmoveRushVertical();
                    rushVert = -rushVert;
                    break;
                case PlayerMoveState.ZXMove3D:
                    hori = Input.GetAxisRaw("Vertical");
                    Vert = -1 * Input.GetAxisRaw("Horizontal");
                    
                    ZXmoveRushHorizontal();
                    ZXmoveRushVertical() ;
                    break;
                case PlayerMoveState.ZXMove3DReverse:
                    hori = -1 * Input.GetAxisRaw("Vertical");
                    Vert = Input.GetAxisRaw("Horizontal");

                    ZXmoveRushHorizontal();
                    ZXmoveRushVertical();

                    rushHori = -rushHori;
                    rushVert = -rushVert;
                    break;
            }

            Vector3 moveInput = new Vector3(hori, 0, Vert);
            Vector3 regularMove = new Vector3(rushHori, 0, rushVert);
            if (rushHori != 0 || rushVert != 0)
            {
                RushRotate(regularMove.x, regularMove.z);
                //SoundPlayer.PlayMoveSound();
            }

            if (!onRushRot)
            {
                Vector3 moveVelocity = Vector3.zero;
                Vector3 vector = regularMove.normalized * rushSpeed;
                Vector3 forwardForce = transform.GetChild(0).forward * rushSpeed;
                moveVelocity = forwardForce - playerRb.velocity.x * Vector3.right - playerRb.velocity.z * Vector3.forward;
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

    public float rotateTime;
    float rushRotateSpeed = 4.5f;
    bool onRushRot, onlyRot;
    Vector3 currentRotateVector;
    // 돌진 회전
    public void RushRotate(float hori, float vert)
    {
        Vector3 rotateVector = Vector3.zero;
        Vector3 saveRotateVector = Vector3.zero;
        // Check horizontal and vertical inputs and determine the direction
        if (hori == 1)
        {
            saveDirection = direction;
            direction = direction.Right;
        }
        else if (hori == -1)
        {
            saveDirection = direction;
            direction = direction.Left;
        }
        else
            direction = direction.none;
        if (vert == 1)
        {
            saveDirectionZ = directionz;
            directionz = directionZ.back;
        }
        else if (vert == -1)
        {
            saveDirectionZ = directionz;
            directionz = directionZ.forward;
        }
        else
            directionz = directionZ.none;

        //PlayerStat.instance.Trans3D
        //PlayerStat.instance.direction = direction;
        if (hori == -1 && vert == 0) // Left
        {
            rotateVector = new Vector3(0, 180, 0);
            if (saveDirection == direction.Right)
            {
                saveRotateVector = new Vector3(0, 0, 0);
            }
        }
        else if (hori == 1 && vert == 0) // Right
        {
            rotateVector = new Vector3(0, 0, 0);
            if (saveDirection == direction.Left)
            {
                saveRotateVector = new Vector3(0, 180, 0);
            }
        }
        else if (hori == 0 && vert == 1) // Up
        {
            rotateVector = new Vector3(0, -90, 0);
            if (saveDirectionZ == directionZ.back)
            {
                saveRotateVector = new Vector3(0, 90, 0);
            }
        }
        else if (hori == 0 && vert == -1) // Down
        {
            rotateVector = new Vector3(0, 90, 0);
            if (saveDirectionZ == directionZ.forward)
            {
                saveRotateVector = new Vector3(0, -90, 0);
            }
        }
        else if (hori == -1 && vert == 1) // UpLeft
        {
            rotateVector = new Vector3(0, -135, 0);
            if (saveDirectionZ == directionZ.back && saveDirection == direction.Right)
            {
                saveRotateVector = new Vector3(0, 45, 0);
            }

        }
        else if (hori == 1 && vert == 1) // UpRight
        {
            rotateVector = new Vector3(0, -45, 0);
            if (saveDirectionZ == directionZ.back && direction == direction.Left)
            {
                saveRotateVector = new Vector3(0, 135, 0);
            }
        }
        else if (hori == -1 && vert == -1) // DownLeft
        {
            rotateVector = new Vector3(0, 135, 0);
            if (saveDirectionZ == directionZ.forward && saveDirection == direction.Right)
            {
                saveRotateVector = new Vector3(0, -45, 0);
            }
        }
        else if (hori == 1 && vert == -1) // DownRight
        {
            rotateVector = new Vector3(0, 45, 0);
            if (saveDirectionZ == directionZ.forward && saveDirection == direction.Left)
            {
                saveRotateVector = new Vector3(0, -135, 0);
            }
        }
        rotateVector += new Vector3(0, 90, 0);

        transform.GetChild(0).rotation = Quaternion.Euler(rotateVector);
    }
    bool inputHori, inputVert;
    float saveHori, saveVert;
    /*public void RushRotate(float hori, float vert)
    {
        if (onRushRot || )
            return;
        Vector3 rotateVector = Vector3.zero;

        ReverseDirection();
        // Check horizontal and vertical inputs and determine the direction
        if (hori == 1)
        {
            if (saveHori == -hori)
            {
                saveHori = hori;
                saveDirection = direction;
            }
            direction = direction.Right;
        }
        else if (hori == -1)
        {
            if (saveHori == -hori)
            {
                saveHori = hori;
                saveDirection = direction;
            }
            direction = direction.Left;
        }
        else
        {                       
            if (saveHori != hori && saveHori == -hori)
            {
                saveHori = hori;
                saveDirection = direction;
            }
            direction = direction.none;
        }
        if (vert == 1)
        {
            if (saveVert == -vert)
            {
                saveVert = vert;
                saveDirectionZ = directionz;
            }
            directionz = directionZ.back;
        }
        else if (vert == -1)
        {
            if (saveVert == -vert)
            {
                saveVert = vert;
                saveDirectionZ = directionz;
            }
            directionz = directionZ.forward;
        }
        else
        {
            if (saveVert != vert && saveVert == -vert)
            {
                saveVert = vert;
                saveDirectionZ = directionz;
            }
            directionz = directionZ.none;
        }

        //PlayerStat.instance.Trans3D
        //PlayerStat.instance.direction = direction;
        if (hori == -1 && vert == 0) // Left
        {
            rotateVector = new Vector3(0, 180, 0);


        }
        else if (hori == 1 && vert == 0) // Right
        {
            rotateVector = new Vector3(0, 0, 0);

        }
        else if (hori == 0 && vert == 1) // Up
        {
            rotateVector = new Vector3(0, -90, 0);

        }
        else if (hori == 0 && vert == -1) // Down
        {
            rotateVector = new Vector3(0, 90, 0);

        }
        else if (hori == -1 && vert == 1) // UpLeft
        {
            rotateVector = new Vector3(0, -135, 0);

        }
        else if (hori == 1 && vert == 1) // UpRight
        {
            rotateVector = new Vector3(0, -45, 0);

        }
        else if (hori == -1 && vert == -1) // DownLeft
        {
            rotateVector = new Vector3(0, 135, 0);

        }
        else if (hori == 1 && vert == -1) // DownRight
        {
            rotateVector = new Vector3(0, 45, 0);

        }
        rotateVector += new Vector3(0, 90, 0);
        currentRotateVector = transform.GetChild(0).eulerAngles;
        //transform.GetChild(0).rotation = Quaternion.Euler(rotateVector);
        if (onlyRot)
        {
            if (!onRushRot)
            {
                onRushRot = true;
                StartCoroutine(RushRotation(rotateVector));
            }
        }
        else
        {
            transform.GetChild(0).rotation = Quaternion.Euler(rotateVector);
        }
    }*/

    public void ReverseDirection()
    {
        /*if (saveDirection == direction.Right && saveDirectionZ == directionZ.forward)
        {
            if (direction == direction.Left && directionz == directionZ.back)
            {
                onlyRot = true;
            }
        }
        else if (saveDirection == direction.Left && saveDirectionZ == directionZ.back)
        {
            if (direction == direction.Right && directionz == directionZ.forward)
            {
                onlyRot = true;
            }
        }
        else */if (saveDirection == direction.Right && direction == direction.Left)
        {
            onlyRot = true;
        }
        else if (saveDirectionZ == directionZ.forward && directionz == directionZ.back)
        {
            onlyRot = true;
        }
        else if (saveDirection == direction.Left && direction == direction.Right)
        {
            onlyRot = true;
        }
        else if (saveDirectionZ == directionZ.back && directionz == directionZ.forward)
        {
            onlyRot = true;
        }
        else
        {
            onlyRot = false;
        }

        /*if (saveDirection == direction.Right && direction == direction.Left)
        {
            onlyRot = true;
        }
        else if (saveDirectionZ == directionZ.forward && directionz == directionZ.back)
        {
            onlyRot = true;
        }
        else if (saveDirection == direction.Left && direction == direction.Right)
        {
            onlyRot = true;
        }
        else if (saveDirectionZ == directionZ.back && directionz == directionZ.forward)
        {
            onlyRot = true;
        }
        else if (saveDirection == direction.Right && saveDirectionZ == directionZ.forward)
        {
            if (direction == direction.Left && directionz == directionZ.back)
            {
                onlyRot = true;
            }
        }
        else if (saveDirection == direction.Left && saveDirectionZ == directionZ.back)
        {
            if (direction == direction.Right && directionz == directionZ.forward)
            {
                onlyRot = true;
            }
        }
        else
        {
            onlyRot = false;
        }            */
    }

    IEnumerator RushRotation(Vector3 lastVector)
    {
        Debug.Log($"회전 코루틴 호출{(lastVector - currentRotateVector).magnitude}");
        float rotateSpeed = (lastVector - currentRotateVector).magnitude / rotateTime;
        Debug.Log($"돌진 회전 속도{rotateSpeed}");
        float timer=0;
        while (timer < rotateTime)
        {
            transform.GetChild(0).Rotate(0, rotateSpeed * Time.fixedDeltaTime, 0);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.GetChild(0).rotation = Quaternion.Euler(lastVector);

        yield return new WaitForSeconds(.5f);
        onRushRot = false;

    }

    public void CheckRushTime()
    {
        if (onRush && !rushEnd)
        {
            //돌진 지속 시간
            if (rushTimer > 0)
            {
                rushTimer -= Time.deltaTime;
            }
            else
            {
                RushEnd();
            }

            //돌진 공격 주기?
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

        // 돌진 재사용 대기시간
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
        SecondFormDeactive();
    }

    public override void PlayerJumpEvent()
    {
        if(!onRush)
            base.PlayerJumpEvent();
    }

    #region 돌진 관련
    //돌진 공격
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
                    Debug.Log("공격함수 실행중");
                    attackBufferTimer = 0;
                    attackInputValue = 1;
                    ironAttack = false;

                    rushHori = 0;
                    rushVert = 0;

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
        saveDirection = direction;
        saveDirectionZ = directionz;
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
            if (!ironDashEffect.gameObject.activeSelf)
            {
                ironDashEffect.gameObject.SetActive(true);
            }
            ironDashEffect.Play();
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
        ironDashEffect.Stop();
        SecondFormDeactive();
        Humonoidanimator.Play("RushEnd");
        StartCoroutine(RushEndCheck());
    }    

    IEnumerator RushEndCheck()
    {
        playerRb.velocity = new(0, playerRb.velocity.y, 0);
        yield return new WaitForSeconds(0.5f);

        AnimatorClipInfo[] clipGroup = Humonoidanimator.GetCurrentAnimatorClipInfo(0);
        Debug.Log($"rushend 현재 재생되고있는 애니메이션{clipGroup[0].clip.name}");
        if (Humonoidanimator.GetCurrentAnimatorStateInfo(0).IsName("RushEnd"))
        {
            while (Humonoidanimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                //Debug.Log($"돌진 종료 체크{Humonoidanimator.GetCurrentAnimatorStateInfo(0).normalizedTime}");
                yield return null;
            }
            PlayerHandler.instance.CantHandle = false;
            onInvincible = false;
            ironAttack = true;
            onRush = false;            
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            if (downAttack)
            {
                PlayerHandler.instance.CantHandle = true;
                downEnd = true;
                if (!ironDownAtkEffect.gameObject.activeSelf)
                {
                    ironDownAtkEffect.gameObject.SetActive(true);
                    ironDownAtkEffect.Play();
                }
            }
        }
    }

    #region 변신 2형태에 대한 처리
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

    #region 돌진 방향    

    // XZ move 관련
    float XZmoveRushHorizontal()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rushHori = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rushHori = -1;
        }

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                rushHori = 0;
            }
        }

        return rushHori;
    }

    float XZmoveRushVertical()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rushVert = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rushVert = -1;
        }

        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                rushVert = 0;
            }
        }

        return rushVert;
    }

    // ZX move 관련
    public void ZXmoveRushHorizontal()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rushHori = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rushHori = -1;
        }

        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                rushHori = 0;
            }
        }
    }

    public void ZXmoveRushVertical()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rushVert = -1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rushVert = 1;
        }

        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                rushVert = 0;
            }
        }
    }

    public void XmoveRushHori()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rushHori = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rushHori = -1;
        }
    }

    public void ZmoveRushHori()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rushHori = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rushHori = -1;
        }
    }

    public void XmoveRushVert()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rushVert = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rushVert = -1;
        }
    }

    public void ZmoveRushVert()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rushVert = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rushVert = -1;
        }
    }
    #endregion
}
