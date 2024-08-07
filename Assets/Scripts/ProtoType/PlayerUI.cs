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
    public RectTransform HpbarTransform;

   void FormUIUpdate()
    {
        switch (PlayerHandler.instance.CurrentType)
        {
            case TransformType.Default:
                PlayerFormText.text = "���͸�";
 
                break;
            case TransformType.remoteform:
                PlayerFormText.text = "������";
   
                break;
            case TransformType.mouseform:
                PlayerFormText.text = "���콺";

                break;
        }
    }

    void HPUIUpdate()
    {
        Hpbar.pixelsPerUnitMultiplier = 0.45f * PlayerStat.instance.hpMax;
        HpbarTransform.sizeDelta = new Vector2(240-(
            (240/PlayerStat.instance.hpMax)*(PlayerStat.instance.hpMax-PlayerStat.instance.hp)
            ), 75f);
        switch (PlayerStat.instance.hp)
        {
            case 1:
                Hpbar.color = Color.red;
                break;
            case 2:
                Hpbar.color = Color.yellow;
                break;
            default:
      Hpbar.color = Color.green;
                break;

        }
    }
    // Update is called once per frame
    void Update()
    {
        FormUIUpdate();
        HPUIUpdate();
    }
}
