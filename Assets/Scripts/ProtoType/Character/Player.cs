using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;

public enum direction { Left = -1, none = 0, Right = 1 }
public enum directionZ {back=-1,none=0,forward=1 }
public class Player : Character
{
    IngameUIManager gameuimanager;
    #region ����
    public Rigidbody playerRb;
    public CapsuleCollider capsuleCollider;


    Vector3 EnvironmentPower;

    public direction direction = direction.Right;

    public directionZ directionz = directionZ.none;
    [Header("���� �� ���Ÿ� ���� ����")]
    public GameObject meleeCollider; // ���� ���� �ݶ��̴�
    public GameObject flyCollider; // ���� ���� �ݶ��̴�
    public GameObject downAttackCollider; // ������� �ݶ��̴�
    public Transform firePoint; // ���Ÿ� �� Ư������ ������ġ

    public Animator ModelAnimator;
    public Animator Humonoidanimator;
    public Renderer ChrRenderer;
    public Material chrmat;
    public Color color;

    //public float moveValue; // ������ ������ �����ϱ� ���� ����
   

    [Header("#���� Ȧ�� ����")]
    public float jumpholdLevel = 0.85f;
    public float jumpBufferTimeMax;
    public float jumpBufferTimer;
    public bool canjumpInput;
    public bool jumpLimitInput;
    [Header("#Ű ���Է� ����")]
    public float attackBufferTimeMax;
    public float attackBufferTimer;
    public int attackInputValue;
    [Header("���� ����Ʈ Ȱ��ȭ ����")]
    public float flyTimer;
    public float flyTime;

    [Header("�ִϸ��̼� ���� ����")]
    public bool isJump, jumpAnim;
    public bool isRun;
    public bool isIdle;
    public bool isAttack;

    [Header("���� �ִϸ��̼� �׽�Ʈ�� ����")]
    public float animationSpeed; // �ִϸ����� ����� �ӵ� ����
    public float waitTime; // �ڷ�ƾ yield return �ð� ����
    public bool formChange; // ������Ʈ ���� ������ üũ    
    public GameObject changeEffect; // ���� �Ϸ� ����Ʈ
    public bool onTransform;// 

    [Space(15f)]
    public bool onGround; // ���� ���� ����
    public bool downAttack; // ������� ���� Ȯ��

    public bool attackSky; // ���� ����
    public bool attackGround; // ���� ����

    public Vector3 velocityValue; // ���ν�Ƽ��

    public bool onInvincible; // ���� ����
    [HideInInspector]
    public bool onDash; // ��� ��� ���� ����    

    public bool isMove; // �̵� ���� ����
    public bool canAttack; // ���� ����
    public bool wallcheck;
    #endregion

  public  float jumpkeyinputCheck = 0.15f;
    float jumpkeyinputcheckvalue;
    public bool inputCheck;

    private void OnBecameInvisible()
    {
        if (PlayerHandler.instance.CurrentPlayer == this)
        {
            PlayerHandler.instance.PlayerFallOut();
        }
    }

    [Header("�̵��� ���� �� ��ȭ �׽�Ʈ")]
    public Vector3 velocityMove; // ���ν�Ƽ �̵� �׽�Ʈ
    public Vector3 rigidbodyPos; // ������ٵ� ������ Ȯ�ο�

    public float sizeX;
    public float sizeY;
    [Header("���� ���̿��� �׽�Ʈ�� �� �߰�")]
    public float sizeFir;
    public float sizeSec;

    bool platform;
    public float raySize;
    public float jumpInitDelay;

    public int jumpInputValue;
    [Header("�ڽ� ĳ��Ʈ �׽�Ʈ")]
    public Vector3 boxRaySize; // box ����ĳ��Ʈ >> ���� �����Ǵ� �� ������ ����
    public float distanceRay; // box ĳ��Ʈ�� �Ÿ�
    RaycastHit boxHit;
    protected override void Awake()
    {
        base.Awake();
        SoundPlayer = GetComponent<PlayerSoundPlayer>();
    }
    // Start is called before the first frame update
    void Start()
    {

        if (PlayerStat.instance.formInvincible)
        {
            StartCoroutine(FormInvincible());
        }

        //chrmat = ChrRenderer.material;
        //color = Color.red;

        canAttack = true;
        onDash = true;
        PlayerHandler.instance.RegisterChange3DEvent(rotateBy3Dto2D);
    }

