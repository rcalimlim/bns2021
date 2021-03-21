using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    [SerializeField] private Item item;
    [SerializeField] private int qty;

    public Item Item { get { return item; } }
    public int Qty { get { return qty; } }
}
