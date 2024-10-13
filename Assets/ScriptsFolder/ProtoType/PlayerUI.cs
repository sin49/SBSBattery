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
    public List<Sprite> formList = new List<Sprite>();
    public Image currentFormImage;
   void FormUIUpdate()
    {
        switch (PlayerHandler.instance.CurrentType)
        {
            case TransformType.Default:
                PlayerFormText.text = "배터리";
                currentFormImage.sprite = formList[0];
                break;
            case TransformType.remoteform:
                PlayerFormText.text = "리모컨";
                currentFormImage.sprite = formList[1];
                break;
            case TransformType.mouseform:
                PlayerFormText.text = "마우스";
                currentFormImage.sprite = formList[2];
                break;
            case TransformType.ironform:
                currentFormImage.sprite = formList[3];
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
            if (n < hp)
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
