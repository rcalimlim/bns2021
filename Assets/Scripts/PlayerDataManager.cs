using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    // singleton
    private static PlayerDataManager instance;
    public static PlayerDataManager Instance { get { return instance; } }

    // Player scene spawning data
    [SerializeField] private string spawnDoor;
    [SerializeField] private string prevScene;
    [SerializeField] private string currScene;
    [SerializeField] private Vector3 loadPos;

    /*
     Player Stats
    */
    [SerializeField] private int minStress = 0;
    [SerializeField] private int maxStress = 100;
    [SerializeField] private int currentStress = 60;
    
    /* 
     Game Stats
    */
    [SerializeField] private int score;
    [SerializeField] private int turns;

    // global game flags because this is a hackathon baby
    private Dictionary<string, bool> triggerFlags = new Dictionary<string, bool>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }

        DontDestroyOnLoad (gameObject);
    }

    public string SpawnDoor { get { return spawnDoor; } }

    public string PrevScene { get { return prevScene; } }

    public string CurrScene { get { return currScene; } }

    public Vector3 LoadAtPos { get { return loadPos;} }

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

    public bool GetTriggerFlag(string flagName)
    {
        bool val;
        triggerFlags.TryGetValue(flagName, out val);
        // this is a hack, because return false means it wasn't in the hash not
        // that the flag is actually false in this case, not existing is equivalent
        // to having a false val
        return val;
    }
    public void SetTriggerFlag(string flagName, bool val)
    {
        triggerFlags[flagName] = val;
    }
}
