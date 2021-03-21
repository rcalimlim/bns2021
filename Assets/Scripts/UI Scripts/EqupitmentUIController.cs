using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EqupitmentUIController : MonoBehaviour
{   
    [SerializeField]
    Inventory inventory;

    [SerializeField]
    GameObject weapon;
    ItemUiController weaponUi;

    [SerializeField]
    GameObject armor;
    ItemUiController armorUi;

    void Start()
    {
        weaponUi = weapon.GetComponent<ItemUiController>();
        armorUi = armor.GetComponent<ItemUiController>();
    }

    // Update is called once per frame
    void Update()
    {
            weaponUi.Item = inventory.EquipedWeapon;
            armorUi.Item = inventory.EquipedArmor;
    }
}
