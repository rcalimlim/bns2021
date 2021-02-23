using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text muteText;
    public GameObject credits;

    bool muted = false;

    void Start() {
        if(AudioListener.volume == 0)
        {
            muted = true;
            muteText.text = "Unmute";
        }  
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("bns2019");
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

    public void QuitGame()
    {
        // Will not work in editor
        print("Quitting");
        Application.Quit();
    }

    public void ShowCredits()
    {
        credits.SetActive(true);
    }

    public void HideCredits()
    {
        credits.SetActive(false);
    }
}
