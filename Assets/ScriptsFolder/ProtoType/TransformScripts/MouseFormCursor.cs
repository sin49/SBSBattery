using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MouseFormCursor : MonoBehaviour
{
    public bool onCatch;
    public GameObject cursorParent;
    public CursorInteractObject interactObj;
    public float forwardThrowForce;
    public float upThrowForce;
    [Header("0번 잡았을 때 1번 놓았을 때")]
    public SoundEffectListPlayer soundEffectListPlayer;

    public ParticleSystem clickEffect;
    GameObject playerRotate;

    public DontMoveCollider dontMove;
    public Vector3 dontMoveOriginScale;

    public float dontMoveScalevalue;

    public Vector3 saveCursorPos;
    public float upPos, forwardPos;
    private void Awake()
    {
        playerRotate = GetComponentInParent<Player>().transform.GetChild(0).gameObject;

        soundEffectListPlayer=GetComponent<SoundEffectListPlayer>();

    }

    private void Update()
    {
        UpdateCursorPos();
    }    

    public void UpdateCursorPos()
    {
        if (interactObj != null)
        {
            interactObj.transform.position = cursorParent.transform.position + PlayerHandler.instance.CurrentPlayer.transform.GetChild(0).forward * interactObj.ColliderEndPoint();
            interactObj.transform.rotation = playerRotate.transform.rotation;
        }

        //Debug.Log((transform.parent.position - PlayerHandler.instance.CurrentPlayer.transform.position));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!onCatch)
        {
            CursorInteractObject cursorInteract;
            if (other.TryGetComponent<CursorInteractObject>(out cursorInteract))
            {
                if (cursorInteract.CompareTag("CursorObject"))
                {
                    cursorInteract.AddComponent<CursorInteractObjectCheck>();
                    cursorInteract.GetComponent<CursorInteractObjectCheck>().cursorParent = transform.parent.gameObject;
                }
                //dontMove.ChangeScaleByFormCursor(dontMoveScalevalue);
                onCatch = true;
                soundEffectListPlayer.PlayAudio(0);
                interactObj = cursorInteract;
                interactObj.GetComponent<Rigidbody>().useGravity = false;
                interactObj.GetComponent<Rigidbody>().isKinematic = true;
                interactObj.GetComponent<Collider>().isTrigger = true;
                other.transform.position = cursorParent.transform.position;
                clickEffect.Play();

                cursorInteract.caught = true;
                cursorInteract.CaughtTypeCheck();

                if (cursorInteract.CompareTag("CursorObject"))
                {
                    other.transform.rotation = Quaternion.identity;
                    cursorInteract.gameObject.layer = LayerMask.NameToLayer("DontMoveIgnore");
                }
                Debug.Log(other.gameObject);
                if (other.TryGetComponent<fireenemy>(out fireenemy fire))
                {
                    Debug.Log("불몬 잡았나?");
                    ParticleSystem[] breath = fire.fireeffects;
                    foreach (ParticleSystem a in breath)
                    {
                        a.gameObject.SetActive(false);
                    }

                    fire.breathsmallcollider.gameObject.SetActive(false);
                    fire.breathcollider.gameObject.SetActive(false);
                    Debug.Log("불몬 화염방사 취소되나?");
                }
                else
                {
                    Debug.Log("불몬 안잡힘");
                }
            }
        }
    }

    public void InteractTypeCheck()
    {
        Debug.Log("체크하러 마실 나왔습니다");
        clickEffect.Play();
        CursorInteractObject cursorInteract;
        if (interactObj != null && interactObj.TryGetComponent<CursorInteractObject>(out cursorInteract))
        {
            Enemy enemy;
            if (interactObj.TryGetComponent<Enemy>(out enemy))
            {
                Debug.Log("몬스터 고객님이시네요");
                ThrowMonster();
            }
            else
            {
                Debug.Log("플랫폼 고객님이시네요");
                DropPlatformObject();
            }
            if(cursorInteract.CompareTag("CursorObject"))
            Destroy(cursorInteract.GetComponent<CursorInteractObjectCheck>());
        }
        //dontMove.ReturnScale();                
    }

    public void ThrowMonster()
    {
        Enemy enemy;
        if (interactObj.TryGetComponent<Enemy>(out enemy))
        {
            enemy.gameObject.layer = LayerMask.NameToLayer("Default");

            enemy.GetComponent<Collider>().isTrigger = false;
            enemy.GetComponent<Rigidbody>().useGravity = true;
            enemy.GetComponent<Rigidbody>().isKinematic = false;
            enemy.GetComponent<Rigidbody>().AddForce(transform.forward * forwardThrowForce + transform.up * upThrowForce, ForceMode.VelocityChange);

            interactObj = null;
            onCatch = false;
            soundEffectListPlayer.PlayAudio(1);
            RagdolEnemy re;
            if (enemy.TryGetComponent<RagdolEnemy>(out re))
            {
                re.ThrowRagdoll();
            }
        }
    }

    public void DropPlatformObject()
    {
        interactObj.gameObject.layer = LayerMask.NameToLayer("Default");
        interactObj.GetComponent<Collider>().isTrigger = false;
        interactObj.GetComponent<Rigidbody>().useGravity = true;
        interactObj.GetComponent<Rigidbody>().isKinematic = false;
        interactObj = null;
        onCatch = false;
        soundEffectListPlayer.PlayAudio(1);
    }

    public void InitCursorPos()
    {
        saveCursorPos = (PlayerHandler.instance.CurrentPlayer.transform.GetChild(0).forward * forwardPos) +
            (PlayerHandler.instance.CurrentPlayer.transform.GetChild(0).up * upPos);
        cursorParent.transform.position = (PlayerHandler.instance.CurrentPlayer.transform.GetChild(0).position + saveCursorPos);
        cursorParent.transform.rotation = PlayerHandler.instance.CurrentPlayer.transform.GetChild(0).rotation;
    }
}
