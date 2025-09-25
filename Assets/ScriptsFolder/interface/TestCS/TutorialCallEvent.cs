using UnityEngine;

public class TutorialCallEvent : MonoBehaviour
{
    public InteractTutorial tutorial;

    private void OnDisable()
    {
        if (tutorial != null && !GameManager.instance.loading)
            tutorial.CallByTutoEvent();
    }
}
