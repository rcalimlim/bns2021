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
    [SerializeField] private Vector3 loadPos;

    /*
     Player Stats
    */
    [SerializeField]
    private int minStress = 0;
    [SerializeField]
    private int maxStress = 100;
    [SerializeField]
    private int currentStress = 60;
    
    /* 
     Game Stats
    */
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
        get { return spawnDoor; }
        set { spawnDoor = value; }
    }

    public string PrevScene {
        get { return prevScene; }
        set { prevScene = value; }
    }

    public string CurrScene {
        get { return currScene; }
        set { currScene = value; }
    }

    public Vector3 LoadAtPos {
        get { return loadPos;}
        set {loadPos = value;}
    }

    // Call when changing scenes to keep track of player
    public void TrackSceneChange(string doorTag, string currentScene, string nextScene)
    {
        spawnDoor = doorTag;
        prevScene = currentScene;
        currScene = nextScene;
        loadPos = Vector3.zero; 
    }

    public void TrackSceneChange(Vector3 loadAtPos, string currentScene, string nextScene)
    {
        loadPos = loadAtPos;
        spawnDoor = "";
        prevScene = currentScene;
        currScene = nextScene;
    }

    public void UpdateStats(int stress, int score, int turns)
    {
        currentStress = stress * 10;
        this.score = score;
        this.turns = turns; 
    }
}
