using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance = null;
    [SerializeField]
    private string spawnDoor;
    [SerializeField]
    private string prevScene;
    [SerializeField]
    private string currScene;
    [SerializeField]
    private int minStress;
    [SerializeField]
    private int maxStress;
    [SerializeField]
    private int currentStress;
    [SerializeField]
    private int score;
    [SerializeField]
    private int turns;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad (gameObject);
    }

    public string SpawnDoor {
        get { return name; }
        set { name = value; }
    }

    public string PrevScene {
        get { return prevScene; }
        set { prevScene = value; }
    }

    public string CurrScene {
        get { return currScene; }
        set { currScene = value; }
    }

    // Call when changing scenes to keep track of player
    public void TrackSceneChange(string doorTag, string currentScene, string nextScene)
    {
        spawnDoor = doorTag;
        prevScene = currentScene;
        currScene = nextScene; 
    }
}
