using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject credits, closeCreditsButton, showCreditsButton, initialSelected;
    public Text muteText;

    [SerializeField] private AudioClip song;
    private SoundManager soundManager;
    private bool muted = false;
     
    
    private void Awake() {
        if(AudioListener.volume == 0 && muteText)
        {
            muted = true;
            muteText.text = "Unmute";
        }
    }

    private void Start() {
        soundManager = SoundManager.Instance;
        // Default the menu option
        setInitial();

        if(gameObject.name == "StartMenu")
            soundManager.PlayMusic(song);        
    }
    public void setInitial()
    {   
        switch(gameObject.name)
        {
            case "StartMenu":
            case "GameOver":
            case "GameOver(Clone)":
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(initialSelected); break;
            default:
                EventSystem.current.SetSelectedGameObject(null);
                break;
        }
    }
    private void Update() {
        
    }

    // Various Menu Functions
    public void Mute()
    {
        if(!muted)
        {
            AudioListener.volume = 0;
            muteText.text = "Unmute";
        }
            
        else
        {
            AudioListener.volume = 1;
            muteText.text = "Mute";
        }
            
        muted = !muted;
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("BNS2019");
    }

    public void StartMenu() 
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void PopulateTextElement(string path, string value)
    {
        Text itemText = transform.Find(path)?.gameObject.GetComponent<Text>();
        if(itemText)
        {
            itemText.text = value;
        }
            
    }


    // Start Menu Functions
    public void OpenCredits()
    {
        credits.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(closeCreditsButton);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(showCreditsButton);
    }

    public void QuiteGame()
    {
        // Will not work in editor
        print("Quitting");
        Application.Quit();

    }
}
