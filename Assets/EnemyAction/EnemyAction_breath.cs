using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//코루틴 내용물을 끄집어내서 코드로 따로 판 후 attackcollider에 파티클+해서 붙이기
//주석 처리한거 다잡기
public class EnemyAction_breath : NormalEnemyAction
{
    [Header("브레스 전 대기 시간")]
    public float breathinittime;
    [Header("브레스 지속 시간")]
    public float breathtime;
    [Header("브레스 범위가 최대가 되는 시간")]
    public float breathspreadmaxtime;
    [Header("브레스 후 딜레이")]
    public float breathdelay;
    [Header("브레스 범위가 사라지는 시간")]
    public float breathendtime;

    //public ParticleSystem[] fireeffects; ->attackcollider
    public Light fireLight;

    [Header("바로 앞의 사각형말고 커지는 사각형이 있음 그거 건드는거")]
    [Header("브레스 최대 범위")]
    public Vector3 breathsize;
    [Header("브레스 초기 범위")]
    public Vector3 breathinitsize;

    [Header("이 밑으로는 그냥 건들지 마샘")]
    //public GameObject breath;

    //브레스 관련 코드 따로 파서 파티클에서 처리
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
        //스테이터스 만들 때 브레스 지속시간 넣기
        //foreach (var a in fireeffects)
        //{
        //    var main = a.main;
        //    main.duration = breathtime + breathendtime;
        //}

        yield return new WaitForSeconds(breathinittime);
        //play-> attackcollidder.setacive(true)로 바꾼 후 particle 설정 바꾸기
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
