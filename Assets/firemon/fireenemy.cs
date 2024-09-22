using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fireenemy : Enemy
{
    [Header("�극�� �� ��� �ð�")]
    public float breathinittime;
    [Header("�극�� ���� �ð�")]
    public float breathtime;
    [Header("�극�� ������ �ִ밡 �Ǵ� �ð�")]
    public float breathspreadmaxtime;
    [Header("�극�� �� ������")]
    public float breathdelay;
    [Header("�극�� ������ ������� �ð�")]
    public float breathendtime;

    public ParticleSystem[] fireeffects;
    public Light fireLight;

    [Header("�ٷ� ���� �簢������ Ŀ���� �簢���� ���� �װ� �ǵ�°�")]
    [Header("�극�� �ִ� ����")]
    public Vector3 breathsize;
    [Header("�극�� �ʱ� ����")]
    public Vector3 breathinitsize;

    [Header("�� �����δ� �׳� �ǵ��� ����")]
    public GameObject breath;

    public Transform breathsmallcollider;
    public Transform breathcollider;
  
    bool oncorutine;
    public override void Damaged(float damage)
    {
        base.Damaged(damage);

        StartCoroutine(waitingdelay());
    }
    public override void Attack()
    {
         
        
        base.Attack();
        StopAllCoroutines();
        initializebreath();
        if (oncorutine)
            return;
        else
        StartCoroutine(breathattack());
    }
    IEnumerator waitingdelay()
    {
        yield return new WaitForSeconds(breathdelay);
        activeAttack = false;
    }
    void initializebreath()
    {
        oncorutine = false;
        breathcollider.gameObject.SetActive(false);
        fireLight.gameObject.SetActive(false);
        foreach (var a in fireeffects)
        {
            a.Pause();
        }

        breathsmallcollider.gameObject.SetActive(false);
    }
    IEnumerator breathattack()
    {
        oncorutine = true;
        foreach (var a in fireeffects)
        {
            var main = a.main;
            main.duration = breathtime + breathendtime;
        }
      
        yield return new WaitForSeconds(breathinittime);
        foreach (var a in fireeffects)
        {
            a.Play();
        }
        fireLight.gameObject.SetActive(true);
        breath.SetActive(true);
        breathsmallcollider.gameObject.SetActive(true);
        breathcollider.gameObject.SetActive(true);
        float timer = 0;
        float breathtimer = 0;
        breathcollider.transform.localPosition = Vector3.forward * -0.5f;
        breathcollider.localScale = breathinitsize;
        Vector3 calculatevector = (breathsize - breathinitsize) / breathspreadmaxtime;

        while (breathtimer < breathspreadmaxtime)
        {
           
            breathcollider.localScale += calculatevector * Time.deltaTime;
            breathcollider.Translate(Vector3.back * calculatevector.z * Time.deltaTime * 0.5f);
            timer += Time.deltaTime;
            breathtimer += Time.deltaTime;
            yield return null;
        }
      
        yield return new WaitForSeconds(breathtime - timer);
      
        breathsmallcollider.gameObject.SetActive(false);

        timer = 0;

        calculatevector = Vector3.forward * (breathsize.z / breathendtime);
        while (timer < breathendtime)
        {
           
            breathcollider.localScale -= calculatevector * Time.deltaTime;
            breathcollider.Translate(Vector3.back * calculatevector.z * Time.deltaTime * 0.5f);
            timer += Time.deltaTime;
            yield return null;

        }
        breathcollider.gameObject.SetActive(false);
        fireLight.gameObject.SetActive(false);
        yield return new WaitForSeconds(breathdelay);
        oncorutine = false;
        InitAttackCoolTime();
    }
}
