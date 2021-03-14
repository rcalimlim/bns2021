using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance = null;
    [SerializeField]
    private string spawnDoor;
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

    public string SpawnDoor { get; set; }
}
