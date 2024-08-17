using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
public class ConvertCoordiate {
};
public class ShootingFIeld: signalSender
{
    public Camera PlatformerCam;
    public Camera ShootingCam;

    public ShootingScroll scroll;
    public Vector2 FieldSize;
    public Vector2 Center;
    public folder[] folders;
    int currentfolderindex;
    Queue<ShootingEnemyGroup> shootingwavesqueue = new Queue<ShootingEnemyGroup>();
    public ShootingPlayer player;
    public float nextwavetime;

    public TextMeshProUGUI CompleteTExt;

    [Header("World Position")]
  public  float MaxSizeX;
   public float MinSizeX;
    public float MaxSizeY;
    public float MinSizeY;

    public float SnapPoint;


    public void EndShooting()
    {
        PlatformerCam.gameObject.SetActive(true);
        ShootingCam.gameObject.SetActive(false);

        PlayerHandler.instance.Deform();
       
    }
    void DefeatShooting()
    {
        PlayerStat.instance.hp--;
        EndShooting();
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        startshooting();
    }
    public void startshooting()
    {
        CompleteTExt.gameObject.SetActive(false);
        PlatformerCam.gameObject.SetActive(false);
        ShootingCam.gameObject.SetActive(true);
        player.ShootingPlayerDieEvent += DefeatShooting;
        player.gameObject.SetActive(true);
        player.field = this;
        currentfolderindex = 0;
        if (folders.Length > 0)
        {
            folders[currentfolderindex].gameObject.SetActive(true);
            folders[currentfolderindex].portal.Portalevent+=
               activefolders;
            getenemywaves(folders[currentfolderindex]);
            activewave();
        }
    }
    void activewave()
    {
        var obj = shootingwavesqueue.Dequeue();
   
        obj.gameObject.SetActive(true);
        obj.startwave();
    }
  public  void activefolders()
    {
        currentfolderindex++;
        var a = folders[currentfolderindex];
        getenemywaves(a);
        a.gameObject.SetActive(true);
        activewave();
    }
    bool foldersCheck()
    {
       
            if (shootingwavesqueue.Count == 0)
            {
            if (currentfolderindex != folders.Length-1)
            {
                folders[currentfolderindex].GetComponent<folder>().activeportal();

             
            }
            else
            {
              
                CompleteTExt.gameObject.SetActive(true);
                active = true;
                Send(active);
                EndShooting();
             
            }
            return false;
            }
            else
                return true;
        
        
    }

    void getenemywaves(folder g)
    {
        var a = g.enemylist.GetComponentsInChildren<ShootingEnemyGroup>();
        foreach (var s in a)
        {

            s.OnwaveCleard += gonextwave;
            shootingwavesqueue.Enqueue(s);
            s.gameObject.SetActive(false);
        }
        scroll.setSlide(a.Length);
    
    }
    void gonextwave(ShootingEnemyGroup g)
    {
        g.OnwaveCleard -= gonextwave;
        //if(g.gameObject.activeSelf)
        //    g.gameObject.SetActive(false);
        scroll.GetVaule();
        if (foldersCheck())
            activewave();
    }

   
    private void Update()
    {
        GetSize();
        if (player != null)
            snapFieldPosition(player.transform);
    }
    void GetSize()
    {
      
        MaxSizeX =  (Center.x + FieldSize.x)/2- SnapPoint;
        MinSizeX = -1 * (Center.x + FieldSize.x) / 2+ SnapPoint;
        MaxSizeY = ( FieldSize.y)/2+Center.y- SnapPoint;
        MinSizeY =  -1* ( FieldSize.y) / 2+Center.y+ SnapPoint;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position+ (Vector3)Center, new Vector3(0,FieldSize.y,FieldSize.x));
    }

 
    public override void register(signalReceiver receiver, int index)
    {
        Receiver.Add( receiver);
        signalnumber = index;
    }

   

    void snapFieldPosition(Transform tf)
    {

        if (tf.localPosition.x > MaxSizeX)
            tf.localPosition = new Vector3(MaxSizeX, transform.localPosition.y, transform.localPosition.z);
        else if (tf.localPosition.x < MinSizeX)
            tf.localPosition = new Vector3(MinSizeX, transform.localPosition.y, transform.localPosition.z);
        if (tf.localPosition.y > MaxSizeY)
            tf.localPosition = new Vector3(transform.localPosition.x, MaxSizeY, transform.localPosition.z);
        else if (tf.localPosition.y < MinSizeY)
            tf.localPosition = new Vector3(transform.localPosition.x, MinSizeY, transform.localPosition.z);


    }
}
