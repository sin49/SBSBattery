using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Experimental.Rendering;
using Unity.VisualScripting;

public class TestPauseUI : MonoBehaviour
{
    public RectTransform imageGroup;
    public GameObject pauseUi;
    public GameObject settingUI;

    public Animator uiAnimator;
    public Animator screwAnimator;

    public GameObject screw;
    public List<GameObject> UiButton = new List<GameObject>();
    int index, beforeIndex;

    public Color originColor;
    public Color choiceColor;

    Vector3 choiceScale = new(1.1f, 1.1f, 1.1f);
    Vector3 originScale = new(1, 1, 1);

    public bool uiGroupActive;
    public bool settingActive; 
    public bool reCheckActive;

    private void Awake()
    {
        imageGroup.gameObject.SetActive(false);
        screw.SetActive(false);
    }

    private void Start()
    {
        uiAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        screwAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (!imageGroup.gameObject.activeSelf)
            {
                imageGroup.gameObject.SetActive(true);         
                index = 0;
                ShowPauseUI();
            }
            else
            {
                if (uiGroupActive)
                {
                    uiGroupActive = false;
                    imageGroup.gameObject.SetActive(false);
                }
            }
        }

        if (uiGroupActive && !settingActive)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                beforeIndex = index;
                if (index <= 0)
                    index = UiButton.Count - 1;
                else
                    index--;

                UpdateUI();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                beforeIndex = index;
                if (index >= UiButton.Count - 1)
                    index = 0;
                else
                    index++;

                UpdateUI();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                switch (index)
                {
                    case 0: // 계속하기
                        break;
                    case 1: // 체크포인트로
                        break;
                    case 2: // 설정
                        ShowSettingUI();
                        break;
                    case 3: // 타이틀로
                        break;
                    case 4: // 종료
                        break;
                    default:
                        Debug.Log("out of range");
                        break;
                }
            }
        }
    }

    public void ShowPauseUI()
    {
        uiAnimator.Play("ShowPauseUI");
        StartCoroutine(StartUiGroup());
    }

    IEnumerator StartUiGroup()
    {
        yield return new WaitForSeconds(0.1f);

        Debug.Log(uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowPauseUI"));
        if (uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShowPauseUI"))
        {
            while (uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
            uiGroupActive = true;
            UpdateUI();
            screw.gameObject.SetActive(true);
        }
    }

    public void PauseBackSetting()
    {
        uiAnimator.Play("PauseBackSetting");
        StartCoroutine(PBS());
    }

    IEnumerator PBS()
    {
        yield return new WaitForSeconds(0.1f);
        if (uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseBackSetting"))
        {
            Debug.Log("설정에서 일시정지 UI로");
            while (uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
            settingActive = false;
        }
    }


    public void UpdateUI()
    {
        /*if (settingActive)
            return;*/
        screw.transform.SetParent(UiButton[index].transform.GetChild(1));
        screw.transform.position = UiButton[index].transform.GetChild(1).position;
        screwAnimator.Play("ScrewRotate", 0, 0f);

        UiButton[beforeIndex].GetComponent<Image>().color = originColor;
        UiButton[beforeIndex].transform.localScale = originScale;
        screw.transform.localScale = originScale;

        UiButton[index].GetComponent<Image>().color = choiceColor;
        UiButton[index].transform.localScale = choiceScale;
        screw.transform.localScale = choiceScale;
    }

    public void ShowSettingUI()
    {
        settingActive = true;
        screw.SetActive(false);
        uiAnimator.Play("PauseChangeSetting");
        StartCoroutine(StartSettingUi());
    }



    IEnumerator StartSettingUi()
    {
        yield return new WaitForSeconds(0.1f);

        if (uiAnimator.GetCurrentAnimatorStateInfo(0).IsName("PauseChangeSetting"))
        {
            while (uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                Debug.Log(uiAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                yield return null;
            }

            settingUI.SetActive(true);
            pauseUi.SetActive(false);
        }
    }
}
