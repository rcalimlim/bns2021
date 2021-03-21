using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBComputerInteractableController : MonoBehaviour, Interactable
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;

    [SerializeField] private Dialog dialogForHighStress;
    [SerializeField] private Dialog dialogForLowStress;

    public void Interact()
    {
        if (PlayerDataManager.Instance.CurrentStress >= (PlayerDataManager.Instance.MaxStress / 2))
        {
            Debug.Log(PlayerDataManager.Instance.CurrentStress);
            StartCoroutine(DialogManager.Instance.ShowDialog(dialogForHighStress));
        }
        else
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(dialogForLowStress));
        }
    }
}
