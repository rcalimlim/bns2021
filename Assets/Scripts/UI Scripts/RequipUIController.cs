using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RequipUIController : MonoBehaviour, IPointerExitHandler
{
    List<Item> items;

    EquipableItem equipedWeapon;
    EquipableItem equipedArmor;
    public DuelUIController duelUI;

    public EquipableItem EquipedWeapon {
        get => equipedWeapon;
        set { 
            Button[] buttons = GetComponentsInChildren<Button>();
            foreach(Button button in buttons)
            {
               button.interactable = (button.name != value.name && button.name != equipedArmor.name);
            }
            equipedWeapon = value;
            }
        }
    public EquipableItem EquipedArmor {
        get => equipedArmor;
        set { 
            Button[] buttons = GetComponentsInChildren<Button>();
            foreach(Button button in buttons)
            {
               button.interactable = (button.name != value.name && button.name != equipedWeapon.name);
            }
            equipedArmor = value;
        }
    }

    public List<Item> Items {get => items; 
        set {
            items = new List<Item>();
            foreach(Item i in value)
                items.Add(i);
            
            ItemUiController[] toDelete = GetComponentsInChildren<ItemUiController>();
            
            foreach(ItemUiController iuc in toDelete)
                Destroy(iuc);

            foreach (Item item in items)
            {
                var obj = Instantiate(itemUIPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(transform);
                obj.name = item.name;
                obj.transform.localScale = Vector3.one;
                obj.transform.position = Vector3.zero;
                obj.Item = item;

                Button button = obj.GetComponent<Button>();
                button.interactable = !(item == equipedArmor || item == equipedWeapon);
                ColorBlock colors = button.colors;

                colors.highlightedColor = new Color(1.0f, 0.9f ,0.07f, 1f);
                colors.pressedColor = new Color(1f, 0.7f,0.07f,1f);
                button.colors = colors;

                button.onClick.AddListener(() => {
                    Debug.Log($"<color=yellow>Requip: {item.name}</color>");
                    duelUI.Requip(item);
                });              
            }
        }
    }

    [SerializeField] ItemUiController itemUIPrefab;

    private void Awake() {
        items = new List<Item>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.zero;
    }

    public void ShowRequipMenu()
    {
        transform.localScale = Vector3.one;
    }
}
