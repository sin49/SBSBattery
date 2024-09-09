using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSaveOutputEvent : OutputEvent
{
    public CheckPoint saveCheckpoint;
    public override void output()
    {
        CheckpointSave();
        base.output();
    }

    public void CheckpointSave()
    {
        PlayerSpawnManager.Instance.ChangeCheckPoint(saveCheckpoint);
    }
}
