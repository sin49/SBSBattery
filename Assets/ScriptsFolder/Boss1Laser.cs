using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Boss1Laser : EnemyAction
{
    Transform Target;


    Boss1SOundManager boss1SOundManager;
    [Header("레이저 속도")]
    public float LaserSpeed;
    [Header("레이저 Y축 위치")]
    public float LaserYpos=-6.4f;

    public float handreturntime = 2;
    float lasertime;
    public Transform warning;
    public ParticleSystem particle;
    float laserlifetime;
    [Header("레이저 활성화 까지의 시간")]
    public float laserActiveTimer = 1.5f;

    public Transform ColliderSpawnPoint;

    public Transform Laser;

    public Transform laserBeam;
    [Header("장판 범위")]
    public float TrailColScale = 1;
    [Header("장판 지속 시간")]
    public float TrailDuration;
    [Header("장판 공격 판정 생성 간격")]
    public float ColliderSpawnTime;

    //public TrailRenderer TrailRenderer;

    public Transform pullingtransform;

    public Boss1LaserCollider LaserCollider;

    Queue<Boss1LaserCollider> LaserPulling=new Queue<Boss1LaserCollider>();

    IEnumerator activecoroutine;

    Animator ani;
    public override void StopAction()
    {
        base.StopAction();
        particle.Stop();
        laserlifetime = 0;
    }
    public override void Invoke(Action ActionENd, Transform target = null)
    {
        lasertime = ActionLifeTIme;
        ActionLifeTIme += handreturntime + 0.5f;
        LhandOriginPosition = LhandTransform.position;
        RhandOriginPosition = RhandTransform.position;
        this.Target = target;
        Laser.gameObject.SetActive(true);
        //Laser.transform.position = new Vector3(target.position.x, LaserYpos,
        //   target.position.z);
        ani.enabled = true;
        ani.Play("LaserAttack");
       
        laserBeam.gameObject.SetActive(false);

        laserlifetime = lasertime;
        handreturncorutine = false;
        var main = particle.main;
        main.startSize = TrailColScale * 3;
        main.duration = laserlifetime;
        main.startLifetime = TrailDuration;
        registerActionHandler(() => { /*StopAllCoroutines();*/ StopCoroutine(activecoroutine); });
        registerActionHandler(ActionENd);

    }

    public void LaserStart()
    {
        ani.enabled = false;
        Vector3 v = (LhandTransform.position + RhandTransform.position) / 2;
        v.y = LaserYpos;
        Laser.transform.position = v;
        
        warning.position = Laser.transform.position+Vector3.up ;
        warning.gameObject.SetActive(true);
        activecoroutine = laserPattern();
        StartCoroutine(activecoroutine);
   
    }

    protected override void CancelActionEvent()
    {
        laserlifetime = 0;
        laserBeam.gameObject.SetActive(false);
        ColliderSpawnPoint.gameObject.SetActive(false);
        warning.gameObject.SetActive(false);
        var a = pullingtransform.GetComponentsInChildren<Boss1LaserCollider>();
        foreach(var t in a)
        {
            if (t.gameObject.activeSelf)
            {

                laserpullingevent(t);
                t.gameObject.SetActive(false);
            }
        }
        base.CancelActionEvent();
    }

    public GameObject bosslasereffect;
    public ParticleSystem Bosslasergaze;

    public void lasergazeparticleon()
    {
        Bosslasergaze.gameObject.SetActive(true);
    }
    public void lasergazeparticleoff()
    {
        Bosslasergaze.Stop();
        Bosslasergaze.gameObject.SetActive(false);
    }
    public Transform LhandTransform;
    public Transform RhandTransform;
    Vector3 LhandOriginPosition;
    Vector3 RhandOriginPosition;
    private void Awake()
    {
        boss1SOundManager=this.GetComponent<Boss1SOundManager>();
        laserBeam.gameObject.SetActive(false);
        ColliderSpawnPoint.gameObject.SetActive(false);
        ani = GetComponent<Animator>();
    }
   
    IEnumerator laserPattern()
    {
        if(boss1SOundManager!=null)
        boss1SOundManager.LazerinitClipPlay();
        yield return new WaitForSeconds(laserActiveTimer);
        warning.gameObject.SetActive(false);
        laserBeam.gameObject.SetActive(true);
        ColliderSpawnPoint.gameObject.SetActive(true);
        particle.Play();
        if (boss1SOundManager != null)
            boss1SOundManager.LazerStartClipPlay();

        while (true)
        {
            if (!laserBeam.gameObject.activeSelf)
                break;
            CreateLaser();
            yield return new WaitForSeconds(ColliderSpawnTime);
        }
      
        
    }
    void laserMove()
    {
        if (Target != null)
        {
            Vector3 LaserVector = (Target.transform.transform.position - Laser.transform.position).normalized;
          
            LaserVector.y = 0;
            Laser.Translate(LaserVector * LaserSpeed * Time.fixedDeltaTime);
            LhandTransform.position = Laser.transform.position + new Vector3(-1.5f, 5, 0);
            RhandTransform.position = Laser.transform.position + new Vector3(1.5f
                , 5, 0);
        }
    }
    void laserpullingevent(Boss1LaserCollider collider)
    {
        LaserPulling.Enqueue(collider);

        collider.gameObject.SetActive(false);
    }
    void CreateLaser()
    {
        Boss1LaserCollider col;
        if (LaserPulling.Count == 0)
        {
            col = Instantiate(LaserCollider.gameObject, ColliderSpawnPoint.transform.position,
                Quaternion.identity).GetComponent<Boss1LaserCollider>();
            col.transform.SetParent(pullingtransform);
        }
        else
        {
            col = LaserPulling.Dequeue();
            col.transform.position = ColliderSpawnPoint.transform.position;
            col.gameObject.SetActive(true);
        }


        col.initLaserCollider(TrailDuration-0.1f, Vector3.one* TrailColScale,
              laserpullingevent);
    }
    bool handreturncorutine;
    IEnumerator handreturn(){
        handreturncorutine = true;
        float timer = 0;
        float rotationspeed = 60 / handreturntime;
        Vector3 Lhandvector = (-LhandTransform.position + LhandOriginPosition);
        float LSpeed = Lhandvector.magnitude / handreturntime;
        Vector3 Rhandvector = (-RhandTransform.position + RhandOriginPosition);
        float RSpeed = Rhandvector.magnitude / handreturntime;
        while (timer < handreturntime)
        {
            LhandTransform.Rotate(Vector3.forward * -1 * rotationspeed * Time.fixedDeltaTime);
            RhandTransform.Rotate(Vector3.forward  * rotationspeed * Time.fixedDeltaTime);
            LhandTransform.Translate(LSpeed * Lhandvector.normalized * Time.fixedDeltaTime,Space.World);
            RhandTransform.Translate(RSpeed * Rhandvector.normalized * Time.fixedDeltaTime, Space.World);
            timer += Time.fixedDeltaTime;
            yield return null;
        }
        LhandTransform.position = LhandOriginPosition;
        RhandTransform.position = RhandOriginPosition;
        LhandTransform.rotation = Quaternion.Euler(0, 0, 30);
       RhandTransform.rotation = Quaternion.Euler(0, 0, -30);

        yield return new WaitForSeconds(0.5f);
        DisableActionMethod();


    }
    private void FixedUpdate()
    {
        if (!laserBeam.gameObject.activeSelf)
            return;
        laserMove();
        if (Bosslasergaze.isPlaying)
        {
            bosslasereffect.transform.position = (LhandTransform.position + RhandTransform.position) / 2;
        }
       
        if(laserlifetime < 0)
        {
            laserBeam.gameObject.SetActive(false);
            particle.Stop();
            if (boss1SOundManager != null)
                boss1SOundManager.LazerStartClipEnd();
            if (!handreturncorutine)
                StartCoroutine(handreturn());
        }
        else
            laserlifetime -= Time.fixedDeltaTime;
     
       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerHandler.instance.CurrentPlayer.onInvincible)
        {

            other.GetComponent<Player>().Damaged(1);
        }
    }
}
