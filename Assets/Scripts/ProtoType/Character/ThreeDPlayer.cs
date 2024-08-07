using System.Collections;
using UnityEngine;

public class ThreeDPlayer : Character
{
    //public static Player instance;
    #region 변수
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;
    public PlayerAnim animator;

    [Header("근접 및 원거리 공격 관련")]
    public GameObject meleeCollider;
    public Transform firePoint;
    public GameObject rangePrefab;
    public GameObject sAttackPrefab; // 특수공격 파티클
    public GameObject IronDash; // 다리미 특수공격 대쉬 테스트를 위한 오브젝트 (추후 개선 사항)

    public float moveValue; // 움직임 유무를 결정하기 위한 변수
    public float hori, vert; // 플레이어의 움직임 변수

    [Header("애니메이션 관련 변수")]
    public bool isJump, jumpAnim;
    public bool isRun;
    public bool isIdle;
    public bool isAttack;


    public bool onGround; // 지상 판정 유무
    public float jumpLimit; // 점프 높이 제한하는 변수 velocity의 y값을 제한

    public Vector3 velocityValue; // 벨로시티값

    public bool onInvincible; // 무적 유무
    public bool onDash; // 대시 사용 가능 상태
    public bool isMove; // 이동 가능 상태


    bool currentUp; // 뒤로 보게 만들기
    bool currentDown; // 앞으로 보게 만들기
    bool currentLeft; // 좌측 보게 만들기
    bool currentRight; // 우측 보게 만들기

    #endregion
    /*private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }*/

    // Start is called before the first frame update
    void Start()
    {
        //meleeCollider.GetComponent<MeleeCollider>().SetDamage(PlayerStat.instance.atk);
        onDash = true;
    }



    private void FixedUpdate()
    {

        if (!onGround)
        {
            isJump = true;
            //if (playerRb.velocity.y > jumpLimit)
            //{
            //    playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y * 0.6f, playerRb.velocity.z);
            //}
        }
        else
        {
            isJump = false;
        }
    }

    Vector3 translateFix;

    #region move
    public void P_Move()
    {
        //translate우선
        //rigidbody 건
        //ZMove도 구현해놓기
        //left right arrow Xmove

        float hori = Input.GetAxisRaw("Horizontal");
        ////Up Down arrow Zmove
        float vert = Input.GetAxisRaw("Vertical");

        this.hori = hori;
        this.vert = vert;

        //Vector3 posFix = new(hori, 0, vert);

        translateFix = new(vert, 0, hori);

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
            animator.RunAnimation(isRun);
        }

        //translate우선
        //transform.Translate(PlayerStat.instance.moveSpeed * Time.deltaTime * translateFix.normalized);
        /*if (hori < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }*/

        //rigidbody 건 (미구현)

        //Vector3 lookValue = transform.position + posFix.normalized * moveSpeed * Time.deltaTime;
        //Debug.Log(transform.position + posFix);
        //transform.Translate(moveVec);

        /*transform.position += posFix.normalized * moveSpeed * Time.deltaTime;
        
        if (!isJump)
        {
            transform.LookAt(transform.position + posFix);
        }*/
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

    public void Jump()
    {

        if (isJump == false)
        {
            //플랫폼에 닿았을 때 점프 가능(바닥,천장, 벽에 닿아도 점프 되지만 신경쓰지말기)
            onGround = false;
            jumpAnim = true;
            //moveSpeed = ;
            animator.JumpAnimation(jumpAnim);
            playerRb.velocity = Vector3.zero;
            //addforce
            //YMove 
            playerRb.AddForce(Vector3.up * 16, ForceMode.Impulse);
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

        /*if (attackType == AttackType.melee)
        {
            attackType = AttackType.range;
        }
        else
        {
            attackType = AttackType.melee;
        }*/
    }

    /*public virtual void Attack()
    {
        if (PlayerStat.instance.attackType == AttackType.range)
            RangeAttack();
        else
            MeleeAttack();
    }*/

    void MeleeAttack()
    {
        //ColliSion Prefab 활성화 시키는 걸로?
        isAttack = true;
        StartCoroutine(TestMeleeAttack());
        //animator.AttackAnimation(isAttack);
    }

    //애니메이션 없이 근접 공격
    IEnumerator TestMeleeAttack()
    {
        meleeCollider.GetComponent<SphereCollider>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        isAttack = false;

        meleeCollider.GetComponent<SphereCollider>().enabled = false;
    }

    // 원거리 공격 함수
    void RangeAttack()
    {
        //Collision prefab instaiate 시키는 걸로?
        GameObject rangeObj = Instantiate(rangePrefab, firePoint.position, Quaternion.identity);
        //rangeObj.GetComponent<RangeObject>().SetDamage(PlayerStat.instance.atk);
    }

    //근접 공격 애니메이션
    public IEnumerator ActiveMeleeAttack()
    {
        meleeCollider.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        isAttack = false;
        animator.AttackAnimation(isAttack);
        meleeCollider.GetComponent<BoxCollider>().enabled = false;
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.ContinueAnimation();
            Debug.Log("바닥 체크");
            onGround = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (onInvincible)
            {
                Debug.Log("무적 상태입니다");
            }
            else
            {
                Damaged(collision.gameObject.GetComponent<Enemy>().eStat.atk);
                if (PlayerStat.instance.hp <= 0)
                {
                    //피해를 받음
                    //PlayerStat.instance.Damaged(collision.gameObject.GetComponent<Enemy>().eStat.atk, collision.gameObject);
                    //if(Hp0)

                    PlayerStat.instance.hp = 0;
                    Dead();
                }
            }
        }
    }

