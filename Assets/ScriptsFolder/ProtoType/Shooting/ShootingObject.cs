using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObject : MonoBehaviour
{
    public float movespeed;
    public int Hp;
  protected int currenthp;
    public float AttackDelay;
    public GameObject Bullet;
    public float bulletspeed;
    public float bulletlifetime;
    protected Vector2 TargetVector;
    public bool Player;
   protected WaitForSeconds corutineseconds;
  protected  bool onshoot;
    protected Vector3 initposition;

    public virtual void Start()
    {
        corutineseconds =new WaitForSeconds( AttackDelay);
       
    }
    private void OnEnable()
    {
        currenthp = Hp;
        if(initposition==Vector3.zero)
            initposition = this.transform.position;
        else
         this.transform.position = initposition;
    }
    public virtual void hitted()
    {
        currenthp--;
        if (currenthp <= 0)
            this.gameObject.SetActive(false);
    }
   
}
