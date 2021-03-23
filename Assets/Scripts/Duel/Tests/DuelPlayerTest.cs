using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelPlayerTest : MonoBehaviour
{
    public bool RunTest = false;
    public Weapon testWeapon;
    public Armor testArmor;
    public Inventory testInventory;

    // Start is called before the first frame update
    void Start()
    {
        if(RunTest)
        {

            DuelPlayer player = new DuelPlayer("Test Player", ConvertWeapon(testWeapon),
                 ConvertArmor(testArmor), ConvertInventory(testInventory));
            
            print(player);

            print($"Play second Attack card is Success: {player.PlayCard("Attack", 1)}");
            print(DuelPlayer.HandToString(player.AttackHand));

            print($"Draw Attack card is Sucess: {player.DrawCard("Attack")}");
            print(DuelPlayer.HandToString(player.AttackHand));
            

            print($"Play fifth Defense card is Success: {player.PlayCard("Defense", 4)}");
            print(DuelPlayer.HandToString(player.DefenseHand));
            print($"Draw Defense card is Sucess: {player.DrawCard("Attack")}");
            print(DuelPlayer.HandToString(player.DefenseHand));
            
        }
        
    }

    DuelWeapon ConvertWeapon(Weapon gear)
    {
        return new DuelWeapon(gear.name, (WeaponType)System.Enum.Parse(typeof(WeaponType), gear.Type),
             (EquitmentRaiting)System.Enum.Parse(typeof(EquitmentRaiting), (gear.Rating == "✩")? "S":gear.Rating),
             gear.Description, null);
    }

    DuelArmor ConvertArmor(Armor gear)
    {
        return new DuelArmor(gear.name, (ArmorType)System.Enum.Parse(typeof(ArmorType), gear.Type),
             (EquitmentRaiting)System.Enum.Parse(typeof(EquitmentRaiting), (gear.Rating == "✩")? "S":gear.Rating),
             gear.Description, null);
    }

    List<Equipment> ConvertInventory(Inventory inventory)
    {
        List<Equipment> equipList = new List<Equipment>();

        foreach(InventorySlot inventorySlot in inventory.Items)
        {
            Item item = inventorySlot.Item;
            if(item is Weapon)
                equipList.Add(ConvertWeapon((Weapon)item));
            else if(item is Armor)
                equipList.Add(ConvertArmor((Armor) item));
        }
        return equipList;
    }


}
