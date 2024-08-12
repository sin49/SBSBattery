using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Laser : MonoBehaviour
{
    public Transform Target;

    public float LaserSpeed;

    public Transform ColliderSpawnPoint;

    public Transform laserBeam;

    public float TrailColScale = 1;

    public float TrailDuration;

    public float ColliderSpawnTime;

    public TrailRenderer TrailRenderer;

    int ActiveTrailColNumber;

    public Transform pullingtransform;

    public Boss1LaserCollider LaserCollider;

    Queue<Boss1LaserCollider> LaserPulling=new Queue<Boss1LaserCollider>();

    public void activelaser(Transform playerpostion)
    {
        laserBeam.transform.position =-8f*Vector3.up;
    }



    private void OnEnable()
    {
        StartCoroutine(laserPattern());
    }
    private void Awake()
    {
        if (TrailRenderer != null)
        {
            TrailRenderer.time = TrailDuration;
        }
    }
   
    IEnumerator laserPattern()
    {
        while (true)
        {
            CreateLaser();
            yield return new WaitForSeconds(ColliderSpawnTime);
        }
    }
    void laserMove()
    {
        if (Target != null)
        {
            Vector3 LaserVector = (Target.transform.transform.position - laserBeam.transform.position).normalized;
            LaserVector.y = 0;
            laserBeam.Translate(LaserVector * LaserSpeed * Time.fixedDeltaTime);
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
        laserMove();
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
