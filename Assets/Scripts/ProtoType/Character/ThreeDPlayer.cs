using System.Collections;
using UnityEngine;

public class ThreeDPlayer : Character
{
    //public static Player instance;
    #region ����
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;
    public PlayerAnim animator;

    [Header("���� �� ���Ÿ� ���� ����")]
    public GameObject meleeCollider;
    public Transform firePoint;
    public GameObject rangePrefab;
    public GameObject sAttackPrefab; // Ư������ ��ƼŬ
    public GameObject IronDash; // �ٸ��� Ư������ �뽬 �׽�Ʈ�� ���� ������Ʈ (���� ���� ����)

    public float moveValue; // ������ ������ �����ϱ� ���� ����
    public float hori, vert; // �÷��̾��� ������ ����

    [Header("�ִϸ��̼� ���� ����")]
    public bool isJump, jumpAnim;
    public bool isRun;
    public bool isIdle;
    public bool isAttack;


    public bool onGround; // ���� ���� ����
    public float jumpLimit; // ���� ���� �����ϴ� ���� velocity�� y���� ����

    public Vector3 velocityValue; // ���ν�Ƽ��

    public bool onInvincible; // ���� ����
    public bool onDash; // ��� ��� ���� ����
    public bool isMove; // �̵� ���� ����


    bool currentUp; // �ڷ� ���� �����
    bool currentDown; // ������ ���� �����
    bool currentLeft; // ���� ���� �����
    bool currentRight; // ���� ���� �����

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
        //translate�켱
        //rigidbody ��
        //ZMove�� �����س���
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

        //translate�켱
        //transform.Translate(PlayerStat.instance.moveSpeed * Time.deltaTime * translateFix.normalized);
        /*if (hori < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }*/

        //rigidbody �� (�̱���)

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
            //�÷����� ����� �� ���� ����(�ٴ�,õ��, ���� ��Ƶ� ���� ������ �Ű澲������)
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
        //ColliSion Prefab Ȱ��ȭ ��Ű�� �ɷ�?
        isAttack = true;
        StartCoroutine(TestMeleeAttack());
        //animator.AttackAnimation(isAttack);
    }

    //�ִϸ��̼� ���� ���� ����
    IEnumerator TestMeleeAttack()
    {
        meleeCollider.GetComponent<SphereCollider>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        isAttack = false;

        meleeCollider.GetComponent<SphereCollider>().enabled = false;
    }

    // ���Ÿ� ���� �Լ�
    void RangeAttack()
    {
        //Collision prefab instaiate ��Ű�� �ɷ�?
        GameObject rangeObj = Instantiate(rangePrefab, firePoint.position, Quaternion.identity);
        //rangeObj.GetComponent<RangeObject>().SetDamage(PlayerStat.instance.atk);
    }

    //���� ���� �ִϸ��̼�
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
            Debug.Log("�ٴ� üũ");
            onGround = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (onInvincible)
            {
                Debug.Log("���� �����Դϴ�");
            }
            else
            {
                Damaged(collision.gameObject.GetComponent<Enemy>().eStat.atk);
                if (PlayerStat.instance.hp <= 0)
                {
                    //���ظ� ����
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
                Debug.Log("���� �� false�� ��ȯ ������ ����");
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
        //translate�켱
        //rigidbody ��
        //ZMove�� �����س���
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

        #region �̵� �Լ� �ۼ� ��
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Ű �Էµ鿡 ���� bool���� �޾� �ٶ󺸴� ������ ������Ű���� ����
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
            } // ���Ϸ� ����  180 <= Y <=359.9f�� ��
            else if (Input.GetKey(KeyCode.RightArrow)) // UpŰ + rightŰ
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
                // -20 <= y <= 5.5f �Ǵ� 355f <= y < 359.99f
                if (transform.eulerAngles.y >= -20 && transform.eulerAngles.y <= 5.5f || transform.eulerAngles.y < 359.99f && transform.eulerAngles.y >= 355f)
                {
                    // ī�޶� ���� ���� ����
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    // ������ �� ������ ȸ��
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
            //Debug.Log($"up Ű �Է� rotation.eulerAngles.y ��: {transform.eulerAngles.y}");
            // == transform.Translate(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
            // == playerRb.AddForce(transform.forward * PlayerStat.instance.moveSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        // �Ʒ�Ű
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
            //Debug.Log($"down Ű �Է� rotation.eulerAngles.y ��: {transform.eulerAngles.y}");
        }
        // ��Ű
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

                //Debug.Log("���� ���� Ű�� �Է��ص� �̵��ϰ� �˴ϴ�.");
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
                    //Debug.Log($"�������� ȸ���մϴ�\nY�� Quaternion ��: {transform.rotation.y}, euler��: {transform.eulerAngles.y}");
                }
                else
                {
                    transform.Rotate(0, -50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"�������� ȸ���մϴ�\nY�� Quaternion ��: {transform.rotation.y}, euler��: {transform.eulerAngles.y}");
                }
                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(new Vector3(-1, 0, 0).normalized * PlayerStat.instance.moveSpeed * Time.deltaTime, Space.World);
                }
            }
        }
        //transform.Rotate()
        // rotation
        //��Ű
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.eulerAngles.y >= 85 && transform.eulerAngles.y <= 95)
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
                //Debug.Log("���� ���� Ű�� �Է��ص� �̵��ϰ� �˴ϴ�.");
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
                    //Debug.Log($"�������� ȸ���մϴ�\nY�� Quaternion ��: {transform.rotation.y}, euler��: {transform.eulerAngles.y}");
                }
                else
                {
                    transform.Rotate(0, 50f * PlayerStat.instance.moveSpeed * Time.deltaTime, 0);
                    //Debug.Log($"�������� ȸ���մϴ�\nY�� Quaternion ��: {transform.rotation.y}, euler��: {transform.eulerAngles.y}");
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
            Debug.Log("��� ��Ÿ�� ���Դϴ�");
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
    //            Debug.Log("ª�� �뽬 �Է� ����");
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
            Debug.Log("��������� �۵��ϳ�");
            StartCoroutine(WaitCoolTime());
        }
    }*/


    public void SpecialAttack()
    {
        sAttackPrefab.SetActive(true);
    }


    /*IEnumerator WaitCoolTime()
    {
        Debug.Log("��� ���� ���Դϴ�");


    //    yield return new WaitForSeconds(PlayerStat.instance.dashTimer);

    //    gameObject.layer = 0;
    //    playerRb.velocity = Vector3.zero;
    //    onInvincible = false;

    //    yield return new WaitForSeconds(PlayerStat.instance.dashCoolTime);


        onDash = true;
        Debug.Log("��� ��Ÿ�� �Ϸ�");
    }*/
}
