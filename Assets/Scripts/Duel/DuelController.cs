using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelController : MonoBehaviour
{   
    [SerializeField] Inventory playerInventory, enemyInventory;
    DuelPlayer player, enemy;

    public DuelPlayer Player {get => player;}
    public DuelPlayer Enemy {get => enemy;}

    private void Awake() {
        player = new DuelPlayer("Pkmn Trainer Red", 
            new DuelWeapon(playerInventory.EquipedWeapon), 
            new DuelArmor(playerInventory.EquipedArmor),
            Equipment.InventoryToEquipment(playerInventory));
        
        enemy = new DuelPlayer("Pkmn Trainer Blue", 
            new DuelWeapon(enemyInventory.EquipedWeapon), 
            new DuelArmor(enemyInventory.EquipedArmor),
            Equipment.InventoryToEquipment(enemyInventory));
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    public void FakeTurn(Card played)
    {

        // Is the enemy Attacking or Defending? 
        string enemyHand = (played.CardClass == "Attack")? "Defense": "Attack";
        
        // Enemy plays a random card from their hand
        Card enemyPlayed = enemy.PlayCard(enemyHand, Random.Range(0, 6));
        enemy.DrawCard(enemyHand);
        Debug.Log($"<color=orange><color=red>{player.Name}</color> Played {played.Name} <color=aqua>{enemy.Name}</color> Played {enemyPlayed.Name} </color>");
        
        int changeToHP = 0;
        
        string attacker = (played.CardClass == "Attack")? played.Type : enemyPlayed.Type;
        string defender = (played.CardClass == "Defense")? played.Type : enemyPlayed.Type;
        
        switch(attacker, defender)
        {
            case ("S", "P"):
            case ("T", "B"):
            case ("L", "D"):
                // CRITICAL DAMAGE BRO
                Debug.Log("<color=yellow>CRITICAL HIT</color>");
                changeToHP = Mathf.Abs(played.Strength - enemyPlayed.Strength) * 5;
                break;
            case ("S", "D"):
            case ("T", "P"):
            case ("L", "B"):
                // Counter - no change to HP
                Debug.Log("<color=yellow>FULL COUNTER</color>");
                break;
            default:
                // PHHHHSSSSSS casual
                Debug.Log("<color=yellow>CONTACT!</color>");
                changeToHP = Mathf.Abs(played.Strength - enemyPlayed.Strength)*2;
                break;
        }

        // Update the player HP. UI Only looks at Player HP and computes enemy value
        Debug.Log($"<color=orange>{player.Name}'s stress has changed by </color><color=green><b>{changeToHP}</b></color>");
        player.HP += (played.CardClass == "Attack")? changeToHP : -1 * changeToHP;
    }
}
