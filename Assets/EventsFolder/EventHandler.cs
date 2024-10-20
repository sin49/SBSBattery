using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EventHandler : MonoBehaviour
{


    public float EventCheckDelay;


    public bool or;

    public bool startonawake;

    public bool loop;
    [HideInInspector]
  public  bool evenactive;

    public float EventDisabletimer;


    public bool eventcomplete;

    [SerializeField, HideInInspector]
    string inputeventname;
    [SerializeField,HideInInspector]
     inputeventenum inputenum;
    [HideInInspector]
    public List<InputEvent> inputevents;
    [SerializeField, HideInInspector]
    string outputeventname;
    [SerializeField, HideInInspector]
    OutputeventEnum outputenum;
    public List<OutputEvent> outputevents;

    bool checker;
    public void DeleteInputEvent(int n)
    {
        inputevents.RemoveAt(n);
    }
    public void DeleteoutputEvent(int n)
    {
        outputevents.RemoveAt(n);
    }
    public void CreateOutputEvent()
    {
        OutputEvent oevent = null;
        switch (outputenum)
        {
            case OutputeventEnum.outputcreate:
                oevent = gameObject.AddComponent<ObjectCreateOutputEvent>();
                break;
            case OutputeventEnum.objectremove:
                oevent = gameObject.AddComponent<ObjectRemoveEvent>();
                break;
            case OutputeventEnum.enemySpawn:
                oevent = gameObject.AddComponent<EnemySpawnOutputEvent>();
                break;
            case OutputeventEnum.enemyRemove:
                oevent = gameObject.AddComponent<EnemyRemoveOutputEvent>();
                break;
            case OutputeventEnum.enemyTeleport:
                oevent = gameObject.AddComponent<EnemyTeleportOutputEvent>();
                break;
            case OutputeventEnum.abilityUP:
                oevent = gameObject.AddComponent<AbilityUpOutputEvent>();
                break;
            case OutputeventEnum.Camerachange:
                oevent = gameObject.AddComponent<CameraChangerEvent>();
                break;
            case OutputeventEnum.CheckpointSave:
                oevent = gameObject.AddComponent<CheckPointSaveOutputEvent>();
                break;
            case OutputeventEnum.ControlActive:
                oevent = gameObject.AddComponent<ControlActiveOutputEvent>();
                break;
            case OutputeventEnum.ControlChange:
                oevent = gameObject.AddComponent<ControlChangeOutputEvent>();
                break;
            case OutputeventEnum.ControlDeactive:
                oevent = gameObject.AddComponent<ControlDeactiveOutputEvent>();
                break;
            case OutputeventEnum.Description:
                oevent = gameObject.AddComponent<DescriptionEvent>();
                break;
            case OutputeventEnum.EnemyMove:
                oevent = gameObject.AddComponent<EnemyMoveOutputEvent>();
                break;
            case OutputeventEnum.EnemyKill:
                oevent = gameObject.AddComponent<EnemyKillOutputEvent>();
                break;
            case OutputeventEnum.killArea:
                oevent = gameObject.AddComponent<KillAreaOutputEvent>();
                break;
            case OutputeventEnum.objectActive:
                oevent = gameObject.AddComponent<ObjectActiveEvent>();
                break;
            case OutputeventEnum.ObjectDeactive:
                oevent = gameObject.AddComponent<ObjectDeactiveEvent>();
                break;
            case OutputeventEnum.PlayerBlowOut:
                oevent = gameObject.AddComponent<PlayerBlowOutputEvent>();
                break;
            case OutputeventEnum.PlayerKill:
                oevent = gameObject.AddComponent<PlayerKillOutputEvent>();
                break;
            case OutputeventEnum.PlayerReturn:
                oevent = gameObject.AddComponent<PlayerReturnOutputEvent>();
                break;
            case OutputeventEnum.PlayerSpawn:
                oevent = gameObject.AddComponent<PlayerSpawnOutputEvent>();

                break;
            case OutputeventEnum.ScenePass:
                oevent = gameObject.AddComponent<ScenePassOutputEvent>();
                break;
            case OutputeventEnum.SoundPlay:
                oevent = gameObject.AddComponent<SoundPlayEvent>();
                break;
            case OutputeventEnum.TimerStart:
                oevent = gameObject.AddComponent<Timer_Start_Event>();
                break;
            case OutputeventEnum.Teleport:
                oevent = gameObject.AddComponent<TeleportOutputEvent>();
                break;
            case OutputeventEnum.TransformChange:
                oevent = gameObject.AddComponent<TransformChangeOuputEvent>();
                break;
            default:
                oevent = null;
                break;
        }
        if (oevent != null)
        {
            if(outputevents==null)
                outputevents = new List<OutputEvent>();
            if (outputeventname != null)
            {
                oevent.eventname = outputeventname;
            }
            else
            {
                oevent.eventname="outputevent"+outputevents.Count;
            }
            outputevents.Add(oevent);
        }
    }
    public void CreateInputEvent()
    {
        InputEvent Ievent=null;
        switch (inputenum)
        {
            case inputeventenum.characterhit:
                Ievent = gameObject.AddComponent<characterhitevent>();
                break;
            case inputeventenum.characterskillinput:
                Ievent = gameObject.AddComponent<CharacterSkillInputEvent>();
                break;
            case inputeventenum.collision:
                Ievent = gameObject.AddComponent<CollisionEvent>();
                break;
            case inputeventenum.Trigger:
                Ievent = gameObject.AddComponent<CollisionisTriggerEvent>();
                break;
            case inputeventenum.objectdelete:
                Ievent=gameObject.AddComponent<ObjectDeleteEvent>();
                break;
            case inputeventenum.objectTrigger:
                Ievent = gameObject.AddComponent<ObjectTriggerInputEvent>();
                break;
            case inputeventenum.ObjectCollision:
                Ievent = gameObject.AddComponent<objectcollisionevent>();
                break;
            case inputeventenum.TImerPlaying:
                Ievent = gameObject.AddComponent<TimerPlayingInputEvent>();
                break;
            case inputeventenum.TImerEnd:
                Ievent=gameObject.AddComponent<TimerEndInputEvent>();
                break;
            case inputeventenum.enemySpawn:
                Ievent = gameObject.AddComponent<EnemySpawnInputEvent>();
                break;
            case inputeventenum.enemyKill:
                Ievent = gameObject.AddComponent<EnemyKillInputEvent>();
                break;
            case inputeventenum.Die:
                Ievent = gameObject.AddComponent<DieInputEvent>();
                break;
            case inputeventenum.Control:
                Ievent = gameObject.AddComponent<ControlInputEvent>();
                break;
            case inputeventenum.Moribund:
                Ievent = gameObject.AddComponent<MoribundInputEvent>();
                break;
            case inputeventenum.interaction:
                Ievent = gameObject.AddComponent<InteractionInputEvent>();
                break;
            case inputeventenum.signalsenderinput:
                Ievent = gameObject.AddComponent<signalactiveinput>();
                break;
            default:
                Ievent = null;
                break;
        }
        if (Ievent != null)
        {
            if (inputevents == null)
                inputevents = new List<InputEvent>();
            if (inputeventname != "")
                Ievent.eventname = inputeventname;
            else
                Ievent.eventname = "inputevent" + inputevents.Count;
            inputeventname = "";
         
            inputevents.Add(Ievent);
        }
    }
    IEnumerator OREventCorutine()
    {
        evenactive = true;
        while (!checker)
        {
            checker = false;
            foreach (var a in inputevents)
            {
                checker |= a.input();
            }
            if (EventCheckDelay <= 0)
                yield return null;
            else
                yield return new WaitForSeconds(EventCheckDelay);
        }
        foreach (var a in outputevents)
        {
            a.output();
        }
        eventcomplete = true;
        evenactive = false;
        if (loop)
        {
            yield return new WaitForSeconds(EventDisabletimer);
            eventcomplete = false;
            foreach (var a in inputevents)
            {
                a.initialize();
            }
            StartCoroutine(OREventCorutine());
        }
    }
    IEnumerator EventCorutine()
    {
        evenactive = true;
        while (!checker)
        {
            checker = true;
            foreach(var a in inputevents)
            {
                checker &= a.input();
            }
            if(EventCheckDelay<=0)
            yield return null;
            else
                yield return new WaitForSeconds(EventCheckDelay);
        }
        foreach(var a in outputevents)
        {
            a.output();
        }
        eventcomplete = true;
        evenactive = false;
        if (loop)
        {
            yield return new WaitForSeconds(EventDisabletimer);
            eventcomplete = false;
            foreach (var a in inputevents)
            {
                a.initialize();
            }
            StartCoroutine(EventCorutine());
        }
    }
    public void stopevent()
    {
        StopAllCoroutines();
        evenactive = false;
    }
    public void startevent()
    {
        
        if (!or)
            StartCoroutine(EventCorutine());
        else
            StartCoroutine(OREventCorutine());
    }
    private void Start()
    {
        if(startonawake)
        startevent();
    }
}
