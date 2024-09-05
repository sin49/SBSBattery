using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScreen : MonoBehaviour
{
    public List<TItleText> titletexts;
    public int index;
    public TextMeshProUGUI ResetText;
    public ButtonSoundEffectPlayer ButtionSoundEffectPlayer_;
    
    public void StartNewGame()
    {
        GameManager.instance.DeleteSaveSetting();
        GameManager.instance.LoadingSceneWithKariEffect("Alpha_1-1-A");
   
    }
    public void ContinueGame()
    {

        Debug.Log("작동 시도"+ GameManager.instance.LoadLastestStage());

        GameManager.instance.LoadingSceneWithKariEffect(GameManager.instance.LoadLastestStage());

      
    }
    public void ResetData()
    {
        GameManager.instance.DeleteSaveSetting();
        ResetText.gameObject.SetActive(true);
    }
    public void removeEvents()
    {
        //foreach (TItleText t in titletexts)
        //{
        //    t.removeevent();
        //}
    }
    public void handletitle()
    {
        int LastIndex;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            LastIndex = index;
            index++;
            ButtionSoundEffectPlayer_.PlaySelectAudio();
            if (!PlayerPrefs.HasKey("LastestStageName") && index == 1)
            {
                index++;
            }
            if (index >= titletexts.Count)
                index = titletexts.Count - 1;
            changehub(LastIndex, index);
          
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LastIndex = index;
            index--;
            ButtionSoundEffectPlayer_.PlaySelectAudio();
            if (!PlayerPrefs.HasKey("LastestStageName") && index == 1)
            {
                index--;
            }
            if (index < 0)
                index = 0;
            changehub(LastIndex, index);
          
        }
        else if (Input.GetKeyDown(KeyCode.X)|| Input.GetKeyDown(KeyCode.C))
        {
            ButtionSoundEffectPlayer_.PlayActiveAudio();
            titletexts[index].ButtonActive();
        }

    }
    public void changehub(int before,int after)
    {
        titletexts[before].DeActiveImageHub();
        titletexts[after].ActiveImageHub();
    }
    public void quitgame()
    {
        removeEvents();
        Application.Quit();
    }
    public void InitText()
    {
        if (PlayerPrefs.HasKey("LastestStageName"))
            index = 1;
        else
        index = 0;
        titletexts[index].ActiveImageHub();
        titletexts[0].ButtonEffect += StartNewGame;
        titletexts[1].ButtonEffect += ContinueGame;
        titletexts[2].ButtonEffect += ResetData;
        titletexts[titletexts.Count - 1].ButtonEffect += quitgame;
        ResetText.gameObject.SetActive(false);
    }
    private void Awake()
    {
        ButtionSoundEffectPlayer_ = gameObject.GetComponent<ButtonSoundEffectPlayer>();
        //var a = GameManager.instance.loadingscenename;
        //if (a[0] != 'A')
        //{
        //    GameManager.instance.DeleteSaveSetting();
        //}
    }
    private void Start()
    {
        InitText();
        
       
    }
    // Update is called once per frame
    void Update()
    {
        handletitle();
    }
}
