using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKjs : Character
{
    #region 변수
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;

    public direction direction = direction.Right;

    [Header("근접 및 원거리 공격 관련")]
    public GameObject meleeCollider; // 근접 공격 콜라이더
    public GameObject flyCollider; // 공중 공격 콜라이더
    public GameObject downAttackCollider; // 내려찍기 콜라이더
    public Transform firePoint; // 원거리 및 특수공격 생성위치

    public Animator animator;

    public float moveValue; // 움직임 유무를 결정하기 위한 변수
    public float hori, vert; // 플레이어의 움직임 변수

    [Header("애니메이션 관련 변수")]
    public bool isJump, jumpAnim;
    public bool isRun;
    public bool isIdle;
    public bool isAttack;


    public bool onGround; // 지상 판정 유무
    public bool downAttack; // 내려찍기 공격 확인
    public float jumpLimit; // 점프 높이 제한하는 변수 velocity의 y값을 제한

    public bool attackSky; // 공중 공격
    public bool attackGround; // 지상 공격

    public Vector3 velocityValue; // 벨로시티값

    public bool onInvincible; // 무적 유무
    public bool onDash; // 대시 사용 가능 상태
    public bool isMove; // 이동 가능 상태

    public bool canAttack; // 공격 가능

    /*bool currentUp; // 뒤로 보게 만들기
    bool currentDown; // 앞으로 보게 만들기
    bool currentLeft; // 좌측 보게 만들기
    bool currentRight; // 우측 보게 만들기*/

    #endregion

    public float SizeX;
    public float SizeY;


    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        canAttack = true;
        onDash = true;
    }

    #region 레이 체크
    void jumpRaycastCheck()
    {
        if (!onGround)
        {
            RaycastHit hit;
            //if (playerRb.velocity.y <=0)
            //{
            Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
            if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 0.1f))
            {

                if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("InteractivePlatform"))
                {

                    if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("InteractivePlatform"))
                    {
                        onGround = true;
                        isJump = false;
                        //PlayerStat.instance.jumpCount = 0;
                        Debug.Log("BottomRayCheck");


                    }

                }
            }

        }
    }

    void wallRayCastCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position + Vector3.up * 0.1f + Vector3.forward * 0.1f * (int)direction, Vector3.forward * (int)direction, out hit, 0.1f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                wallcheck = true;
                Debug.Log("Blue Ray:" + hit.collider.name);
            }


        }
        else
        {
            wallcheck = false;
        }
    }

    #endregion

    public void HittedTest()
    {
        animator.SetTrigger("Damaged");
    }
    bool wallcheck;
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) { HittedTest(); }


        if (animator != null)
        {
            animator.SetBool("run", isRun);
            animator.SetBool("Onground", onGround);

        }
        else
        {
            Debug.Log("달리기 애니메이션 무응답");
        }

        wallRayCastCheck();
        InteractivePlatformrayCheck();
        InteractivePlatformrayCheck2();
        var a = RunEffect.main;

        if (isRun && onGround)
        {
            a.maxParticles = 100;
            if (!RunEffect.isPlaying)
                RunEffect.Play();

            Debug.Log("활성");
        }
        else
        {


            a.maxParticles = 0;
            if ((RunEffect.isPlaying && RunEffect.particleCount == 0))
                RunEffect.Stop();

        }

        if (CullingPlatform)
        {
            platformDisableTimer += Time.deltaTime;
            if (PlatformDisableTime <= platformDisableTimer)
            {
                platformDisableTimer = 0;
                CullingPlatform = false;
                Physics.IgnoreLayerCollision(6, 11, false);
            }
        }
    }
    public ParticleSystem RunEffect;
    Vector3 translateFix;

    #region 추상화 오버라이드 함수
    #region 이동
    public void rotate(float f)
    {
        if ((f == -1 && direction == direction.Right) || (f == 1 && direction == direction.Left))
        {
            this.transform.Rotate(new Vector3(0, 180, 0));
            if (direction == direction.Right)
                direction = direction.Left;
            else
                direction = direction.Right;
        }
    }
    public override void Move()
    {

        float hori = Input.GetAxisRaw("Horizontal");



        this.hori = hori;

        rotate(hori);


        translateFix = new(0, 0, Mathf.Abs(hori));


        if (wallcheck)
            transform.Translate(translateFix * PlayerStat.instance.moveSpeed * 0.05f * Time.deltaTime);
        else
            transform.Translate(translateFix * PlayerStat.instance.moveSpeed * Time.deltaTime);
        if (!isJump)
        {
            if (MoveCheck(hori, vert))
            {
                isRun = true;
            }
            else
            {
                isRun = false;
            }
            //animator.RunAnimation(isRun);
        }
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
    #endregion

    #region 공격
    public override void Attack()
    {
        if (PlayerStat.instance.attackType == AttackType.melee && canAttack && !downAttack)
        {
            if (!onGround)
            {
                attackSky = true;
            }
            else
            {
                attackGround = true;
            }
            Debug.Log("공격키");
            StartCoroutine(TestMeleeAttack());
        }
    }






    #region 내려찍기

    //public float DownAttackForce;

    public void DownAttack()
    {
        if (!downAttack)
        {
            downAttack = true;

            StartCoroutine(GoDownAttack());
        }
    }

    IEnumerator GoDownAttack()
    {
        playerRb.useGravity = false;
        playerRb.velocity = Vector3.zero;

        playerRb.AddForce(transform.up * 3f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f);
        playerRb.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.5f);

        downAttackCollider.SetActive(true);
        playerRb.useGravity = true;
        playerRb.AddForce(Vector3.down * PlayerStat.instance.downForce);
    }
    #endregion

    #region 특수공격
    public virtual void Skill1()
    {
        Debug.Log("s키를 이용한 스킬");
    }
    public virtual void Skill2()
    {

    }
    #endregion

    #endregion

    #region 피격
    public override void Damaged(float damage)
    {
        PlayerStat.instance.pState = PlayerState.hitted;

        PlayerStat.instance.hp -= damage;

        if (PlayerStat.instance.hp <= 0)
        {
            //Dead()
            PlayerStat.instance.hp = 0;
            Dead();
        }
        else
        {
            StartCoroutine(WaitEndDamaged());
        }
    }

    IEnumerator WaitEndDamaged()
    {
        if (animator != null)
        {
            animator.SetTrigger("Damaged");
        }

        playerRb.AddForce(-transform.forward * 1.2f, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);

        PlayerStat.instance.pState = PlayerState.idle;
    }

    #endregion

    #region 사망
    public override void Dead()
    {
        PlayerStat.instance.pState = PlayerState.dead;
        gameObject.SetActive(false);
    }
    #endregion
    #endregion

    #region 점프동작
    public void Jump()
    {
        if (!isJump)
        {
            //플랫폼에 닿았을 때 점프 가능(바닥,천장, 벽에 닿아도 점프 되지만 신경쓰지말기)
            isJump = true;
            if (animator != null)
            {
                animator.SetTrigger("jump");
            }
            isRun = false;
            /*if (PlayerStat.instance.jumpCount < PlayerStat.instance.jumpCountMax)
            {

                //YMove 

                playerRb.AddForce(Vector3.up * PlayerStat.instance.jumpForce, ForceMode.Impulse);
                PlayerStat.instance.jumpCount++;
            }*/
        }
    }

    public void jumphold()
    {
        if (playerRb.velocity.y > 0)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y * 0.85f, playerRb.velocity.z);
        }
    }

    #endregion


    #region Attack
    public void SwapAttackType()
    {
        PlayerStat ps = PlayerStat.instance;

        if (ps.attackType == AttackType.melee)
        {
            ps.attackType = AttackType.range;
        }
        else
        {
            ps.attackType = AttackType.melee;
        }
    }


    //애니메이션 없이 근접 공격
    IEnumerator TestMeleeAttack()
    {
        canAttack = false;
        if (attackSky)
        {
            flyCollider.SetActive(true);
            flyCollider.GetComponent<SphereCollider>().enabled = true;
        }
        else if (attackGround)
        {
            animator.SetTrigger("Attack");
            meleeCollider.SetActive(true);
            meleeCollider.GetComponent<SphereCollider>().enabled = true;
            playerRb.AddForce(transform.forward * 3, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);

        playerRb.velocity = Vector3.zero;
        if (attackSky)
        {
            flyCollider.SetActive(false);
            flyCollider.GetComponent<SphereCollider>().enabled = false;
            attackSky = false;
        }
        else if (attackGround)
        {
            meleeCollider.SetActive(false);
            meleeCollider.GetComponent<SphereCollider>().enabled = false;
            attackGround = false;
        }
        canAttack = true;
    }

    // 원거리 공격 함수

    //근접 공격 애니메이션
    public IEnumerator ActiveMeleeAttack()
    {
        meleeCollider.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        isAttack = false;
        animator.SetTrigger("Attack");
        meleeCollider.GetComponent<BoxCollider>().enabled = false;
    }
    #endregion

    #region 콜라이더 트리거
    private void OnCollisionExit(Collision collision)
    {
        #region 바닥 상호작용
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("InteractivePlatform"))

        {

            onGround = false;

        }
        #endregion
    }
    private void OnCollisionStay(Collision collision)
    {
        //#region 바닥 상호작용
        if (collision.gameObject.CompareTag("Ground") && onGround == false)
        {
            jumpRaycastCheck();

            downAttack = false;
        }
        //#endregion

        if (collision.gameObject.CompareTag("InteractivePlatform"))
        {
            Debug.Log("checkplaatform");
            jumpRaycastCheck();

            downAttack = false;

            if (Input.GetKey(KeyCode.DownArrow) && !CullingPlatform)
            {
                Debug.Log("DownArrowChk");
                CullingPlatform = true;
                Physics.IgnoreLayerCollision(6, 11, true);
            }
        }


        #region 적 상호작용
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (onInvincible)
            {
                Debug.Log("무적 상태입니다");
            }
        }
        #endregion
    }









    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack") && !onInvincible)
        {
            Damaged(other.GetComponent<EnemyMeleeAttack>().GetDamage(), other.gameObject);
        }
    }*/

    public float DownAttackForce;




    private void OnCollisionEnter(Collision collision)
    {
        //playerRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
    }
    #endregion

    #region 양방향 플랫폼
    public bool CullingPlatform;
    public float PlatformDisableTime;
    float platformDisableTimer;
    public void InteractivePlatformrayCheck2()
    {

        RaycastHit hit;
        //if (playerRb.velocity.y <=0)
        //{
        if (!CullingPlatform && playerRb.velocity.y > 0)
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.3f, Vector3.up * 0.1f, Color.green);

            if (Physics.Raycast(this.transform.position + Vector3.up * 0.3f, Vector3.up, out hit, 0.1f))
            {

                if (hit.collider.CompareTag("InteractivePlatform"))
                {

                    CullingPlatform = true;
                    Physics.IgnoreLayerCollision(6, 11, true);
                    Debug.Log("rayCheck1");

                }

            }

        }


    }
    public void InteractivePlatformrayCheck()
    {


        RaycastHit hit;
        //if ()
        //{
        if (CullingPlatform && playerRb.velocity.y <= 0)
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.3f, Vector3.up * 0.1f, Color.yellow);
            if (Physics.Raycast(this.transform.position + Vector3.up * 0.3f, Vector3.up, out hit, 0.1f))
            {

                if (hit.collider.CompareTag("InteractivePlatform"))
                {

                    CullingPlatform = false;
                    Physics.IgnoreLayerCollision(6, 11, false);
                    platformDisableTimer = 0;
                    Debug.Log("rayCheck");
                }

            }

        }
    }
    #endregion
}
