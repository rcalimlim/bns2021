using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrnamentTradingController : MonoBehaviour, Interactable
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;
    [SerializeField] private InventoryItem[] inventoryItems;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Item itemToRemoveOnAccept;
    [SerializeField] private Item itemToGiveOnAccept;

    [SerializeField] private Dialog dialog;
    [SerializeField] private Dialog onAcceptDialogSuccess;
    [SerializeField] private Dialog onAcceptDialogFailure;
    [SerializeField] private Dialog onDecline;

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

    private IEnumerator OnAcceptSync()
    {
        DialogManager.Instance.CloseDialog();


        // logic for taking the ornament
        if (playerInventory.GetQuantity(itemToRemoveOnAccept) > 0)
        {
            playerInventory.RemoveItem(itemToRemoveOnAccept, -1);
            playerInventory.AddItem(itemToGiveOnAccept, 1);
            yield return StartCoroutine(DialogManager.Instance.ShowDialog(onAcceptDialogSuccess));
        }
        else
        {
            yield return StartCoroutine(DialogManager.Instance.ShowDialog(onAcceptDialogFailure));
        }
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
        if (dbch != null)
        {
            dbch.onAccept = OnAccept;
            dbch.onDecline = OnDecline;
        }
    }

    private IEnumerator InvokeActionsSync()
    {
        SetupDialogCallbacks();
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog, true));
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