    void Update()
    {
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (attackBufferTimer > 0)
        {            
            attackBufferTimer -= Time.deltaTime;
        }        

        if (!onGround)
        {
            if (flyTimer > 0)
            {
                flyTimer -= Time.deltaTime;
            }
        }

    }

    public void BaseBufferTimer()
    {
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (attackBufferTimer > 0)
        {
            attackBufferTimer -= Time.deltaTime;
        }
    }

    #region ���� �� ����
    IEnumerator FormInvincible()
    {
        onInvincible = true;

        yield return new WaitForSeconds(PlayerStat.instance.invincibleCoolTime);

        PlayerStat.instance.formInvincible = false;
        onInvincible = false;
    }
    #endregion
    protected float JumprayDistance=0.28f;
    #region ���� üũ
    void jumpRaycastCheck()
    {


        Debug.DrawRay(this.transform.position,  Vector3.down * JumprayDistance, Color.blue);
        //+Vector3.down * sizeY * 0.15f
        if (!onGround&&playerRb.velocity.y<=0)
        {
            RaycastHit hit;

      
            if (Physics.Raycast(this.transform.position , Vector3.down, out hit, JumprayDistance))
            {

                if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("InteractivePlatform") || hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("GameController"))
                {
     
                    onGround = true;
                    isJump = false;
                    downAttack = false;
                    PlayerStat.instance.doubleJump = true;
                    SoundPlayer.PlayLandingSound();
                    if (LandingEffect != null && flyTimer < 0)
                        LandingEffect.SetActive(true);

                    flyTimer = flyTime;
                }


            }



        }
    }

    void wallRayCastCheck()
    {
        #region ���� ����ĳ��Ʈ
        //wallcheck = false;
        RaycastHit hit;
        Debug.DrawRay(this.transform.position + (Vector3.up * sizeFir + Vector3.right * raySize) * 0.1f * (int)direction, Vector3.right * (int)direction * 0.1f, Color.red, 0.1f);
        Debug.DrawRay(this.transform.position + (Vector3.up * sizeSec + Vector3.right * raySize) * 0.1f * (int)direction, Vector3.right * (int)direction * 0.1f, Color.magenta, 0.1f);
        bool firstCast = Physics.Raycast(this.transform.position + (Vector3.up * sizeFir + Vector3.right * raySize) * 0.1f * (int)direction, Vector3.right * (int)direction, out hit, 0.1f);
        bool secondCast = Physics.Raycast(this.transform.position + (Vector3.up * sizeSec + Vector3.right * raySize) * 0.1f * (int)direction, Vector3.right * (int)direction, out hit, 0.1f);
        //Debug.DrawRay(this.transform.position + Vector3.right * (int)direction, Vector3.right * distanceRay * (int)direction, Color.white, 0.1f);
        //bool boxCast = Physics.BoxCast(this.transform.position, boxRaySize, Vector3.right * (int)direction, out hit, transform.rotation, distanceRay);
        if (firstCast || secondCast)
        {
            Debug.Log("����ĳ��Ʈ�� �νĵ�");
            if (hit.collider == null)
            {
                Debug.Log("hit���� null�� ����");
            }
            else if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("InteractiveObject"))
            {
                wallcheck = true;
                Debug.Log("�� üũ��");
            
            }
        }
        else
        {
            Debug.Log("�� üũ �ȵ�");
            wallcheck = false;
        }
        #endregion
    }

    public void SetWallcheck(bool checking)
    {
        wallcheck = checking;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + Vector3.right * 0.05f * (int)direction, Vector3.right * (int)direction * distanceRay, Color.white, 0.1f);
        if (Physics.BoxCast(this.transform.position, boxRaySize, Vector3.right * (int)direction, out boxHit, transform.rotation, distanceRay))
        {
            //Debug.Log($"�ڽ�ĳ��Ʈ �ν��� {boxHit.collider}");
            /*if (boxHit.collider.CompareTag("InteractivePlatform"))
            {
                wallcheck = true;
                Debug.Log($"�ڽ�ĳ��Ʈ�� �÷����� �ν��Ͽ� �� ������ ������{boxHit.collider}");
            }
            else
            {
                Debug.Log($"�ڽ�ĳ��Ʈ�� �÷����� ã�� ���Ͽ� ������{boxHit.collider}");
                wallcheck = false;
            }
            Debug.DrawRay(transform.position + Vector3.right * 0.1f * (int)direction, Vector3.right * (int)direction * distanceRay, Color.black, 0.1f);*/
            Gizmos.DrawWireCube(boxHit.point, boxRaySize);
            //Debug.Log(boxHit.point);
        }
        Gizmos.DrawWireCube(transform.position, boxRaySize);
    }

    #endregion

    public void HittedTest()
    {

        if (Humonoidanimator != null)
        {
            Humonoidanimator.SetTrigger("Damaged");
        }

        if (HittedEffect != null)
            HittedEffect.gameObject.SetActive(true);

    }

    private void FixedUpdate()
    {

        InteractivePlatformrayCheck();
        InteractivePlatformrayCheck2();

        if (jumpkeyinputcheckvalue > 0)
            jumpkeyinputcheckvalue -= Time.fixedDeltaTime;

        JumpKeyInput();
        if(!downAttack)
        Attack();

        if (onGround == true&& isJump == true)
            isJump = false;

        /* chrmat.SetColor("_Emissive_Color", color);*///emission �ǵ��
        if (Input.GetKeyDown(KeyCode.Tab)) { HittedTest(); }

        //if (onGround && isJump && playerRb.velocity.y <= 0)
        //    jumpRaycastCheck();
        if (Humonoidanimator != null)
        {
            Humonoidanimator.SetBool("run", isRun);
            Humonoidanimator.SetBool("Onground", onGround);
            ModelAnimator.SetBool("Rolling", downAttack);
            Humonoidanimator.SetBool("DownAttack", downAttack);
        }

        /*if (RunEffect != null)
        {            

            if (isRun && onGround)
            {
                a.maxParticles = 100;
                if (!RunEffect.isPlaying)
                    RunEffect.Play();
            }
            else
            {
                a.maxParticles = 0;
                if ((RunEffect.isPlaying && RunEffect.particleCount == 0))
                    RunEffect.Stop();
            }
        }
        else
        {
            a.maxParticles = 0;
            if ((RunEffect.isPlaying && RunEffect.particleCount == 0))
                RunEffect.Stop();
        }*/

        if (RunEffect!=null)
        {
            var a = RunEffect.main;

            /*platformDisableTimer += Time.deltaTime;
            if (PlatformDisableTime <= platformDisableTimer)*/


                if (isRun && onGround)
                {
                    a.maxParticles = 100;
                    if (!RunEffect.isPlaying)
                        RunEffect.Play();


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
    }
    public ParticleSystem RunEffect;
    public GameObject HittedEffect;
    public GameObject AttackEffect;
    public GameObject LandingEffect;
    public GameObject JumpEffect;

    Vector3 translateFix;

    [Header("PlayerSoundPlayer")]
    public PlayerSoundPlayer SoundPlayer;


    #region �߻�ȭ �������̵� �Լ�

    #region �̵�
    public void rotate(float hori,float vert)
    {
        Vector3 rotateVector = Vector3.zero;

        // Check horizontal and vertical inputs and determine the direction
        if (hori == 1)
            direction = direction.Right;
        else if (hori == -1)
            direction = direction.Left;
        else
            direction = direction.none;
        if (vert == 1)
            directionz = directionZ.back;
        else if (vert == -1)
            directionz = directionZ.forward;
        else
            directionz = directionZ.none;

        //PlayerStat.instance.Trans3D

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

        transform.GetChild(0).rotation = Quaternion.Euler(rotateVector);
    }
    public void rotateBy3Dto2D()
    {
        Vector3 rotateVector = Vector3.zero;
        if (PlayerStat.instance.MoveState==PlayerMoveState.SideX)
        {
           
            if (direction == direction.Right||direction==direction.none)
            {
                rotateVector = new Vector3(0, 90, 0);
            }
            else if (direction == direction.Left)
            {
                rotateVector = new Vector3(0, -90, 0);
            }
            transform.rotation = Quaternion.Euler(rotateVector);
        }
       
    }

    public float Decelatate = 2;

    public float hori;
    public float Vert;

    public override void Move()
    {
        hori = 0;
        Vert=0;
        switch (PlayerStat.instance.MoveState)
        {
            case PlayerMoveState.SideX:
                hori = Input.GetAxisRaw("Horizontal");
                break;
            case PlayerMoveState.SideZ:
       
                Vert = -1 * Input.GetAxisRaw("Horizontal");
                break;
            case PlayerMoveState.Trans3D:
                hori = Input.GetAxisRaw("Vertical");
                Vert = -1 * Input.GetAxisRaw("Horizontal");
                break;
        }
       
   
        if (!canAttack && onGround)
        {
            hori = 0;
            Vert = 0;
        }

        Vector3 moveInput = new Vector3(hori, 0, Vert);
        if (hori != 0 || Vert != 0)
        {
            if(canAttack)
            rotate(moveInput.x, moveInput.z);
            SoundPlayer.PlayMoveSound();
            
        }
        //Vert ȸ�� �߰�
        //translateFix = new(hori, 0, 0);

        #region ������



        Vector3 Movevelocity = Vector3.zero;
        Vector3 desiredVector = moveInput.normalized * PlayerStat.instance.moveSpeed + EnvironmentPower;
        Movevelocity = desiredVector - playerRb.velocity.x * Vector3.right - playerRb.velocity.z * Vector3.forward;

      
        if (!wallcheck) 
            playerRb.AddForce(Movevelocity, ForceMode.VelocityChange);
        else
            playerRb.AddForce(EnvironmentPower, ForceMode.VelocityChange);


        if (Movevelocity == Vector3.zero)
        {
            Vector3 CurrentVelocity = playerRb.velocity;

            var newDecelateVector = Vector3.Lerp(CurrentVelocity, Vector3.zero, Decelatate * Time.fixedDeltaTime);


            playerRb.velocity = new Vector3(newDecelateVector.x, CurrentVelocity.y, newDecelateVector.z);
            //else
            //           playerRb.velocity = new Vector3(0,playerRb.velocity.y, playerRb.velocity.z);

        }


        EnvironmentPower = Vector3.zero;


        #endregion

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

        velocityMove = playerRb.velocity;
        rigidbodyPos = playerRb.position;



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
    protected void AttackEvents()
    {
        canAttack = false;
        if (Humonoidanimator != null)
            Humonoidanimator.Play("Attack");
        if (SoundPlayer != null)
            SoundPlayer.PlayAttackAudio();
    }
    public override void Attack()
    {
        if (PlayerHandler.instance.onAttack)
        {
            if (attackBufferTimer > 0 && canAttack)
            {
             
                if (PlayerStat.instance.attackType == AttackType.melee && canAttack && !downAttack)
                {
                    attackBufferTimer = 0;
                
                    if (!onGround)
                    {
                        attackSky = true;
                    }
                    else
                    {
                        attackGround = true;
                    }

                    AttackEvents();
                    StartCoroutine(TestMeleeAttack());
                }
            }
        }
    }

    void AttackMove()
    {
        if (!wallcheck)
        {
            if (PlayerStat.instance.MoveState!=PlayerMoveState.Trans3D && directionz != directionZ.none && hori == 0)
            {
                playerRb.AddForce(transform.forward * 7, ForceMode.Impulse);
            }
            else if (PlayerStat.instance.MoveState == PlayerMoveState.Trans3D)
            {
                if (direction != direction.none && Vert != 0 || directionz != directionZ.none && hori != 0)
                {
                    playerRb.AddForce(transform.forward * 7, ForceMode.Impulse);
                }                
            }
        }
    }




    #region �������

    //public float DownAttackForce;

    public void DownAttack()
    {
        if (!downAttack)
        {
            Debug.Log("�������");
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


        yield return new WaitForSeconds(PlayerStat.instance.downAttackFlyTime);

        playerRb.AddForce(Vector3.down * PlayerStat.instance.downForce);
        downAttackCollider.SetActive(true);
        playerRb.useGravity = true;
    }
    #endregion

    #region Ư������
    public virtual void Skill1()
    {
        attackBufferTimer = attackBufferTimeMax;
    }
    public virtual void Skill2()
    {

    }
    #endregion

    #endregion

    #region �ǰ�
    public void DamagedIgnoreInvincible(float damage)
    {
        onInvincible = true;

        PlayerStat.instance.pState = PlayerState.hitted;
        HittedEffect.gameObject.SetActive(true);
        PlayerStat.instance.hp -= damage;


        if (PlayerStat.instance.hp <= 0)
        {
            //Dead()
            PlayerStat.instance.hp = 0;
            Dead();
        }
        else
        {
            StartCoroutine(ActiveInvincible());
            StartCoroutine(WaitEndDamaged());
        }
    }
    public override void Damaged(float damage)
    {
        if (onInvincible)
            return;
        onInvincible = true;

        PlayerStat.instance.pState = PlayerState.hitted;
        HittedEffect.gameObject.SetActive(true);
        PlayerStat.instance.hp -= damage;


        if (PlayerStat.instance.hp <= 0)
        {
            //Dead()
            PlayerStat.instance.hp = 0;
            Dead();
        }
        else
        {
            StartCoroutine(ActiveInvincible());
            StartCoroutine(WaitEndDamaged());
        }
    }

    IEnumerator ActiveInvincible()
    {
        yield return new WaitForSeconds(PlayerStat.instance.invincibleCoolTime);

        onInvincible = false;
    }

    IEnumerator WaitEndDamaged()
    {
        if (Humonoidanimator != null)
        {
            Humonoidanimator.SetTrigger("Damaged");
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
        SoundPlayer.PlayCharacterDieClip();
        if(!PlayerSpawnManager.Instance.DontSave)
        GameManager.instance.LoadingSceneWithKariEffect(GameManager.instance.LoadLastestStage());
        else
            GameManager.instance.LoadingSceneWithKariEffect(SceneManager.GetActiveScene().name);
    }
    #endregion

    #endregion

    #region ��������
    public void Jump()
    {
        isJump = true;
        jumpBufferTimer = 0;
        canjumpInput = false;
        jumpLimitInput = true;
       
        if (Humonoidanimator != null)
        {
            Humonoidanimator.SetTrigger("jump");
        }

        if (JumpEffect != null)
            JumpEffect.SetActive(true);

        isRun = false;
        if (SoundPlayer != null)
            SoundPlayer.PlayJumpAudio();
        playerRb.velocity = Vector3.zero;
        playerRb.AddForce(Vector3.up * PlayerStat.instance.jumpForce, ForceMode.Impulse);
        onGround = false;
      

    }


    public void JumpKeyInput()
    {
        if (jumpBufferTimer > 0)
        {
            if (!downAttack)
            {
                if (!isJump)
                {
                    Jump();
                }
                else if (jumpInputValue > 0 && canjumpInput && PlayerStat.instance.doubleJump)
                {
                    PlayerStat.instance.doubleJump = false;
                    Jump();
                    /*if (
                PlayerInventory.instance.checkessesntialitem("item02"))
                    {
                        PlayerStat.instance.doubleJump = false;
                        Jump();
                    }*/
                }
   
            }
        }
    }
    public void GetJumpBuffer()
    {
        if(jumpkeyinputcheckvalue <= 0&&onGround)
            jumpkeyinputcheckvalue = jumpkeyinputCheck;
        jumpInputValue = 1;
        if (!jumpLimitInput)
            jumpBufferTimer = jumpBufferTimeMax;
    }
    public void jumphold()
    {
        //jumpLimitInput = false;
        jumpInputValue = 0;
        canjumpInput = true;
        if (playerRb.velocity.y > 0)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y * jumpholdLevel, playerRb.velocity.z);
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

    bool onattack;
    //�ִϸ��̼� ���� ���� ����
    IEnumerator TestMeleeAttack()
    {
        AttackMove();

        if (attackSky)
        {
            meleeCollider.SetActive(true);
            meleeCollider.GetComponent<SphereCollider>().enabled = true;
        }
        else if (attackGround)
        {

            meleeCollider.SetActive(true);
            meleeCollider.GetComponent<SphereCollider>().enabled = true;
            
        }
        if (AttackEffect != null)
            AttackEffect.SetActive(true);
        yield return new WaitForSeconds(PlayerStat.instance.attackDelay);


        if (attackSky)
        {
            meleeCollider.SetActive(false);
            meleeCollider.GetComponent<SphereCollider>().enabled = false;
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
        if (Humonoidanimator != null)
            Humonoidanimator.SetTrigger("Attack");
        meleeCollider.GetComponent<BoxCollider>().enabled = false;
    }
    #endregion

    #region ����
    public void FormChange(TransformType type, Action event_ = null)
    {

        StartCoroutine(EndFormChange(type, event_));
    }

    IEnumerator EndFormChange(TransformType type, Action event_)
    {

        PlayerStat.instance.formInvincible = true;
        PlayerHandler.instance.lastDirection = direction;
        PlayerHandler.instance.formChange = true;
        onInvincible = true;
        Time.timeScale = 0.2f;
        ModelAnimator.SetTrigger("FormChange");
        ModelAnimator.SetFloat("Speed", animationSpeed);

        yield return new WaitForSeconds(waitTime);

        PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;
        Instantiate(changeEffect, transform.position, Quaternion.identity);
        PlayerHandler.instance.transformed(type, event_);        
        if (PlayerHandler.instance.CurrentPlayer != null)
            PlayerHandler.instance.CurrentPlayer.direction = direction;
    }
    #endregion

    #region �ݶ��̴� Ʈ����
    private void OnCollisionExit(Collision collision)
    {
        #region �ٴ� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("InteractivePlatform") ||
            collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("GameController"))
        {
            onGround = false;
     
        }
        #endregion
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground")&& jumpkeyinputcheckvalue <= 0 )
        {
            jumpRaycastCheck();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //#region �ٴ� ��ȣ�ۿ�
        if (collision.gameObject.CompareTag("Ground") && jumpkeyinputcheckvalue <= 0)
        {
            jumpRaycastCheck();
        }
        //#endregion

        if (collision.gameObject.CompareTag("InteractivePlatform") && jumpkeyinputcheckvalue <= 0)
        {
            jumpRaycastCheck();
           

            if (PlayerHandler.instance.doubleDownInput && !CullingPlatform)
            {
                PlayerHandler.instance.doubleDownInput = false;
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

            jumpRaycastCheck();
        }

        if (collision.gameObject.CompareTag("GameController"))
        {
            jumpRaycastCheck();
        }
        #endregion
    }









    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack") && !onInvincible)
        {
            Debug.Log("���ظ� ����");
            Damaged(other.GetComponent<EnemyMeleeAttack>().GetDamage());
        }
    }*/

    public float DownAttackForce;





    #endregion

    #region ����� �÷���
    public bool CullingPlatform;
    float PlatformDisableTime = 0.3f;
    float platformDisableTimer;
    public void InteractivePlatformrayCheck2()
    {

        RaycastHit hit;
        //if (playerRb.velocity.y <=0)
        //{

        //Debug.DrawRay(transform.position, Vector3.up * 0.2f*sizeY, Color.green);
        if (!CullingPlatform && playerRb.velocity.y > 0)
        {
         

            if (Physics.Raycast(this.transform.position , Vector3.up, out hit, InteractiveUprayDistance))
            {

                if (hit.collider.CompareTag("InteractivePlatform"))
                {

                    CullingPlatform = true;
                    Physics.IgnoreLayerCollision(6, 11, true);
                

                }

            }

        }


    }
    public void AddEnviromentPower(Vector3 power)
    {
        EnvironmentPower += power;
    }
    //public  void getEnviromentPower()
    //  {
    //      playerRb.AddForce(EnvironmentPower, ForceMode.Acceleration);

    //      Debug.Log("Velocity"+playerRb.velocity);
    //  }
    protected float InteractiveUprayDistance=0.4f;
    public void InteractivePlatformrayCheck()
    {

        Debug.DrawRay(transform.position, Vector3.up * InteractiveUprayDistance, Color.green);
        RaycastHit hit;
        //if ()
        //{

        if (CullingPlatform && playerRb.velocity.y <= 0)
        {

            if (Physics.Raycast(this.transform.position, Vector3.up, out hit, InteractiveUprayDistance))
            {

                if (hit.collider.CompareTag("InteractivePlatform"))
                {

                    CullingPlatform = false;
                    Physics.IgnoreLayerCollision(6, 11, false);
                    platformDisableTimer = 0;
               
                }

            }

        }
    }
    #endregion


}