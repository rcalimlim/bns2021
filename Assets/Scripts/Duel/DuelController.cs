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
}
