using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKjs : Character
{
    #region ����
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;

    public direction direction = direction.Right;

    [Header("���� �� ���Ÿ� ���� ����")]
    public GameObject meleeCollider; // ���� ���� �ݶ��̴�
    public GameObject flyCollider; // ���� ���� �ݶ��̴�
    public GameObject downAttackCollider; // ������� �ݶ��̴�
    public Transform firePoint; // ���Ÿ� �� Ư������ ������ġ

    public Animator animator;

    public float moveValue; // ������ ������ �����ϱ� ���� ����
    public float hori, vert; // �÷��̾��� ������ ����

    [Header("�ִϸ��̼� ���� ����")]
    public bool isJump, jumpAnim;
    public bool isRun;
    public bool isIdle;
    public bool isAttack;


    public bool onGround; // ���� ���� ����
    public bool downAttack; // ������� ���� Ȯ��
    public float jumpLimit; // ���� ���� �����ϴ� ���� velocity�� y���� ����

    public bool attackSky; // ���� ����
    public bool attackGround; // ���� ����

    public Vector3 velocityValue; // ���ν�Ƽ��

    public bool onInvincible; // ���� ����
    public bool onDash; // ��� ��� ���� ����
    public bool isMove; // �̵� ���� ����

    public bool canAttack; // ���� ����

    /*bool currentUp; // �ڷ� ���� �����
    bool currentDown; // ������ ���� �����
    bool currentLeft; // ���� ���� �����
    bool currentRight; // ���� ���� �����*/

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

    #region ���� üũ
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
            Debug.Log("�޸��� �ִϸ��̼� ������");
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

            Debug.Log("Ȱ��");
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

    #region �߻�ȭ �������̵� �Լ�
    #region �̵�
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

    #region ����
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
            Debug.Log("����Ű");
            StartCoroutine(TestMeleeAttack());
        }
    }






    #region �������

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

    #region Ư������
    public virtual void Skill1()
    {
        Debug.Log("sŰ�� �̿��� ��ų");
    }
    public virtual void Skill2()
    {

    }
    #endregion

    #endregion

    #region �ǰ�
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

    #region ���
    public override void Dead()
    {
        PlayerStat.instance.pState = PlayerState.dead;
        gameObject.SetActive(false);
    }
    #endregion
    #endregion

    #region ��������
    public void Jump()
    {
        if (!isJump)
        {
            //�÷����� ����� �� ���� ����(�ٴ�,õ��, ���� ��Ƶ� ���� ������ �Ű澲������)
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


    //�ִϸ��̼� ���� ���� ����
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

    // ���Ÿ� ���� �Լ�

    //���� ���� �ִϸ��̼�
    public IEnumerator ActiveMeleeAttack()
    {
        meleeCollider.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(0.5f);

        isAttack = false;
        animator.SetTrigger("Attack");
        meleeCollider.GetComponent<BoxCollider>().enabled = false;
    }
    #endregion

    #region �ݶ��̴� Ʈ����
    private void OnCollisionExit(Collision collision)
    {
        #region �ٴ� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("InteractivePlatform"))

        {

            onGround = false;

        }
        #endregion
    }
    private void OnCollisionStay(Collision collision)
    {
        //#region �ٴ� ��ȣ�ۿ�
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


        #region �� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (onInvincible)
            {
                Debug.Log("���� �����Դϴ�");
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

    #region ����� �÷���
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
