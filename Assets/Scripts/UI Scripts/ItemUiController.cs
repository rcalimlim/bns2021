using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUiController : MonoBehaviour
{
    // Reference to all the components of the ItemUI
    Text label;
    Text rating;
    Text type;
    Text description;
    Text special;
    Text quantity;
    Image image;

    [SerializeField]
    Sprite defaultSprite;

    InventorySlot inventorySlot;
    
    [SerializeField]
    Item item;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Text text in GetComponentsInChildren<Text>())
        {
            switch(text.name)
            {
                case "Label":
                    label = text; break;
                case "Rating":
                    rating = text; break;
                case "Type":
                    type = text; break;
                case "Description":
                    description = text; break;
                case "Specials":
                    special = text; break;
                case "Quantity":
                    quantity = text; break;
            }
        }
        
        Image[] images = GetComponentsInChildren<Image>();
        image = images[images.Length-1];
            
    }

    // Update is called once per frame
    void Update()
    {   
        if(item != null)
        {
            if(label != null)
                label.text = item.name;
            
            if(description != null)
                description.text = item.Description;
            
            if(item.Sprite != null)
                image.sprite = item.Sprite;
            else
                image.sprite = defaultSprite;
                        
            if(item is EquipableItem)
            {
                EquipableItem equipItem = (EquipableItem)item;
                if(quantity != null)
                    quantity.gameObject.SetActive(false);

                if(type != null){
                    type.gameObject.SetActive(true);
                    type.text = $"Type: {equipItem.Type}";
                }
                    
                if(rating != null)
                {
                    rating.gameObject.SetActive(true);
                    rating.text = $"Rating: {equipItem.Rating}";
                }
            }
            else
            {
                if(quantity != null && inventorySlot != null)
                {
                    quantity.text = $"Quantity: {inventorySlot.Amount}";
                    quantity.gameObject.SetActive(true);
                }

                if(type != null)
                    type.gameObject.SetActive(false);
                
                if(rating != null)
                    rating.gameObject.SetActive(false);
            }
        }
        else 
        { // This is a just in case scenario
            resetUI();
        }
        
        
    }

    public Item Item
    {   get {return item;}
        set 
        {
            if(inventorySlot == null)
                item = value;
        }
    }

    public InventorySlot InventorySlot
    {
        get => inventorySlot;
        set {
            inventorySlot = value;
            if(inventorySlot != null)
                item = inventorySlot.Item;
        }
    }

    public void resetUI()
    {
        if(label != null)
                label.text = "";
            
        if(description != null)
            description.text = "";
        
        if(type != null)
            type.text = "";
        
        if(rating != null)
            rating.text = "";

        if(image != null)
            image.sprite = defaultSprite;

        if(quantity != null)
            quantity.text = "";
    }
}
