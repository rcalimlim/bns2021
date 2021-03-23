using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableController : MonoBehaviour
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;
    [SerializeField] private InventoryItem[] inventoryItems;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private bool autoEquipItem = false;

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

    private void AddItemsToInventory()
    {
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            playerInventory.AddItem(inventoryItem.Item, inventoryItem.Qty);
            if (autoEquipItem == true)
            {
                // playerInventory.Equip();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (IsEnabled() && other.CompareTag("Player") && !other.isTrigger)
        {
            if (dialog.Lines.Count > 0)
            {
                StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
            }
            AddItemsToInventory();
            PlayerDataManager.Instance.SetTriggerFlag(activatesTriggerFlagName, true);
        }
    }
}
