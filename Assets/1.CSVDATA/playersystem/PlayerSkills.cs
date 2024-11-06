using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkills : ScriptableObject
{
    public abstract void Invoke();

    public abstract void initEvent();
}
