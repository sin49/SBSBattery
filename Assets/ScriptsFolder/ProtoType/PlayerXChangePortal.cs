using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXChangePortal : InteractiveObject
{
    //float ZchangeVaule;
    public PlayerXChangePortal Destination;

    public bool HasLoadingEffect;
    public Animator _animation;

    public Transform teleportertransform;
    public Transform teleporterdestination;
    [Header("들어갈 때 Z 부터 시작")]
    public bool StartZtoX;
    [Header("나올 때 Z부터 시작")]
    public bool EndZtoX;
    [Header("에니메이션 속도(프리팹 열어서 수정)")]
    public float Shutter_animationspeed=1f;
    [Header("로딩 이펙트 들어가지 까지의 대기 시간")]
    public float WaitingLoadingTIme=1.2f;
    [Header("에니메이터 초기화까지의 대기 시간")]
    public float initializeanimatortime = 1;
    [Header("닫힌 문 보여주는 시간")]
    public float waitingopendoortime = 1f;
    [Header("도착한 플레이어 이동까지의 대기시간")]
    public float playermovewaitingtime = 0.6f;

    public Material greenlight;
    public Material redlight;
    public Renderer lightrenderer;
    public Light lightobj;
   public bool closed;
    protected override void Awake()
    {
        base.Awake();
        _animation.speed = Shutter_animationspeed;
    }
    private void Update()
    {
        _animation.SetBool("closed", closed);
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

            Debug.Log("ㅇㅇㅇㅇㅇ");
            PlayerHandler.instance.CurrentPlayer.transform.position = Destination.teleportertransform.position;
        if (PlayerHandler.instance.CurrentCamera != null)
            PlayerHandler.instance.CurrentCamera.transform.position = Destination.transform.position;
        StartCoroutine(Destination.EndMoveanimation());

    }
    public IEnumerator EndMoveanimation()
    {
        PlayerHandler.instance.CantHandle = true;
    
        yield return new WaitForSeconds(waitingopendoortime);//도착한 텔레포터 닫힌 문 보여주는 시간
        closed= false;
  
        yield return new WaitForSeconds(playermovewaitingtime);//텔레포터 열기 까지 대기시간
    

        if (EndZtoX)
            yield return StartCoroutine(PlayerHandler.instance.CurrentPlayer.moveportalanimationZX(teleporterdestination));
        else
            yield return StartCoroutine(PlayerHandler.instance.CurrentPlayer.moveportalanimationZX(teleporterdestination));
        PlayerHandler.instance.CantHandle = false;

    }
   IEnumerator MoveAnimation()
    {
        closed = false;

        PlayerHandler.instance.CantHandle = true;
        if(StartZtoX)
            yield return StartCoroutine(PlayerHandler.instance.CurrentPlayer.moveportalanimationZX(teleportertransform));
        else
        yield return StartCoroutine(PlayerHandler.instance.CurrentPlayer.moveportalanimation(teleportertransform));
        _animation.SetTrigger("Close");
        Destination. _animation.SetTrigger("Open");
        closed = true;
        Destination.closed = true;
     
        yield return new WaitForSeconds(WaitingLoadingTIme);//s어두워지기 전 딜레이
        if (!HasLoadingEffect)
            MovePosition();
        else
            GameManager.instance.LoadingEffectToAction(MovePosition);
        yield return new WaitForSeconds(initializeanimatortime);//초기화 되기까지 시간
        closed = false;
    }
    public override void Active(direction direct)
    {
        base.Active(direct);
        StartCoroutine(MoveAnimation());
       
        //PlayerCam.instance.PlayerZVaule += ZchangeVaule;
        //if(PlayerCam.instance.PlayerZVaule!=0)
        //     PlayerCam.instance.ZPin= isZpin;
    }
}
