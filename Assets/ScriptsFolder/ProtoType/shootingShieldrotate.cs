using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class shootingShieldrotate : MonoBehaviour
{
    public Vector3 Target;
    public float rotatespeed;
    void Update()
    {
        Vector3 lookdir = Target - transform.position;
        lookdir.x *= -1;
   
        Quaternion targetrot = Quaternion.LookRotation(Vector3.forward, lookdir);
        targetrot.z -= 180;
        transform.rotation = targetrot;
        var a = Quaternion.RotateTowards(transform.rotation, targetrot, rotatespeed * Time.deltaTime);
        
        transform.rotation = Quaternion.Euler(0, 0, a.eulerAngles.z);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShootingBullet"))
        {
            ShootingBullet s;
            if (other.TryGetComponent<ShootingBullet>(out s))
            {
                if (s.Player)
                {
                    Destroy(s.gameObject);
                }
            }
        }
    }
   
}
