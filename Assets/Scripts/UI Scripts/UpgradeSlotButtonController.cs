using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeSlotButtonController : MonoBehaviour, IPointerClickHandler
{
    InventorySlot inventorySlot;
    
    [SerializeField] Inventory inventory;
    [SerializeField] Item duperJunk;
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

    public void OnUpgradeAccept()
    {
        inventory.RemoveItem(duperJunk, -1);
        ((EquipableItem)inventorySlot.Item).Upgrade();
        CloseDialogs();
    }

    public void OnUpgradeDecline()
    {
        CloseDialogs();
    }

    private void CloseDialogs()
    {
        GameObject.Find("CantUpgradeUI").transform.localScale = Vector3.zero;
        GameObject.Find("UpgradeUI").transform.localScale = Vector3.zero;
    }

    private void CantUpgrade(string text)
    {
        CloseDialogs();
        Debug.Log(text);
        GameObject.Find("CantUpgradeUIText").GetComponent<Text>().text = text;
        GameObject.Find("CantUpgradeUI").transform.localScale = Vector3.one;
    }

    private void CanUpgrade()
    {
        CloseDialogs();
        EquipableItem item = (EquipableItem)inventorySlot.Item;
        GameObject.Find("UpgradeUIWeaponName").GetComponent<Text>().text = item.name;
        GameObject.Find("UpgradeUIWeaponLevel").GetComponent<Text>().text = "Rating: " + item.Rating + " ➞ " + item.GetUpgradeRating();
        GameObject.Find("UpgradeUIAccept").GetComponent<Button>().onClick.AddListener(OnUpgradeAccept);
        GameObject.Find("UpgradeUIDecline").GetComponent<Button>().onClick.AddListener(OnUpgradeDecline);
        GameObject.Find("UpgradeUI").transform.localScale = Vector3.one;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventorySlot.Item is EquipableItem)
        {
            EquipableItem item = (EquipableItem)inventorySlot.Item;

            if (item.CanBeUpgraded() == false)
            {
                CantUpgrade("This item can't be upgraded.");
            }
            else if (inventory.GetQuantity(duperJunk) <= 0)
            {
                CantUpgrade("You don't have enough Duper Junks");
            }
            else if (item.CanBeUpgraded() == true)
            {
                CanUpgrade();
            }
        }
    }
    
}
