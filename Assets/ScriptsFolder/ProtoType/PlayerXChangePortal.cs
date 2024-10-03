using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXChangePortal : InteractiveObject
{
    [Header("���� 1�� ��� �� �� ����")]
    [Header("���� 2�� ���� �� �� ����")]
    [Header("���� 0���� Ȱ��ȭŰ ���� ���� ���� �Ҹ��� ������� ��")]
    //float ZchangeVaule;
    public PlayerXChangePortal Destination;

    public bool HasLoadingEffect;
    public Animator _animation;



    public Transform teleportertransform;
    public Transform teleporterdestination;

    public Collider portalcollider;

    [Header("�� �� Z ���� ����")]
    public bool StartZtoX;
    [Header("���� �� X���� ����")]
    public bool EndZtoX;
    [Header("���ϸ��̼� �ӵ�(������ ��� ����)")]
    public float Shutter_animationspeed=1f;
    [Header("�ε� ����Ʈ ���� ������ ��� �ð�")]
    public float WaitingLoadingTIme=1.2f;
    [Header("���ϸ����� �ʱ�ȭ������ ��� �ð�")]
    public float initializeanimatortime = 1;
    [Header("���� �� �����ִ� �ð�")]
    public float waitingopendoortime = 1f;
    [Header("������ �÷��̾� �̵������� ���ð�")]
    public float playermovewaitingtime = 0.6f;

    public Material greenlight;
    public Material redlight;
    public Renderer lightrenderer;
    public Light lightobj;
   public bool closed;
    protected override void Awake()
    {
        base.Awake();
        if(_animation !=null)
        _animation.speed = Shutter_animationspeed;
    }
    private void Update()
    {
        if(_animation != null)
        _animation.SetBool("closed", closed);
        if(lightrenderer !=null && lightobj !=null)
        if (closed)
        {
            lightrenderer.material = greenlight;
            lightobj.color = Color.green;
        }
        else
        {
            lightrenderer.material = redlight;
            lightobj.color = Color.red;
        }
    }
    public void MovePosition(string s=null)
    {
        
        portalcollider.enabled = true;
        Destination.portalcollider.enabled = false;
            Debug.Log("����������");
            PlayerHandler.instance.CurrentPlayer.transform.position = Destination.teleportertransform.position;
        if (PlayerHandler.instance.CurrentCamera != null)
            PlayerHandler.instance.CurrentCamera.transform.position = Destination.transform.position;
        StartCoroutine(Destination.EndMoveanimation());

    }
    public IEnumerator EndMoveanimation()
    {
        PlayerHandler.instance.CantHandle = true;
    
        yield return new WaitForSeconds(waitingopendoortime);//������ �ڷ����� ���� �� �����ִ� �ð�
     
        closed= false;
        soundEffectListPlayer.PlayAudio(2);
        yield return new WaitForSeconds(playermovewaitingtime);//�ڷ����� ���� ���� ���ð�


        if (EndZtoX)
            yield return StartCoroutine(PlayerHandler.instance.CurrentPlayer.moveportalanimationZX(teleporterdestination));
        else
            yield return StartCoroutine(PlayerHandler.instance.CurrentPlayer.moveportalanimation(teleporterdestination));
        PlayerHandler.instance.CantHandle = false;
        PlayerHandler.instance.CurrentPlayer.cantmove = false;
       
        portalcollider.enabled = true;
    }
    
   IEnumerator MoveAnimation()
    {
        PlayerHandler.instance.CurrentPlayer.cantmove = true;

        closed = false;

        PlayerHandler.instance.CantHandle = true;
        if(StartZtoX)
            yield return StartCoroutine(PlayerHandler.instance.CurrentPlayer.moveportalanimationZX(teleportertransform));
        else
        yield return StartCoroutine(PlayerHandler.instance.CurrentPlayer.moveportalanimation(teleportertransform));
        _animation.SetTrigger("Close");
        Destination. _animation.SetTrigger("Open");
      
        closed = true;
        soundEffectListPlayer.PlayAudio(1);
        Destination.closed = true;
     
        yield return new WaitForSeconds(WaitingLoadingTIme);//s��ο����� �� ������
        if (!HasLoadingEffect)
            MovePosition();
        else
            GameManager.instance.LoadingEffectToAction(MovePosition);
        yield return new WaitForSeconds(initializeanimatortime);//�ʱ�ȭ �Ǳ���� �ð�
        closed = false;

    }
    public override void Active(direction direct)
    {
        base.Active(direct);
        portalcollider.enabled = false;
        StartCoroutine(MoveAnimation());
       
        //PlayerCam.instance.PlayerZVaule += ZchangeVaule;
        //if(PlayerCam.instance.PlayerZVaule!=0)
        //     PlayerCam.instance.ZPin= isZpin;
    }
}
