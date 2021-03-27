using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHUnsealedSwordController : MonoBehaviour
{
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private string enableAfterTriggerFlag;
    [SerializeField] private string disableAfterTriggerFlag;
    [SerializeField] private string activatesTriggerFlagName;
    [SerializeField] private Item sealedSword;
    [SerializeField] private Item zxvSword;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private bool autoEquipItems = false;

    [SerializeField] private Dialog dialogNoSword;
    [SerializeField] private Dialog dialogYesSword;
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
        playerInventory.RemoveItem(sealedSword, -1);
        playerInventory.AddItem(zxvSword, 1);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (IsEnabled() && other.CompareTag("Player") && !other.isTrigger)
        {

            // path with sealed sword
            if (playerInventory.GetQuantity(sealedSword) > 0)
            {
                if (dialogYesSword.Lines.Count > 0)
                {
                    StartCoroutine(DialogManager.Instance.ShowDialog(dialogYesSword));
                }
                AddItemsToInventory();
                PlayerDataManager.Instance.SetTriggerFlag(activatesTriggerFlagName, true);
            }
            else
            {
                if (dialogNoSword.Lines.Count > 0)
                {
                    StartCoroutine(DialogManager.Instance.ShowDialog(dialogNoSword));
                }
            }

        }
    }
}
