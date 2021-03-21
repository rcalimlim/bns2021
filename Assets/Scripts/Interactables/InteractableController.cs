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
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            playerInventory.AddItem(inventoryItem.Item, inventoryItem.Qty);
        }
    }

    public void Interact()
    {
        if (IsEnabled())
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
            AddItemsToInventory();
            ActivateTriggerFlag();
        }
    }
}
