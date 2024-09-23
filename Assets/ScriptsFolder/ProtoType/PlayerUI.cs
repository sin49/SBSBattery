using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI: MonoBehaviour
{
    public TextMeshProUGUI PlayerFormText;
    public Image Hpbar;
    public Sprite Empty;
    public Sprite Fill;
    public Transform HPbartransform;
    List<Image> HPbarList=new List<Image>();

   void FormUIUpdate()
    {
        switch (PlayerHandler.instance.CurrentType)
        {
            case TransformType.Default:
                PlayerFormText.text = "배터리";
 
                break;
            case TransformType.remoteform:
                PlayerFormText.text = "리모컨";
   
                break;
            case TransformType.mouseform:
                PlayerFormText.text = "마우스";

                break;
        }
    }

    void HPUIUpdate()
    {
        float hp = PlayerStat.instance.hp;
        float maxhp = PlayerStat.instance.hpMax;
       
       
     for(int n = 0; n < maxhp; n++)
        {
            
                if (HPbarList.Count <= n)
            {
            
             
               var a=     Instantiate(Hpbar.gameObject, HPbartransform).GetComponent<Image>();
                HPbarList.Add(a);
            }
            if (n <= hp)
                HPbarList[n].sprite = Fill;
            else
                HPbarList[n].sprite = Empty;
        }
    }
    private void Start()
    {
        PlayerStat.instance.registerRecoverAction(HPUIUpdate);
        PlayerStat.instance.registerHPLoseAction(HPUIUpdate);
        PlayerInventory.instance.registerItemGetAction(HPUIUpdate);
        HPUIUpdate();
    }
    // Update is called once per frame
    void Update()
    {
        FormUIUpdate();
        HPUIUpdate();
    }
}
