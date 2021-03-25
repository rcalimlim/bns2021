using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObjectBase
{
    [SerializeField] List<InventorySlot> itemBag = new List<InventorySlot>();
    [SerializeField] Weapon equipedWeapon;
    [SerializeField] Armor equipedArmor;

    public Item EquippedWeapon { get { return equipedWeapon; } }
    public Item EquippedArmor { get { return equipedArmor; } }

    public void AddItem(Item _item, int _amount)
    {
        for(int i = 0; i < itemBag.Count; i++)
        {
            if(itemBag[i].Item == _item)
            {
                itemBag[i].ModifyAmount(_amount);
                return;
            }
        }

        itemBag.Add(new InventorySlot(_item, _amount));
    }

    public void RemoveItem(Item _item, int _amount)
    {
         for(int i = 0; i < itemBag.Count; i++)
        {
            if(itemBag[i].Item == _item)
            {
                itemBag[i].ModifyAmount(_amount);
                
                if(itemBag[i].Amount <= 0)
                {
                    itemBag.Remove(itemBag[i]);
                }
                return;
            }
        }
    }

    public int GetQuantity(Item item)
    {
        int amt = 0;
        InventorySlot invSlot = itemBag.Find(slot => slot.Item == item);

        if(invSlot != null)
            amt = invSlot.Amount;
        
        return amt;
    }

    public List<InventorySlot> Items {get {return itemBag;}}
    
    public void Equip(EquipableItem item)
    {
        if(item is Weapon)
            equipedWeapon = (Weapon)item;
        else if(item is Armor)
        {
            equipedArmor = (Armor)item;
            GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerController>()?.UpdateSprite(item);
        }
    }

    public Item GetEquippedArmor()
    {
        return equipedArmor;
    }
    
    public Weapon EquipedWeapon
    {   get => equipedWeapon;
        set => equipedWeapon = value ?? equipedWeapon;
    }

    public Armor EquipedArmor
    {
        get => equipedArmor;
        set => equipedArmor = value ?? equipedArmor;
    }

}

[System.Serializable]
public class InventorySlot
{
    [SerializeField] Item item;
    [SerializeField] int amount = 1;

    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void ModifyAmount(int value)
    {
        amount += value;
    }

    public Item Item { get {return item;} }
    public int Amount { get {return amount;}}

}
