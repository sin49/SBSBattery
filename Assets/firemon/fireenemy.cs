using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fireenemy : Enemy
{
    public float breathinittime;
    public float breathtime;
    public float breathdelay;
    public float breathendtime;
    public GameObject breath;

    public Transform breathsmallcollider;
    public Transform breathcollider;
    public Vector3 breathsize;
    public Vector3 breathinitsize;
    bool oncorutine;
    public override void Attack()
    {
        if (oncorutine)
            return;
        base.Attack();
        StartCoroutine(breathattack());
    }
    IEnumerator breathattack()
    {
        oncorutine = true;
        yield return new WaitForSeconds(breathinittime);
        breath.SetActive(true);
        breathsmallcollider.gameObject.SetActive(true);
        breathcollider.gameObject.SetActive(true);
        float timer = 0;

        breathcollider.transform.localPosition = Vector3.forward * -0.5f;
        breathcollider.localScale = breathinitsize;
        Vector3 calculatevector = (breathsize - breathinitsize) / breathtime;

        while (timer < breathtime)
        {
            breathcollider.localScale += calculatevector * Time.deltaTime;
            breathcollider.Translate(Vector3.back * calculatevector.z * Time.deltaTime * 0.5f);
            timer += Time.deltaTime;
            yield return null;
        }
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
        yield return new WaitForSeconds(breathdelay);
        oncorutine = false;
    }
}
