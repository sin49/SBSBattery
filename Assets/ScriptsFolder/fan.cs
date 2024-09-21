using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class fan : RemoteObject
{
    [Header("2번 활성화 중 돌아가는 소리")]
    public ConvayerBelt Air;
    [Header("풍력")]
    public float AirPower;
    public Transform airDistanceTransform;
    [Header("풍향")]
    public float airDistance;
    public Transform Particle;
    public ParticleSystem TrailParticle;
    public float TrailParticleWEight;

    public Animator animator;

    void initairdistance()
    {
        airDistanceTransform.localScale =
            new Vector3(airDistanceTransform.localScale.x, airDistanceTransform.localScale.y, airDistance / 3);
        var forward = Quaternion.LookRotation(this.transform.forward);
        forward = Quaternion.Euler(forward.eulerAngles.x + 90, forward.eulerAngles.y, forward.eulerAngles.z);
        Particle.transform.rotation = forward;
        var velocityOverLifetime =  TrailParticle.velocityOverLifetime;
        Vector3 TrailDir = new Vector3(forward.eulerAngles.x, forward.eulerAngles.y, forward.eulerAngles.z).normalized;
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(transform.forward.x * airDistance * TrailParticleWEight);
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(transform.forward.y * airDistance * TrailParticleWEight);
        velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(transform.forward.z * airDistance * TrailParticleWEight);
    }

    private void Start()
    {
        Air.conveyorDirection = this.transform.forward;
        Air.conveyorSpeed = AirPower;

        if (onActive)
        {
            Air.gameObject.SetActive(true);
            Particle.gameObject.SetActive(true);
           
        }
        else
        {
            Air.gameObject.SetActive(false);
            Particle.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        Air.conveyorSpeed = AirPower;
        animator.SetBool("Active", onActive);
        initairdistance();
        if(onActive&& soundEffectListPlayer!=null)
            soundEffectListPlayer.PlayAudioNoCancel(2);
    }
    public override void Active()
    {
        if (onActive)
        {
            Deactive();
            return;
        }
        onActive = true;
        Air.gameObject.SetActive(true);
        Particle.gameObject.SetActive(true);
        base.Active();
    }

    public override void Deactive()
    {
        onActive = false;
        Air.gameObject.SetActive(false);
        Particle.gameObject.SetActive(false);
        base.Deactive();
    }

    
}
