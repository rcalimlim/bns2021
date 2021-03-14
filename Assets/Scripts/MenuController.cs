using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject credits, closeCreditsButton, showCreditsButton, newGameButton;
    public Text muteText;


    private bool muted = false;
     
    
    private void Awake() {
        if(AudioListener.volume == 0)
        {
            muted = true;
            muteText.text = "Unmute";
        }
    }

    private void Start() {
        // Default the menu option
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }
    private void Update() {
        
    }

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
        SceneManager.LoadScene("Bryce Bedroom");
    }

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
