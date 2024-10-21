using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyActionRush : EnemyAction
{
    Transform target;
    public float rushinitdelay;
    public float rushdistance;
    public float rushspeed;
    public float rushcooltime;
   
    Rigidbody rb;
   IEnumerator rush()
    {
        this.transform.LookAt(target);
        this.transform.rotation =
            Quaternion.Euler(this.transform.rotation.x,
            0, this.transform.rotation.z);
        Vector3 initposition = transform.position;
        yield return new WaitForSeconds(rushinitdelay);
        while((transform.position-
            initposition).magnitude < rushdistance){
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * rushspeed);
        }
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(rushcooltime);
    }
}
