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
    [SerializeField] private Dialog dialogOnRevive;
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

        // do some fade to black
        SceneManager.LoadScene("BryceBedroom");

        while (SceneManager.GetActiveScene().name != "BryceBedroom")
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        PlayerDataManager.Instance.ResetStress();
        OnRevive?.Invoke();
        // do some fade to scene

        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialogOnRevive));
    }

    public IEnumerator ActivateSleepSequence()
    {
        yield return new WaitForEndOfFrame();
        while (GameController.Instance.State != GameState.FreeRoam)
        {
            yield return null;
        }

        // do some fade to black

        PlayerDataManager.Instance.ResetStress();
        OnRevive?.Invoke();
        // do some fade to scene

        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialogOnRevive));
    }

    private void HandleUpdate()
    {
        // nothing necessary
    }

}
