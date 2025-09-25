using System.Collections;
using UnityEngine;
using TMPro;
public class RespawnSignal : MonoBehaviour
{
    public static RespawnSignal Instance;

    [Header("재생성 알림UI")]
    public GameObject spawnUISignal;
    public TextMeshProUGUI platformTMP;

    private void Awake()
    {
        Instance = this;
        spawnUISignal.SetActive(false);
    }

    public void SignalStart()
    {
        if (cor != null)
        {
            StopCoroutine(cor);
            cor = null;
        }

        cor = Respawn();
        StartCoroutine(cor);
    }

    IEnumerator cor;

    IEnumerator Respawn()
    {
        spawnUISignal.SetActive(true);
        ChangeLanguage();

        yield return new WaitForSecondsRealtime(2f);

        spawnUISignal.SetActive(false);
    }

    public void ChangeLanguage()
    {
        if (LanguageManager.instance.isKor)
        {
            platformTMP.text = "플랫폼 오브젝트가 재생성되었습니다";
        }
        else
        {
            platformTMP.text = "The platform has respawned";
        }
    }
}
