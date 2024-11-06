using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "NewTestScriptableObject", menuName = "ScriptableObjects/PDefaultAttack")]
public class PlayerDefaultAttack : PlayerSkills
{

    Player p;
    public Vector3 MeleeColliderLocalPosition;

    public Vector3 MeleeColliderLocalrotation;

    public Vector3 MeleeColliderLocalscale;

    public GameObject MeleeCollider;

    GameObject MeleeCOlliderInstance_;

 public event   Action attackEvent_;

    public Vector3 MeleeEffectLocalPosition;

    public Vector3 MeleeEffectLocalrotation;

    public Vector3 MeleeEffectLocalscale;


    public GameObject MeleeEffectCollider;

    GameObject MeleeEffectInstance;

    void initialzeAttackEffect()
    {
        if (p != null)
        {
            if (p.Attackcollider != null)
                Destroy(p.Attackcollider);
            if (p.AttackEffect != null)
                Destroy(p.AttackEffect);
        }
        else
        {
            Debug.Log("P is Null");
        }
    }

    public override void initEvent()
    {


        if (PlayerHandler.instance != null)
        {
            p = PlayerHandler.instance.CurrentPlayer;
            initialzeAttackEffect();
        }
        else
        {
            Debug.Log("Can't find Player");
            return;
        }
        attackEvent_ += p.AttackEvents;

        MeleeCOlliderInstance_ = Instantiate(MeleeCollider, p.AttackColliders);
        MeleeCOlliderInstance_.transform.localPosition = MeleeColliderLocalPosition;
        MeleeCOlliderInstance_.transform.localRotation = Quaternion.Euler(MeleeColliderLocalrotation);
        MeleeCOlliderInstance_.transform.localScale= MeleeColliderLocalscale;

        MeleeCOlliderInstance_.SetActive(false);
        p.Attackcollider= MeleeCOlliderInstance_;

        MeleeEffectInstance = Instantiate(MeleeEffectCollider, p.Effects);
        MeleeEffectInstance.transform.localPosition = MeleeEffectLocalPosition;
        MeleeEffectInstance.transform.localRotation = Quaternion.Euler(MeleeEffectLocalrotation);
        MeleeEffectInstance.transform.localScale = MeleeEffectLocalscale;

        MeleeEffectInstance.SetActive(false);

        p.AttackEffect = MeleeEffectInstance;
    }


    void AttackMove()
    {

        Rigidbody playerRb= p.GetComponent<Rigidbody>();
  
            if ((int)PlayerStat.instance.MoveState < 4 )
            {
                playerRb.AddForce(PlayerHandler.instance.CurrentPlayer.transform.GetChild(0).forward * 7, ForceMode.Impulse);
            }
            else if ((int)PlayerStat.instance.MoveState >= 4)
            {
               
                    playerRb.AddForce(PlayerHandler.instance.CurrentPlayer.transform.GetChild(0).forward * 7, ForceMode.Impulse);
        
            }
    
    }


    public override void Invoke()
    {
        if (MeleeCOlliderInstance_ == null)
            return;
        AttackMove();

        MeleeCOlliderInstance_.SetActive(true);
        MeleeCOlliderInstance_.GetComponent<SphereCollider>().enabled = true;

        if (MeleeEffectCollider != null)
        {
            MeleeEffectInstance.SetActive(false);
            MeleeEffectInstance.SetActive(true);
        }

        attackEvent_?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
