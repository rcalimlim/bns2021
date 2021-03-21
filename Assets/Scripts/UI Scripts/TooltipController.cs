using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipController : MonoBehaviour
{   
    public GameObject tooltip;
    ItemUiController toolTipUI;

    private void Awake() {
        toolTipUI = tooltip.GetComponent<ItemUiController>();
    }

    public void Show(InventorySlot slot)
    { 
        toolTipUI.InventorySlot = slot;
        tooltip.SetActive(true);
    }

    public void Hide()
    {
        toolTipUI.resetUI();
        tooltip.SetActive(false);
    }
}
