using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour, Interactable
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;
    [SerializeField] private Dialog dialog;
    [SerializeField] private Menu upgradeMenu;

    private PlayerInput controls;

    public event Action OnOpenUpgradeMenu;
    public event Action OnCloseUpgradeMenu;

    private void Awake()
    {
        controls = new PlayerInput();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

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

    private void OnAccept()
    {
        DialogManager.Instance.CloseDialog();
        OnOpenUpgradeMenu?.Invoke();
        upgradeMenu.OpenMenu(controls);
    }

    private void OnDecline()
    {
        DialogManager.Instance.CloseDialog();
    }
    private void SetupDialogCallbacks()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        DialogButtonClickHandler dbch = canvas?.GetComponent<DialogButtonClickHandler>(); 
        if (dbch != null)
        {
            dbch.onAccept = OnAccept;
            dbch.onDecline = OnDecline;
        }
    }

    private IEnumerator InvokeActionsSync()
    {
        yield return new WaitForEndOfFrame();

        SetupDialogCallbacks();
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, true));
    }


    public void Interact()
    {
        if (IsEnabled())
        {
            StartCoroutine(InvokeActionsSync());
        }
    }
}
