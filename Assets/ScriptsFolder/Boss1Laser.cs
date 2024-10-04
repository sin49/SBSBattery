using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Boss1Laser : EnemyAction
{
    Transform Target;


    Boss1SOundManager boss1SOundManager;
    [Header("������ �ӵ�")]
    public float LaserSpeed;
    public float laserYpos;
    public Transform warning;
    public ParticleSystem particle;
    float laserlifetime;
    [Header("������ Ȱ��ȭ ������ �ð�")]
    public float laserActiveTimer = 1.5f;

    public Transform ColliderSpawnPoint;

    public Transform Laser;

    public Transform laserBeam;
    [Header("���� ����")]
    public float TrailColScale = 1;
    [Header("���� ���� �ð�")]
    public float TrailDuration;
    [Header("���� ���� ���� ���� ����")]
    public float ColliderSpawnTime;

    //public TrailRenderer TrailRenderer;

    public Transform pullingtransform;

    public Boss1LaserCollider LaserCollider;

    Queue<Boss1LaserCollider> LaserPulling=new Queue<Boss1LaserCollider>();

    IEnumerator activecoroutine;
  

    public override void Invoke(Action ActionENd, Transform target = null)
    {
        this.Target = target;
        Laser.transform.position = new Vector3(target.position.x, -6.4f,
           target.position.z);
        laserBeam.gameObject.SetActive(false);
        warning.position = Laser.transform.position-Vector3.up*2;
        laserlifetime = ActionLifeTIme;
        warning.gameObject.SetActive(true);
        activecoroutine = laserPattern();
        StartCoroutine(activecoroutine);
        var main = particle.main;
        main.startSize = TrailColScale * 3;
        main.duration = laserlifetime;
        main.startLifetime = TrailDuration;
        registerActionHandler(() => { /*StopAllCoroutines();*/ StopCoroutine(activecoroutine); });
        base.Invoke( ActionENd,target);
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





    private void Awake()
    {
        boss1SOundManager=this.GetComponent<Boss1SOundManager>();
        laserBeam.gameObject.SetActive(false);
        ColliderSpawnPoint.gameObject.SetActive(false);
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

    private void FixedUpdate()
    {
        if (!laserBeam.gameObject.activeSelf)
            return;
        laserMove();

       
        if(laserlifetime < 0)
        {
            laserBeam.gameObject.SetActive(false);
            particle.Stop();
            if (boss1SOundManager != null)
                boss1SOundManager.LazerStartClipEnd();
        
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
