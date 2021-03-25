using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplayController : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject itemSlotPrefab;
    [SerializeField] GameObject toolTipCanvas;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if(itemsDisplayed.Count != inventory.Items.Count)
        {
            ClearInventorySlots();
            itemsDisplayed.Clear();
            CreateDisplay();
        }
    }

    private void ClearInventorySlots()
    {
        foreach (InventorySlot key in itemsDisplayed.Keys)
        {
            Destroy(itemsDisplayed[key]);
        }
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            //itemsDisplayed.Add(inventory.Items[i], AddItem(inventory.Items[i], i));
            itemsDisplayed.Add(inventory.Items[i], AddItem(inventory.Items[i]));
        }
    }

    public void UpdateItemList()
    {
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            if(itemsDisplayed.ContainsKey(inventory.Items[i])) 
            {
                itemsDisplayed[inventory.Items[i]].GetComponentsInChildren<Text>()[0].text = inventory.Items[i].Item.name;
                itemsDisplayed[inventory.Items[i]].GetComponentsInChildren<Text>()[1].text = "x " + inventory.Items[i].Amount;
            }
                
            else
                itemsDisplayed.Add(inventory.Items[i], AddItem(inventory.Items[i], i));
        }

    }

    Vector3 GetPosition(int i)
    {
        return new Vector3(0, 0, 0f);
    }

    GameObject AddItem(InventorySlot inventorySlot, int i)
    {
        var obj = Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(this.transform, false);
        obj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i);
        
        var children = obj.GetComponentsInChildren<Text>();
        children[0].text = inventorySlot.Item.name;
        children[1].text = "x " + inventorySlot.Amount;

        return obj;
    }

    GameObject AddItem(InventorySlot inventorySlot)
    {
        var obj = Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity, this.transform);
        UpgradeSlotButtonController usbc = obj.GetComponent<UpgradeSlotButtonController>();
        usbc.InventorySlot = inventorySlot;
        
        TooltipTrigger ttt = obj.GetComponent<TooltipTrigger>();
        ttt.inventorySlot = inventorySlot;
        ttt.TooltipCanvas = toolTipCanvas;

        return obj;
    }
}