    public IEnumerator WaitAndFalseAnimation(string aniBool, float animationTime)
    {
        yield return new WaitForSeconds(animationTime);

        switch (aniBool)
        {
            case "isJump":
                Debug.Log("점프 값 false로 변환 가능한 상태");
                jumpAnim = false;
                animator.JumpAnimation(jumpAnim);
                //PlayerStat.instance.moveSpeed = 10f;
                //moveSpeed = 10f;
                break;
        }
    }

    public override void Attack()
    {
        if (PlayerStat.instance.attackType == AttackType.melee)
        {
            StartCoroutine(TestMeleeAttack());
        }
        else
        {
            RangeAttack();
        }
    }

    public override void Damaged(float damage)
    {
        PlayerStat.instance.hp -= damage;
        
        if (PlayerStat.instance.hp <= 0)
        {
            //Dead()
            PlayerStat.instance.hp = 0;
            Dead();
        }
    }

    public override void Move()
    {
        //translate우선
        //rigidbody 건
        //ZMove도 구현해놓기
        //left right arrow Xmove

        float hori = Input.GetAxisRaw("Horizontal");
        ////Up Down arrow Zmove
        float vert = Input.GetAxisRaw("Vertical");

        this.hori = hori;
        this.vert = vert;

        //Vector3 posFix = new(hori, 0, vert);

        translateFix = new(hori, 0, vert);

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
            animator.RunAnimation(isRun);
        }

