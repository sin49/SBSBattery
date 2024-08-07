using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackTypeScript : MonoBehaviour
{
    TextMeshProUGUI text;
    private void Awake()
    {
        text = this.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        /*string s = "attackType:";
        if (PlayerStat.instance.attackType == AttackType.melee)
            s += "melee";
        else
            s += "range";
        text.text = s;*/

        /*string s = "attackType:";
        if (PlayerHandler.instance.CurrentPlayer.attackType == AttackType.melee)
            s += "melee";
        else
            s += "range";
        text.text = s;*/
    }
}
