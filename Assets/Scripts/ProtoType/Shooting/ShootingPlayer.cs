using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UIElements;

public enum ShootingDirection {right=1,none=0,UP=3,Down=-3,left=-1,right_up=4,right_down=-2,Left_up=2,Left_down=-4 }
public class ShootingPlayer : ShootingObject
{
    public static ShootingPlayer instance;
    public TextMeshProUGUI LifeUI;
    public Transform ShootPos;
    //ShootingDirection direction;
    public Transform Sprite;
    public float untouchableTime;
    float untouchableTimer;
    public ShootingFIeld field;

    public event Action ShootingPlayerDieEvent;
    public override void hitted()
    {

        if (untouchableTimer <= 0&&!field.active)
        {
            base.hitted();
            if (currenthp <= 0)
            {
                ShootingPlayerDieEvent?.Invoke();
                ShootingPlayerDieEvent = null;
            }
                untouchableTimer = untouchableTime;
        }
    }
    Vector3 movedirection;
    private void Awake()
    {
        instance = this;
        TargetVector = Vector2.up;
    }
  
 public void rotateSprite(Vector3 vec)
    {
        vec = vec.normalized;
        if (vec.x == 1)
        {
            Sprite.localRotation = Quaternion.Euler(0, 0, -90);
            //direction = ShootingDirection.right;
            TargetVector = Vector2.right;
        }
        else if (vec.x == -1)
        {
            Sprite.localRotation = Quaternion.Euler(0, 0, 90);
            //direction = ShootingDirection.left;
            TargetVector= Vector2.left;
        }
        else if (vec.y == -1)
        {
            Sprite.localRotation = Quaternion.Euler(0, 0, 180);
            //direction = ShootingDirection.Down;
            TargetVector=Vector2.down;
        }
        else if (vec.x > 0 && vec.y > 0)
        {
            Sprite.localRotation = Quaternion.Euler(0, 0, -45);
            //direction = ShootingDirection.right_up;
            TargetVector = (Vector2.right + Vector2.up) * 0.5f;
        }
        else if (vec.x > 0 && vec.y < 0)
        {
            Sprite.localRotation = Quaternion.Euler(0, 0, -135);
            //direction = ShootingDirection.right_down;
            TargetVector = (Vector2.right + Vector2.down) * 0.5f;
        }
        else if (vec.x < 0 && vec.y > 0)
        {
            Sprite.localRotation = Quaternion.Euler(0, 0, 45);
            //direction = ShootingDirection.Left_up;
            TargetVector = (Vector2.left + Vector2.up) * 0.5f;
        }
        else if (vec.x < 0 && vec.y < 0)
        {
            Sprite.localRotation = Quaternion.Euler(0, 0, 135);
            //direction = ShootingDirection.Left_down;
            TargetVector = (Vector2.left + Vector2.down) * 0.5f;
        }
        else if (vec.y == 1)
        {
            Sprite.localRotation = Quaternion.Euler(0, 0, 0);
            //direction = ShootingDirection.Down;
            TargetVector = Vector2.up;
        }
        
    }

    public void Move()
    {
     
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        movedirection = new Vector3(-1*hori, vert, 0).normalized;
        rotateSprite(movedirection);
        transform.Translate(movedirection * Time.deltaTime* movespeed);
    }

    public IEnumerator PlayerAttack()
    {
        var bullet = Instantiate(Bullet, ShootPos.position, this.transform.rotation);
        bullet.GetComponent<ShootingBullet>().Setbullet(bulletspeed, TargetVector.normalized, bulletlifetime, true);
        onshoot = true;
        yield return new WaitForSeconds(AttackDelay);
        onshoot = false;
    }

   
    private void FixedUpdate()
    {
  
        LifeUI.text = $"Life:{currenthp}";


      if(untouchableTimer>0)
            untouchableTimer-=Time.deltaTime;
        Move();
        if (Input.GetKey(KeyCode.X)&&!onshoot)
        {
           
            StartCoroutine(PlayerAttack());
        }
    }

}