        #region 이동 함수 작성 중
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //키 입력들에 대한 bool값을 받아 바라보는 방향을 고정시키도록 결정
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.eulerAngles.y >= 185f && transform.eulerAngles.y <= 300f)
                {
                    if (transform.eulerAngles.y >= 305f && transform.eulerAngles.y <= 320f)
                    {
                        transform.rotation = Quaternion.Euler(0, -45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, 30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                else if (transform.eulerAngles.y >= 270f && transform.eulerAngles.y <= 360)
                {
                    if (transform.eulerAngles.y >= 305f && transform.eulerAngles.y <= 320f)
                    {
                        transform.rotation = Quaternion.Euler(0, -45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, -30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                transform.Translate(Vector3.forward.normalized * PlayerStat.instance.moveSpeed * Time.deltaTime);
                Debug.Log(transform.eulerAngles.y);
            } // 오일러 각도  180 <= Y <=359.9f일 때
            else if (Input.GetKey(KeyCode.RightArrow)) // Up키 + right키
            {
                if (transform.eulerAngles.y >= -20 && transform.eulerAngles.y <= 95f)
                {
                    if (transform.eulerAngles.y >= 35f && transform.eulerAngles.y <= 48f)
                    {
                        transform.rotation = Quaternion.Euler(0, 45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, 30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                else if (transform.eulerAngles.y >= 270f && transform.eulerAngles.y <= 360)
                {
                    if (transform.eulerAngles.y >= 305f && transform.eulerAngles.y <= 320f)
                    {
                        transform.rotation = Quaternion.Euler(0, 45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, -30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                transform.Translate(Vector3.forward.normalized * PlayerStat.instance.moveSpeed * Time.deltaTime);
            }
            else if (transform.eulerAngles.y <= 359.9 && transform.eulerAngles.y >= 180)
            {
                // -20 <= y <= 5.5f 또는 355f <= y < 359.99f
                if (transform.eulerAngles.y >= -20 && transform.eulerAngles.y <= 5.5f || transform.eulerAngles.y < 359.99f && transform.eulerAngles.y >= 355f)
                {
                    // 카메라 기준 정면 고정
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    // 정면이 될 때까지 회전
                    transform.Rotate(0, 30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                }
                transform.Translate(new Vector3(0, 0, 1).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
            }
            else if (transform.eulerAngles.y < 180f && transform.eulerAngles.y >= -20)
            {
                if (transform.eulerAngles.y >= -20 && transform.eulerAngles.y <= 5.5f || transform.eulerAngles.y < 360f && transform.eulerAngles.y >= 355f)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.Rotate(0, -30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                }
                transform.Translate(new Vector3(0, 0, 1).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
            }
            else
            {

            }
            //Debug.Log($"up 키 입력 rotation.eulerAngles.y 값: {transform.eulerAngles.y}");
            // == transform.Translate(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
            // == playerRb.AddForce(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        // 아래키
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, 0, -1).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
            if (transform.eulerAngles.y <= 360 && transform.eulerAngles.y >= 180)
            {
                if (transform.eulerAngles.y <= 185f && transform.eulerAngles.y >= 175f)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.Rotate(0, -50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                }
            }
            else if (transform.eulerAngles.y < 180f && transform.eulerAngles.y >= -20f)
            {
                if (transform.eulerAngles.y <= 185f && transform.eulerAngles.y >= 175f)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.Rotate(0, 50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                }
            }
            //Debug.Log($"down 키 입력 rotation.eulerAngles.y 값: {transform.eulerAngles.y}");
        }
        // 좌키
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (transform.eulerAngles.y >= 185f && transform.eulerAngles.y <= 300f)
                {
                    if (transform.eulerAngles.y >= 305f && transform.eulerAngles.y <= 320f)
                    {
                        transform.rotation = Quaternion.Euler(0, -45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, 30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                else if (transform.eulerAngles.y >= 270f && transform.eulerAngles.y <= 360)
                {
                    if (transform.eulerAngles.y >= 305f && transform.eulerAngles.y <= 320f)
                    {
                        transform.rotation = Quaternion.Euler(0, -45, 0);
                    }
                    else
                    {
                        transform.Rotate(0, -30f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    }
                }
                transform.Translate(Vector3.forward.normalized * PlayerStat.instance.moveSpeed * Time.deltaTime);
                Debug.Log(transform.eulerAngles.y);
            }
            else if (transform.eulerAngles.y >= 265.5f && transform.eulerAngles.y <= 275.5f)
            {

                transform.rotation = Quaternion.Euler(0, -90, 0);

                //Debug.Log("이제 좌측 키를 입력해도 이동하게 됩니다.");
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
                }
            }
            else
            {
                if (transform.eulerAngles.y >= 175f && transform.eulerAngles.y <= 270f)
                {
                    transform.Rotate(0, 50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"좌측으로 회전합니다\nY축 Quaternion 값: {transform.rotation.y}, euler값: {transform.eulerAngles.y}");
                }
                else
                {
                    transform.Rotate(0, -50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"좌측으로 회전합니다\nY축 Quaternion 값: {transform.rotation.y}, euler값: {transform.eulerAngles.y}");
                }
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(new Vector3(-1, 0, 0).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
                }
            }
        }
        //transform.Rotate()
        // rotation
        //우키
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.eulerAngles.y >= 85 && transform.eulerAngles.y <= 95)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                //Debug.Log("이제 좌측 키를 입력해도 이동하게 됩니다.");
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(Vector3.forward.normalized * PlayerStat.instance.moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (transform.eulerAngles.y <= 185f && transform.eulerAngles.y >= 93f)
                {
                    transform.Rotate(0, -50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"좌측으로 회전합니다\nY축 Quaternion 값: {transform.rotation.y}, euler값: {transform.eulerAngles.y}");
                }
                else
                {
                    transform.Rotate(0, 50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"좌측으로 회전합니다\nY축 Quaternion 값: {transform.rotation.y}, euler값: {transform.eulerAngles.y}");
                }
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(new Vector3(1, 0, 0).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
                }
            }
        }
        #endregion
        //Debug.Log(transfomr.)
    }

    public override void Dead()
    {
        PlayerStat.instance.pState = PlayerState.dead;
        gameObject.SetActive(false);
    }

    /*public void Dash()
    {
        if (!onDash)
        {
            Debug.Log("대시 쿨타임 중입니다");
        }
        else
        {
            onInvincible = true;
            onDash = false;
            gameObject.layer = 6;
            //playerRb.AddForce(Vector3.right * PlayerStat.instance.dashForce, ForceMode.Impulse);


    //        IronDash.SetActive(true);

    //        if (Input.GetKey(KeyCode.RightArrow))
    //        {
    //            playerRb.AddForce(Vector3.right * PlayerStat.instance.dashForce, ForceMode.Impulse);
    //            Debug.Log("짧은 대쉬 입력 성공");
    //        }
    //        else if (Input.GetKey(KeyCode.LeftArrow))
    //        {
    //            playerRb.AddForce(Vector3.left * PlayerStat.instance.dashForce, ForceMode.Impulse);
    //        }
    //        else if (Input.GetKey(KeyCode.DownArrow))
    //        {
    //            playerRb.AddForce(Vector3.back * PlayerStat.instance.dashForce, ForceMode.Impulse);
    //        }
    //        else if (Input.GetKey(KeyCode.UpArrow))
    //        {
    //            playerRb.AddForce(Vector3.forward * PlayerStat.instance.dashForce, ForceMode.Impulse);
    //        }


            velocityValue = playerRb.velocity;
            Debug.Log("여기까지는 작동하냐");
            StartCoroutine(WaitCoolTime());
        }
    }*/


    public void SpecialAttack()
    {
        sAttackPrefab.SetActive(true);
    }


    /*IEnumerator WaitCoolTime()
    {
        Debug.Log("대시 충전 중입니다");


    //    yield return new WaitForSeconds(PlayerStat.instance.dashTimer);

    //    gameObject.layer = 0;
    //    playerRb.velocity = Vector3.zero;
    //    onInvincible = false;

    //    yield return new WaitForSeconds(PlayerStat.instance.dashCoolTime);


        onDash = true;
        Debug.Log("대시 쿨타임 완료");
    }*/
}
