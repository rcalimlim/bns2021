using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Special
{
    [SerializeField]
    public string name;

    [SerializeField]
    public string description;

    [SerializeField]
    public bool expended;

    [SerializeField]
    public int duration;

    [SerializeField]
    public int timing;
    public Sprite sprite;

    public Special(string name, string description, int duration = 0, int timing = 0)
    {
        this.name = name;
        this.description = description;
        this.expended = false;
        this.duration = duration;
        this.timing = timing;
    }
   
    // Specials are always initialized to expended = false
    public Special(Special s)
    {
        this.name = s.name;
        this.description = s.description;
        this.expended = false;
        this.duration = s.duration;
        this.timing = s.timing;
        this.sprite = s.sprite;
    }

}

public class Equipment
{
    public EquitmentRaiting rating;
    public string name;
    public string description;
    public string details;
    public List<Special> specials = new List<Special>();

    protected EquipableItem overworldEquipment;
    public EquipableItem OverworldEquipment
    {
        get => overworldEquipment;
    }
    
    public string Rating
    {
        get => Enum.GetName(typeof(EquitmentRaiting), rating);
    }

    // parse rating enum based on string
    public static EquitmentRaiting strToRating(string ratingString)
    {
        switch(ratingString)
        {
            case "C":
                return EquitmentRaiting.C;
            case "B":
                return EquitmentRaiting.B;
            case "A":
                return EquitmentRaiting.A;
            case "✩":
                return EquitmentRaiting.S;
            default:
                return EquitmentRaiting.C;
        }
    }

    public static List<Equipment> InventoryToEquipment(Inventory inventory)
    {
        List<Equipment> equipList = new List<Equipment>();

        foreach(InventorySlot inventorySlot in inventory.Items)
        {
            Item item = inventorySlot.Item;
            if(item is Weapon)
                equipList.Add(new DuelWeapon((Weapon)item));
            else if(item is Armor)
                equipList.Add(new DuelArmor((Armor) item));
        }
        return equipList;
    }
    
}

public class DuelWeapon : Equipment
{
    public WeaponType weaponType;

    public DuelWeapon(
            string name,
            WeaponType weaponType,
            EquitmentRaiting rating,
            string description,
            List<Special> specials)
    {
        this.name = name;
        this.weaponType = weaponType;
        this.rating = rating;
        this.description = description;
        this.specials = specials;
    }

    public DuelWeapon(DuelWeapon w) 
    {
        this.name = w.name;
        this.weaponType = w.weaponType;
        this.description = w.description;
        this.specials = new List<Special>();
        // copy over specials
        foreach (Special s in w.specials) {
            this.specials.Add(s);
        }
    }

    public DuelWeapon(Weapon a)
    {
        this.name = a.name;
        this.weaponType = (WeaponType)System.Enum.Parse(
                typeof(WeaponType),
                a.Type
                );
        this.rating = Equipment.strToRating(a.Rating);
        // copy over specials
        foreach (Special s in a.Specials) {
            this.specials.Add(s);
        }
        this.overworldEquipment = a;
    }

    public string Type
    {
        get => Enum.GetName(typeof(WeaponType), weaponType);
    }

}

public class DuelArmor : Equipment
{
    public ArmorType armorType;

    public DuelArmor(
            string name,
            ArmorType armorType,
            EquitmentRaiting rating,
            string description,
            List<Special> specials)
    {
        this.name = name;
        this.armorType = armorType;
        this.rating = rating;
        this.description = description;
        this.specials = specials;
    }

    public DuelArmor(DuelArmor a)
    {
        this.name = a.name;
        this.armorType = a.armorType;
        this.description = a.description;
        this.specials = new List<Special>();
        // copy over specials
        foreach (Special s in a.specials) {
            this.specials.Add(s);
        }
    }

    public DuelArmor(Armor a)
    {
        this.name = a.name;
        this.armorType = (ArmorType)System.Enum.Parse(
                typeof(ArmorType),
                a.Type
                );
        this.rating = Equipment.strToRating(a.Rating);
        // copy over specials
        foreach (Special s in a.Specials) {
            this.specials.Add(s);
        }
        this.overworldEquipment = a;
    }

    public string Type
    {
        get => Enum.GetName(typeof(ArmorType), armorType);
    }

}
