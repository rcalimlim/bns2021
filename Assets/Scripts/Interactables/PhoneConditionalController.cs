using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhoneConditionalController : MonoBehaviour, Interactable
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;

    [SerializeField] private Dialog dialog;
    [SerializeField] private Dialog onAcceptDialogSuccess;
    [SerializeField] private Dialog onDecline;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject videoCanvas;

    private bool IsEnabled()
    {
        if (debugEnabled)
        {
            return true;
        }

        bool isDisabled = false;
        if (disableAfterTriggerFlag != "")
        {
            isDisabled = PlayerDataManager.Instance.GetTriggerFlag(disableAfterTriggerFlag);
            if (isDisabled)
            {
                return false;
            }
        }

        if (enableAfterTriggerFlag == "")
        {
            return true;
        }

        return PlayerDataManager.Instance.GetTriggerFlag(enableAfterTriggerFlag);
    }

    private void ActivateTriggerFlag()
    {
        if (activatesTriggerFlagName != "")
        {
            PlayerDataManager.Instance.SetTriggerFlag(activatesTriggerFlagName, true);
        }
    }

    private IEnumerator OnAcceptSync()
    {
        DialogManager.Instance.CloseDialog();

        // logic for taking the ornament
        if (onAcceptDialogSuccess.Lines.Count > 0)
        {
            yield return StartCoroutine(DialogManager.Instance.ShowDialog(onAcceptDialogSuccess));
        }

        // play video
        // yield return StartCoroutine(videoCanvas.GetComponent<CringeVideoController>().PlayVideo());

        
        // setup info in PlayerDataManager
        PlayerDataManager.Instance.TrackSceneChange(
            player.gameObject.transform.position,
            SceneManager.GetActiveScene().name,
            "Battle"
        );
        // call scene switcher
        SceneManager.LoadScene("Battle");

        ActivateTriggerFlag();
    }

    private void OnAccept()
    {
        StartCoroutine(OnAcceptSync());
    }

    private void OnDecline()
    {
        DialogManager.Instance.CloseDialog();
    }

    private void SetupDialogCallbacks()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        DialogButtonClickHandler dbch = canvas?.GetComponent<DialogButtonClickHandler>(); 
        dbch.onAccept = OnAccept;
        dbch.onDecline = OnDecline;
    }

    private IEnumerator InvokeActionsSync()
    {
        SetupDialogCallbacks();
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog, true));
    }

    public void Interact()
    {
        if (IsEnabled())
        {
            StartCoroutine(InvokeActionsSync());
        }
    }
}
