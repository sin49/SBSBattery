using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public enum OutputeventEnum
{none,outputcreate,objectremove, enemySpawn}

public abstract class OutputEvent:MonoBehaviour
{
    public string eventname;
    public bool actived;
    public virtual void output() {
        actived = true;
    }
}
