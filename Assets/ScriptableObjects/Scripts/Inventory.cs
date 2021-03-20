using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObjectBase
{
    [SerializeField] List<InventorySlot> itemBag = new List<InventorySlot>();

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

    public List<InventorySlot> Items {get {return itemBag;}}

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
