using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotButtonController : MonoBehaviour, IPointerClickHandler
{
    InventorySlot inventorySlot;
    
    [SerializeField]
    Inventory inventory;
    Text label;

    public InventorySlot InventorySlot
    {
        set => inventorySlot = value;
    }

    private void Start() 
    {
        label = GetComponentInChildren<Text>();
    }

    void Update() {
        if(inventorySlot != null)
        {
            label.text = inventorySlot.Item.name;
        }

        if(inventorySlot == null || !(inventorySlot.Item is EquipableItem))
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(inventorySlot != null && inventorySlot.Item is EquipableItem)
            inventory.Equip((EquipableItem)inventorySlot.Item);
    }
    
}
