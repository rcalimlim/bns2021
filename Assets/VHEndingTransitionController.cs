using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VHEndingTransitionController : MonoBehaviour
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;
    [SerializeField] private InventoryItem[] inventoryItems;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private bool autoEquipItems = false;
    [SerializeField] private Dialog dialog;
    [SerializeField] private string endingSceneName;

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
            if (autoEquipItems == true)
            {
                Debug.Log("autoequip true");
                foreach (InventoryItem iItem in inventoryItems)
                {
                    playerInventory.Equip((EquipableItem)iItem.Item);
                }
            }
        }
    }

    private IEnumerator InvokeActionsSync()
    {
        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog));

        while (GameController.Instance.State == GameState.Dialog)
        {
            yield return null;
        }

        PlayerDataManager.Instance.SetTriggerFlag(activatesTriggerFlagName, true);

        // go to ending scene
        SceneManager.LoadScene(endingSceneName);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (IsEnabled() && other.CompareTag("Player") && !other.isTrigger)
        {
            StartCoroutine(InvokeActionsSync());
        }
    }
}
