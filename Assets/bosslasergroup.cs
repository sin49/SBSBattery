using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[ExecuteAlways]
public class bosslasergroup : MonoBehaviour
{
    public float space;
    public float ypos;
    public Transform[] lasers;
    public Transform[] laserbeams;
    public Transform[] laserswarning ;
    private void Awake()
    {
        updatelaser();
    }
    private void OnEnable()
    {
        updatelaser();
    }
  public void updatelaser()
    {
        int count = transform.childCount;
      
        lasers = new Transform[count];
        laserswarning = new Transform[count];
        laserbeams = new Transform[count];
        for (int n = 0; n < count; n++)
        {
            lasers[n] = transform.GetChild(n);
            laserbeams[n] = transform.GetChild(n).GetChild(0);
            laserbeams[n].gameObject.SetActive(false);
            laserswarning[n] = transform.GetChild(n).GetChild(1);
        }
        float totalspace = 0;
        for (int n = 0; n < lasers.Length; n++)
        {
            if (n % 2 == 1)
            {
                totalspace += space;
            }
            else
            {
                totalspace *= -1;
            }
            lasers[n].transform.localPosition = new Vector3(totalspace, 0, 0);
            if (n % 2 == 0)
                totalspace *= -1;
        }
    }
 public   void activeLaserBeam()
    {
        for (int n = 0; n < laserbeams.Length; n++)
        {
            if (laserbeams[n] == null)
                break;
            laserbeams[n].gameObject.SetActive(true);

        }
    }
    public void DeactiveLaserBeam()
    {
        for (int n = 0; n < laserbeams.Length; n++)
        {
            if (laserbeams[n] == null)
                break;
            laserbeams[n].gameObject.SetActive(false);

        }
    }
    public void activeLaserWarning()
    {
        for (int n = 0; n < laserswarning.Length; n++)
        {
            if (laserswarning[n] == null)
                break;
            laserswarning[n].gameObject.SetActive(true);

        }
    }
    public void DeactiveLaserWarning()
    {
        for (int n = 0; n < laserswarning.Length; n++)
        {
            if (laserswarning[n] == null)
                break;
            laserswarning[n].gameObject.SetActive(false);
          
        }
    }
}
