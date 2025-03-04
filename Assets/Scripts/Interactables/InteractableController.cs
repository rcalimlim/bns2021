using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour, Interactable
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;
    [SerializeField] private InventoryItem[] inventoryItems;
    [SerializeField] private Inventory playerInventory;

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

    private void ActivateTriggerFlag()
    {
        if (activatesTriggerFlagName != "")
        {
            PlayerDataManager.Instance.SetTriggerFlag(activatesTriggerFlagName, true);
        }
    }
    private void AddItemsToInventory()
    {
        if (inventoryItems != null && inventoryItems.Length > 0)
        {
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                playerInventory.AddItem(inventoryItem.Item, inventoryItem.Qty);
            }
        }
    }

    private IEnumerator InvokeActionsSync()
    {
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog, false));
        AddItemsToInventory();
        ActivateTriggerFlag();
    }

    public void Interact()
    {
        if (IsEnabled())
        {
            StartCoroutine(InvokeActionsSync());
        }
    }
}
