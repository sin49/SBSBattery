using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum inputeventenum {none,characterhit,characterskillinput,
collision,Trigger,ObjectCollision,objectTrigger,objectdelete,
TImerEnd,TImerPlaying, enemySpawn, enemyKill,Die,Control,Moribund,interaction,signalsenderinput}
[SerializeField]
public abstract class InputEvent :MonoBehaviour
{
    [Header("�̺�Ʈ �̸�")]
    public string eventname;
    //public InputEvent();
    public abstract void initialize();
    public abstract bool input(object o=null);
}
