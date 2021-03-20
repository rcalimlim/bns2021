using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableController : MonoBehaviour
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;

    [SerializeField] private Dialog dialog;
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (IsEnabled() && other.CompareTag("Player") && !other.isTrigger)
        {
            if (dialog.Lines.Count > 0)
            {
                StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
            }
            PlayerDataManager.Instance.SetTriggerFlag(activatesTriggerFlagName, true);
        }
    }
}
