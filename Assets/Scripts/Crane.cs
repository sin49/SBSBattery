using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public abstract class Crane : RemoteObject
{
    [Header("2번 크레인이 움직이는 소리")]
    public GameObject MoveObject;
    public float CraneSpeed;

    public Transform ActiveTransform;
Vector3 DeActiveTransform;
    Vector3 MoveVector;
    [Header("조종 없이 자동으로 움직임")]
    public bool autoactive;
    [Header("자동으로 움직일 때 이동 후 대기")]
    public float activewait = 1;

    protected override void Awake()
    {
      base.Awake();
        DeActiveTransform = MoveObject.transform.position;
        if (autoactive)
        {
            CanControl = false;
        }
    }
    private void Start()
    {
        PlayerHandler.instance.registerPlayerFallEvent(Initalize);
        StartCoroutine(autoactivecorutine());
    }
  public  bool CraneMove;
    public void Initalize()
    {
        onActive = false;
    }
    public override void Active()
    {
  
        if (onActive)
        {
            Deactive();
            return;
        }
        else
        {
            onActive = true;
            base.Active();
        }
    }
    protected virtual bool StopMove(Transform origin,Vector3 Target)
    {
        if (soundEffectListPlayer != null)
        {
            soundEffectListPlayer.StopSound();
        }
        return false;
    }
    public override void Deactive()
    {
        if (onActive)
        {
            onActive = false;
            base.Deactive();
        }
    }
    private void OnBecameVisible()
    {
        StartCoroutine(autoactivecorutine());
    }
    public abstract Vector3 GetMoveVector(Vector3 Target, Vector3 origin);
    public abstract bool MoveCrane(Vector3 vector, Vector3 Target, Transform origin);
   
    IEnumerator autoactivecorutine()
    {
   
        while (true)
        {
            if (!CraneMove)
            {
                yield return new WaitForSeconds(activewait);
                onActive = !onActive;
            }


            yield return null;
        }
    }
        private void FixedUpdate()
    {
        if (onActive)
        {
    
            MoveVector = GetMoveVector(ActiveTransform.position, MoveObject.transform.position);
            CraneMove= MoveCrane(MoveVector, ActiveTransform.position, MoveObject.transform);
        }
        else
        {

            MoveVector = GetMoveVector(DeActiveTransform, MoveObject.transform.position);
            CraneMove= MoveCrane(MoveVector, DeActiveTransform, MoveObject.transform);
        }
        if (CraneMove && soundEffectListPlayer != null)
        {
            soundEffectListPlayer.PlayAudioNoCancel(2);
        }
        
    }

}
