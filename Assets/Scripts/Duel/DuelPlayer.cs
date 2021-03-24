using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelPlayer 
{   
    string name;
    int hp;
    Deck deck;
    List<Card> attackHand;
    List<Card> defenseHand;
    DuelWeapon equipedWeapon;
    DuelArmor equipedArmor;
    Dictionary<string, List<Equipment>> inventory;

    /*
     * Getters
    */
    public string Name {get => name;}
    public int HP {
        get => hp;
        set => hp = Mathf.Clamp(hp + value, 0, 200);
    }       
    public DuelWeapon Weapon {get => equipedWeapon;}
    public DuelArmor Armor {get => equipedArmor;}
    
    public List<Card> AttackHand { get => attackHand;}
    public List<Card> DefenseHand {get => defenseHand;}


    public DuelPlayer(string _name, DuelWeapon _weapon, DuelArmor _armor, List<Equipment> gearList)
    {
        name = _name;
        hp = 100;
        equipedWeapon = _weapon;
        equipedArmor = _armor;

        attackHand = new List<Card>();
        defenseHand = new List<Card>();

        deck = new Deck();
        for(int i=0;i < 6; i++)
        {
            attackHand.Add(deck.DrawAttackCard());
            defenseHand.Add(deck.DrawDefenseCard());
        }

        CreateInventory(gearList);        
    }

    public override string ToString()
    {   
        string str = $"Player: {name}\nHP: {hp}\nWeapon: {equipedWeapon.name}\nArmor: {equipedArmor.name}\n"; // DuelWeapon and DuelArmor use lowercase .name field
        str += $"Attack Hand: {HandToString(attackHand)}\nDefense Hand: {HandToString(defenseHand)}\n";
        str += $"Inventory:\n{InventoryToString(inventory)}";

        return str;
    }

    public static string HandToString(List<Card> cards)
    {
        string outputStr = "";
        foreach(Card card in cards)
            outputStr += $"{card.Name} ";
        return outputStr;
    }

    public static string InventoryToString(Dictionary<string, List<Equipment>> inventory)
    {
        string outputStr = "Weapons: ";
        foreach(Equipment gear in inventory["weapons"])
            outputStr += $"{gear.name},"; // Equipment.name attribute
        
        outputStr += "\nArmors: ";
        foreach(Equipment gear in inventory["armors"])
            outputStr += $"{gear.name},"; // Equipment.name attribute

        return outputStr;
    }

    void CreateInventory(List<Equipment> gearList)
    {
        inventory = new Dictionary<string, List<Equipment>>();
        List<Equipment> armorList = new List<Equipment>();
        List<Equipment> weaponList = new List<Equipment>();

        foreach(Equipment gear in gearList)
        {
            if(gear is DuelWeapon)
                weaponList.Add(gear);
            else if(gear is DuelArmor)
                armorList.Add(gear);
        }

        inventory["weapons"] = weaponList;
        inventory["armors"] = armorList;

    }

    /*
     * Player Interaction Methods. 
     * These return a bool just for easily checking that they work
     */
    public bool Equip(Equipment gear) {
        string type = (gear is DuelWeapon)? "weapons" : (gear is DuelArmor)? "armors": "";

        if(inventory.ContainsKey(type) && inventory[type].Contains(gear))
        {
            if(type == "weapons")
                equipedWeapon = (DuelWeapon)gear;
            else if(type == "armors")
                equipedArmor = (DuelArmor)gear;
            else /* Shouldnt hit this but who knows?*/
                return false;
            return true;
        }
        return false;
    }

    public bool ReplaceHand(string cardClass, List<Card> newCards)
    {
        if(newCards.Count < 1 || newCards.Count > 6)
            return false;
        
        foreach(Card card in newCards)
            if(card.CardClass != cardClass)
                return false;
        
        if(cardClass == "Attack")
            attackHand = new List<Card>(newCards);
        else /*if(cardClass == "Defense")*/
            defenseHand = new List<Card>(newCards);

        return true;
    }

    public bool DrawCard(string cardClass)
    {
        if(cardClass == "Attack" && attackHand.Count < 6 && deck.AttackCardsRemaining > 0)
        {
            attackHand.Add(deck.DrawAttackCard());
            return true; 
        }
        else if(cardClass == "Defense" && defenseHand.Count < 6 && deck.DefenseCardsRemaining > 0)
        {
            defenseHand.Add(deck.DrawDefenseCard());
            return true;
        }
        return false;
    }

    public Card PlayCard(string cardClass, int index)
    {   
        if(index < 0 || index > 5)
            return null;
        
        Card played;
        if(cardClass == "Attack") 
        {
            played = attackHand[index];
            attackHand.RemoveAt(index);
        }  
        else /*if(cardClass == "Defense")*/
        {
            played = defenseHand[index];
            defenseHand.RemoveAt(index);
        }
        
        return played;
    }
}
