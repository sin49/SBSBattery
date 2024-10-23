using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firebreathattack : MonoBehaviour
{
    public ParticleSystem[] fireeffects; 
    public Light fireLight;
    [Header("�극�� �ִ� ����")]
    public Vector3 breathsize;
    [Header("�극�� �ʱ� ����")]
    public Vector3 breathinitsize;
 
    [Header("�극�� ���� �ð�")]
    public float breathtime;
    [Header("�극�� ������ �ִ밡 �Ǵ� �ð�")]
    public float breathspreadmaxtime;
    [Header("�극�� ������ ������� �ð�")]
    public float breathendtime;


    public Transform breathsmallcollider;
    public Transform breathcollider;

 
    void initializebreath()
    {
      
        breathcollider.gameObject.SetActive(false);
        if (fireLight != null)
            fireLight.gameObject.SetActive(false);
        foreach (var a in fireeffects)
        {
            a.Stop();
        }

        breathsmallcollider.gameObject.SetActive(false);
    }
    public event Action BreathEndEvent;
    public void registerbreathendevent(Action a)
    {
        BreathEndEvent += a;
    }
    private void Awake()
    {
        initializebreath();
        registerbreathendevent(() => { this.gameObject.SetActive(false); });
    }
    private void OnEnable()
    {
        BreathAttack();
    }
    public void BreathAttack()
    {
        initializebreath();
        StartCoroutine(breathattackCorutine());
    }
    IEnumerator breathattackCorutine()
    {
        
        //�������ͽ� ���� �� �극�� ���ӽð� �ֱ�
        foreach (var a in fireeffects)
        {
            var main = a.main;
            main.duration = breathtime + breathendtime;
        }

        //play-> attackcollidder.setacive(true)�� �ٲ� �� particle ���� �ٲٱ�
        foreach (var a in fireeffects)
        {
            a.Play();
        }
        fireLight.gameObject.SetActive(true);
      
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


        BreathEndEvent.Invoke();
    }
}
