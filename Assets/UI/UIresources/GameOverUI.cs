using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : UIInteract
{
    public List<GameObject> uiList = new List<GameObject>();

    public List<Image> buttonList = new List<Image>();    

    public Sprite deactiveButton, activeButton;
   
    [Range(0,3)] public float activeTimer;

    public bool onHandle;
    int index, beforeIndex;

    public Animator animator;

    private void Awake()
    {
        InitDeactive();
    }

    public void InitDeactive()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < uiList.Count; i++)
        {
            uiList[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(activeGameOverUI());
    }

    private void OnDisable()
    {
        onHandle = false;

        buttonList[index].sprite = deactiveButton;
        fontList[index].color = deactiveFontColor;
    }

    IEnumerator activeGameOverUI()
    {
        index = 0;
        yield return new WaitForSecondsRealtime(0.1f);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ActiveGameOver"))
        {
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }

            yield return new WaitForSecondsRealtime(activeTimer);
            
            buttonList[index].sprite = activeButton;
            fontList[index].color = activeFontColor;

            onHandle = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (onHandle)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (index < buttonList.Count - 1)
                {
                    beforeIndex = index;
                    index++;
                    UpdateUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (index > 0)
                {
                    beforeIndex = index;
                    index--;
                    UpdateUI();
                }
            }

            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                switch (index)
                {
                    case 0:
                        Time.timeScale = 1;
                        GameManager.instance.LoadingSceneWithKariEffect(GameManager.instance.LoadLastestStage());
                        break;
                    case 1:                        
                        break;
                }
            }
        }
    }

    public void UpdateUI()
    {
        if (onHandle)
        {
            buttonList[index].sprite = activeButton;
            buttonList[beforeIndex].sprite = deactiveButton;
            fontList[index].color = activeFontColor;
            fontList[beforeIndex].color = deactiveFontColor;
        }
    }
}
