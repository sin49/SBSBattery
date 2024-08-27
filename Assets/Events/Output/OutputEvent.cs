using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public enum OutputeventEnum
{none,outputcreate,objectremove, enemySpawn, enemyRemove, enemyTeleport,abilityUP,
    Camerachange,CheckpointSave,ControlActive,ControlChange,ControlDeactive,Description,
    EnemyKill,EnemyMove,killArea,objectActive,ObjectDeactive,PlayerBlowOut,PlayerKill,
    PlayerReturn,PlayerSpawn,ScenePass,SoundPlay,TimerStart,Teleport,TransformChange
}

public abstract class OutputEvent:MonoBehaviour
{
    public string eventname;
    public bool actived;
    public virtual void output() {
        actived = true;
    }
}
