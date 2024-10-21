using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//�ڷ�ƾ ���빰�� ������� �ڵ�� ���� �� �� attackcollider�� ��ƼŬ+�ؼ� ���̱�
//�ּ� ó���Ѱ� �����
public class EnemyAction_breath : NormalEnemyAction
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

    //public ParticleSystem[] fireeffects; ->attackcollider
    public Light fireLight;

    [Header("�ٷ� ���� �簢������ Ŀ���� �簢���� ���� �װ� �ǵ�°�")]
    [Header("�극�� �ִ� ����")]
    public Vector3 breathsize;
    [Header("�극�� �ʱ� ����")]
    public Vector3 breathinitsize;

    [Header("�� �����δ� �׳� �ǵ��� ����")]
    //public GameObject breath;

    //�극�� ���� �ڵ� ���� �ļ� ��ƼŬ���� ó��
    //public Transform breathsmallcollider;
    //public Transform breathcollider;

    bool oncorutine;
    void initializebreath()
    {
        oncorutine = false;
        //breathcollider.gameObject.SetActive(false);
        if (fireLight != null)
            fireLight.gameObject.SetActive(false);
        //foreach (var a in fireeffects)
        //{
        //    a.Stop();
        //}

        //breathsmallcollider.gameObject.SetActive(false);
    }
    public override void Invoke(Transform target = null)
    {
        base.Invoke(target);
    
        if (oncorutine)
            return;
        else
        {
            if (TryGetComponent<RagdolEnemy>(out RagdolEnemy re))
            {
                initializebreath();
                if (!re.isRagdoll)
                    StartCoroutine(breathattack());
            }
        }
    }
  
    IEnumerator breathattack()
    {
        oncorutine = true;
        //�������ͽ� ���� �� �극�� ���ӽð� �ֱ�
        //foreach (var a in fireeffects)
        //{
        //    var main = a.main;
        //    main.duration = breathtime + breathendtime;
        //}

        yield return new WaitForSeconds(breathinittime);
        //play-> attackcollidder.setacive(true)�� �ٲ� �� particle ���� �ٲٱ�
        //foreach (var a in fireeffects)
        //{
        //    a.Play();
        //}
        fireLight.gameObject.SetActive(true);
        //breath.SetActive(true);
        //breathsmallcollider.gameObject.SetActive(true);
        //breathcollider.gameObject.SetActive(true);
        float timer = 0;
        float breathtimer = 0;
        //breathcollider.transform.localPosition = Vector3.forward * -0.5f;
        //breathcollider.localScale = breathinitsize;
        Vector3 calculatevector = (breathsize - breathinitsize) / breathspreadmaxtime;

        while (breathtimer < breathspreadmaxtime)
        {

            //breathcollider.localScale += calculatevector * Time.deltaTime;
            //breathcollider.Translate(Vector3.back * calculatevector.z * Time.deltaTime * 0.5f);
            timer += Time.deltaTime;
            breathtimer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(breathtime - timer);

        //breathsmallcollider.gameObject.SetActive(false);

        timer = 0;

        calculatevector = Vector3.forward * (breathsize.z / breathendtime);
        while (timer < breathendtime)
        {

            //breathcollider.localScale -= calculatevector * Time.deltaTime;
            //breathcollider.Translate(Vector3.back * calculatevector.z * Time.deltaTime * 0.5f);
            timer += Time.deltaTime;
            yield return null;

        }
        //breathcollider.gameObject.SetActive(false);
        fireLight.gameObject.SetActive(false);
        //foreach (var a in fireeffects)
        //{
        //    a.Stop();
        //}
        yield return new WaitForSeconds(breathdelay);
        oncorutine = false;
        DisableActionMethod();
    }
}
