using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Laser : EnemyAction
{
    Transform Target;




    public float LaserSpeed;

    public Transform warning;

    float laserlifetime;

    public float laserActiveTimer = 1.5f;

    public Transform ColliderSpawnPoint;

    public Transform Laser;

    public Transform laserBeam;

    public float TrailColScale = 1;

    public float TrailDuration;

    public float ColliderSpawnTime;

    public TrailRenderer TrailRenderer;

    int ActiveTrailColNumber;

    public Transform pullingtransform;

    public Boss1LaserCollider LaserCollider;

    Queue<Boss1LaserCollider> LaserPulling=new Queue<Boss1LaserCollider>();

    IEnumerator activecoroutine;

    public override void Invoke(Action ActionENd, Transform target = null)
    {
        this.Target = target;
        Laser.transform.position = new Vector3(target.position.x, -8,
           target.position.z);
        laserBeam.gameObject.SetActive(false);
        warning.position = Laser.transform.position-Vector3.up*2;
        laserlifetime = ActionLifeTIme;
        warning.gameObject.SetActive(true);
        activecoroutine = laserPattern();
        StartCoroutine(activecoroutine);
        registerActionHandler(() => { /*StopAllCoroutines();*/ StopCoroutine(activecoroutine); });
        base.Invoke( ActionENd,target);
    }



    protected override void CancelActionEvent()
    {
        laserlifetime = 0;
        laserBeam.gameObject.SetActive(false);
        TrailRenderer.gameObject.SetActive(false);
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
        if (TrailRenderer != null)
        {
            TrailRenderer.time = TrailDuration;
        }
        laserBeam.gameObject.SetActive(false);
        ColliderSpawnPoint.gameObject.SetActive(false);
    }
   
    IEnumerator laserPattern()
    {
        yield return new WaitForSeconds(laserActiveTimer);
        warning.gameObject.SetActive(false);
        laserBeam.gameObject.SetActive(true);
        ColliderSpawnPoint.gameObject.SetActive(true);
        TrailRenderer.emitting = true;
        while (true)
        {
            if (!laserBeam.gameObject.activeSelf)
                break;
            CreateLaser();
            yield return new WaitForSeconds(ColliderSpawnTime);
        }
        TrailRenderer.emitting = false;
        
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
        ActiveTrailColNumber--;
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
        ActiveTrailColNumber++;

        col.initLaserCollider(TrailDuration, Vector3.one* TrailColScale,
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
        }else
            laserlifetime -= Time.fixedDeltaTime;

        if (!TrailRenderer.emitting && ActiveTrailColNumber == 0&& TrailRenderer.gameObject.activeSelf)
            TrailRenderer.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerHandler.instance.CurrentPlayer.onInvincible)
        {
            Debug.Log("레이저 피해");
            //other.GetComponent<Player>().Damaged(1);
        }
    }
}
