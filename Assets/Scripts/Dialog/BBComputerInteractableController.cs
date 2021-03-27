using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BBComputerInteractableController : MonoBehaviour, Interactable
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;
    [SerializeField] private GameObject player;

    [SerializeField] private int stressThreshold;
    [SerializeField] private Dialog dialogForHighStress;
    [SerializeField] private Dialog dialogForLowStress;

    private IEnumerator InvokeActionsSync()
    {
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialogForLowStress));

        // wait for dialog to finish
        while (GameController.Instance.State == GameState.Dialog)
        {
            yield return null;
        }

        // setup info in PlayerDataManager
        PlayerDataManager.Instance.TrackSceneChange(
            player.gameObject.transform.position,
            SceneManager.GetActiveScene().name,
            "Battle"
        );
        // call scene switcher
        SceneManager.LoadScene("Battle");
    }

    public void Interact()
    {
        if (PlayerDataManager.Instance.CurrentStress > stressThreshold)
        {
            Debug.Log(PlayerDataManager.Instance.CurrentStress);
            StartCoroutine(DialogManager.Instance.ShowDialog(dialogForHighStress));
        }
        else
        {
            StartCoroutine(InvokeActionsSync());
        }
    }
}
