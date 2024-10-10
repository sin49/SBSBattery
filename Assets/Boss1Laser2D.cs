using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Laser2D : EnemyAction
{
    public float LaserWaringTime;

    public float laseractiveTime;

    public List<bosslasergroup> lasers=new List<bosslasergroup>();
    Boss1SOundManager boss1SOundManager;
    public override void StopAction()
    {
        base.StopAction();
        foreach(var a in lasers)
        {
            a.DeactiveLaserWarning();
            a.DeactiveLaserBeam();
        }
    }
    private void Awake()
    {
        foreach (bosslasergroup group in lasers)
        {
            group.gameObject.SetActive(false);
            group.DeactiveLaserBeam();
            group.DeactiveLaserWarning();
        }
        boss1SOundManager = this.GetComponent<Boss1SOundManager>();
    }
    IEnumerator laserpattern()
    {
        Debug.Log("레이저 패턴 체크");
  
        for (int n = 0; n < lasers.Count; n++)
        {
            lasers[n].gameObject.SetActive(true);

            lasers[n].activeLaserWarning();
            if (boss1SOundManager != null)
                boss1SOundManager.LazerinitClipPlay();
            yield return new WaitForSeconds(LaserWaringTime);
            if (boss1SOundManager != null)
                boss1SOundManager.LazerStartClipEnd();
            lasers[n].DeactiveLaserWarning();
            lasers[n].activeLaserBeam();
            yield return new WaitForSeconds(laseractiveTime);
            lasers[n].DeactiveLaserBeam();
            lasers[n].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.25f);
        DisableActionMethod();
    }
    public override void Invoke(Action ActionENd, Transform target = null)
    {
        registerActionHandler(ActionENd);
        StartCoroutine(laserpattern());
    }


}
