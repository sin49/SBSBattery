using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public abstract class Crane : RemoteObject
{
    [Header("2번 크레인이 움직이는 소리")]
    public GameObject MoveObject;
    public float CraneSpeed;

    public Transform ActiveTransform;
Vector3 DeActiveTransform;
    Vector3 MoveVector;
    protected override void Awake()
    {
      base.Awake();
        DeActiveTransform = MoveObject.transform.position;
       
    }
    private void Start()
    {
        PlayerHandler.instance.registerPlayerFallEvent(Deactive);
    }
    bool CraneMove;
    public override void Active()
    {
        Debug.Log("active()");
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
    public abstract Vector3 GetMoveVector(Vector3 Target, Vector3 origin);
    public abstract bool MoveCrane(Vector3 vector, Vector3 Target, Transform origin);


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
