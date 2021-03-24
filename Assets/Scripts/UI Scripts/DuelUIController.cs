using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuelUIController : MonoBehaviour
{
    public DuelController duel;
    public StressUIController stressUI;
    
    [SerializeField] Text playerName;
    [SerializeField] ItemUiController playerWeapon;
    [SerializeField] ItemUiController playerArmor;
    [SerializeField] List<CardUIController> attackHandUI;
    [SerializeField] List<CardUIController> defenseHandUI;
    [SerializeField] GameObject attackCardPannel;
    [SerializeField] GameObject defenseCardPannel;

    [SerializeField] Text enemyName;
    [SerializeField] ItemUiController enemyWeapon;
    [SerializeField] ItemUiController enemyArmor;

    // Start is called before the first frame update
    void Start()
    {
        playerName.text = duel.Player.Name;
        playerWeapon.Item = duel.Player.Weapon.OverworldEquipment;
        playerArmor.Item = duel.Player.Armor.OverworldEquipment;

        enemyName.text = duel.Enemy.Name;
        enemyWeapon.Item = duel.Enemy.Weapon.OverworldEquipment;
        enemyArmor.Item = duel.Enemy.Armor.OverworldEquipment;

        RegisterButtonListeners(attackHandUI);
        RegisterButtonListeners(defenseHandUI);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateStressBar(duel.Player.HP);
        PopulateCardUIs(duel.Player.AttackHand, attackHandUI);
        PopulateCardUIs(duel.Player.DefenseHand, defenseHandUI);
    }

    void PopulateCardUIs(List<Card> hand, List<CardUIController> handUI)
    {
        for(int i = 0; i < handUI.Count; i++)
        {
            if(i < hand.Count)
            {
                handUI[i].gameObject.SetActive(true);
                handUI[i].strength = hand[i].Strength;
                handUI[i].type = hand[i].Type;
            }
            else
            {
                handUI[i].gameObject.SetActive(false);
                handUI[i].strength = 0;
                handUI[i].type = "";
            }
            
        }
    }

    void UpdateStressBar(int hp)
    {
        stressUI.HP = hp;
    }

    void RegisterButtonListeners(List<CardUIController> uiCards)
    {
        foreach (CardUIController uiCard in uiCards)
        {
            // UnityAction
            uiCard.button.onClick.AddListener(() => {
                print(uiCard.name);
                string[] cardparse = uiCard.name.Split(' ');
                
                // Play current card
                Card played = duel.Player.PlayCard(cardparse[0], int.Parse(cardparse[1]));
                print(played.AllCardInfo());
                
                // Hide this hand
                attackCardPannel.transform.localScale = 
                    (cardparse[0] == "Attack")? Vector3.zero : Vector3.one;
                    
                defenseCardPannel.transform.localScale = 
                    (cardparse[0] == "Defense")? Vector3.zero : Vector3.one;


                duel.Player.DrawCard(cardparse[0]);
            });
        }
    }
}
