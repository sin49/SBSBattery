using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class fan : RemoteObject
{
    public ConvayerBelt Air;
    [Header("Ç³·Â")]
    public float AirPower;
    public Transform airDistanceTransform;
    [Header("Ç³Çâ")]
    public float airDistance;
    public Transform Particle;
    public ParticleSystem TrailParticle;
    public float TrailParticleWEight;
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
        }
        else
            Air.gameObject.SetActive(false);
    }
    private void Update()
    {
        Air.conveyorSpeed = AirPower;
        initairdistance();
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
    }

    public override void Deactive()
    {
        onActive = false;
        Air.gameObject.SetActive(false);
    }

    
}
