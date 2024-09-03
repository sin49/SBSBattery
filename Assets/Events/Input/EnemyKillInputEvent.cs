using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillInputEvent : InputEvent
{
   
    public GameObject obj;
    public Character[] objGroup=new Character[0];

    public bool reduceNum, increaseNum;

    int arraySize;

    public override void initialize()
    {
        arraySize = objGroup.Length;
    }
    private void Awake()
    {

        objGroup = obj.GetComponentsInChildren<Character>();
        foreach(Character c in objGroup) 
        {
            c.registerdeadevent(EnemyKill);
        }
        initialize();
    }
    public override bool input(object o)
    {
        if (arraySize <= 0)
            return true;
        else
            return false;
    }

    public void EnemyKill()
    {
        arraySize--;
    }
}
