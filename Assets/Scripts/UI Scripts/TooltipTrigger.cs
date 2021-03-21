using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventorySlot inventorySlot;
    public GameObject TooltipCanvas;
    TooltipController tooltip;
    void Start()
    {
        tooltip = TooltipCanvas.GetComponent<TooltipController>();
        tooltip.Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Show(inventorySlot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Hide();
    }
}
