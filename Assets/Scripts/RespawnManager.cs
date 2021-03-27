using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    // singleton
    private static RespawnManager instance;
    public static RespawnManager Instance { get { return instance; } }

    [SerializeField] private Dialog dialogOnDeath;
    private Animator sceneFade;
    private GameObject player;
    private GameObject respawnSpot;

    public event Action OnDeath;
    public event Action OnRevive;

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

    public IEnumerator ActivateRespawnSequence()
    {
        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialogOnDeath));

        while (GameController.Instance.State != GameState.FreeRoam)
        {
            yield return null;
        }

        PlayerDataManager.Instance.HasDied = true;
        PlayerDataManager.Instance.TrackSceneChange("", SceneManager.GetActiveScene().name, PlayerDataManager.Instance.PrevScene);
        SceneManager.LoadScene("BryceBedroom");
    }

    public IEnumerator ActivateSleepSequence()
    {
        yield return new WaitForEndOfFrame();
        while (GameController.Instance.State != GameState.FreeRoam)
        {
            yield return null;
        }

        PlayerDataManager.Instance.HasDied = true;
        SceneManager.LoadScene("BryceBedroom");
    }

    private void HandleUpdate()
    {
        // nothing necessary
    }

}
