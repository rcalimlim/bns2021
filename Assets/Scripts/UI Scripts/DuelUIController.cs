using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuelUIController : MonoBehaviour
{
    public DuelController duel;
    public StressUIController stressUI;

    [SerializeField] float moveWaitTime = 2f;

    [SerializeField] Image duelBackground;

    /*
     * Player Related Fields
    */
    [SerializeField] Text playerName;
    [SerializeField] Image playerPortrait;
    [SerializeField] ItemUiController playerWeapon;
    [SerializeField] ItemUiController playerArmor;
    [SerializeField] Text playerMoveName;
    [SerializeField] CardUIController playerMoveCard;
    [SerializeField] List<CardUIController> attackHandUI;
    [SerializeField] List<CardUIController> defenseHandUI;
    [SerializeField] GameObject attackCardPannel;
    [SerializeField] GameObject defenseCardPannel;
    [SerializeField] GameObject equipmentPannelBlocker;

    /*
     * Enemy Related Fields
    */
    [SerializeField] Text enemyName;
    [SerializeField] Image enemyPortrait;
    [SerializeField] ItemUiController enemyWeapon;
    [SerializeField] ItemUiController enemyArmor;
    [SerializeField] Text enemyMoveName;
    [SerializeField] CardUIController enemyMoveCard;


    /*
     * Special
    */
    bool canClickSpecial = true;
    [SerializeField] GameObject equipmentSpecials;
    [SerializeField] GameObject specialBlocker;
    [SerializeField] Button requipButton;
    [SerializeField] RequipUIController requipPannel;
    List<Special> specials;

    

    // Start is called before the first frame update
    void Start()
    {
        playerName.text = duel.Player.Name;
        playerWeapon.Item = duel.Player.Weapon.OverworldEquipment;
        playerArmor.Item = duel.Player.Armor.OverworldEquipment;
        Armor a = (Armor) playerArmor.Item;
        playerPortrait.overrideSprite = a.Portrait;

        playerMoveName.text = "";
        enemyMoveName.text = "";
        
        enemyName.text = duel.Enemy.Name;
        enemyWeapon.Item = duel.Enemy.Weapon.OverworldEquipment;
        enemyArmor.Item = duel.Enemy.Armor.OverworldEquipment;
        enemyPortrait.overrideSprite = duel.DuelInfo.Portrait;

        duelBackground.overrideSprite = duel.DuelInfo.DuelTopBg;


        RegisterButtonListeners(attackHandUI);
        RegisterButtonListeners(defenseHandUI);
        createSpecialButtons();

        requipPannel.duelUI = this;
        requipPannel.EquipedArmor = (EquipableItem)playerArmor.Item;
        requipPannel.EquipedWeapon = (EquipableItem)playerWeapon.Item;
        requipPannel.Items = duel.Player.InventoryToEquipItem();

    }

    void createSpecialButtons()
    {
        requipButton.onClick.AddListener(() => {
            Debug.Log($"<color=yellow>Requip BITCHES!</color>");
            requipPannel.ShowRequipMenu();
        });

        UpdateSpecials();
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateStressBar(duel.Player.HP);
        PopulateCardUIs(duel.Player.AttackHand, attackHandUI, true);
        PopulateCardUIs(duel.Player.DefenseHand, defenseHandUI, false);
        UpdateEquipedWeapons();

    }

    void UpdateEquipedWeapons()
    {
        if(requipPannel.EquipedWeapon != (EquipableItem)playerWeapon.Item)
            requipPannel.EquipedWeapon = (EquipableItem)playerWeapon.Item;
        
        if(requipPannel.EquipedArmor != (EquipableItem)playerArmor.Item)
            requipPannel.EquipedArmor = (EquipableItem)playerArmor.Item;
    }

    void UpdateSpecials()
    {
        // Update the available Specials
        specials = new List<Special>();
        foreach(Special special in duel.Player.Weapon.specials)
            specials.Add(special);

        foreach(Special special in duel.Player.Armor.specials)
            specials.Add(special);
        
        Button[] specialButtons = equipmentSpecials.GetComponentsInChildren<Button>();
        for(int i = specialButtons.Length - 1; i > -1; i-- )
            Destroy(specialButtons[i].gameObject);

        
        foreach(Special special in specials)
        {
            var obj = Instantiate<Button>(requipButton, Vector3.zero, Quaternion.identity);
            obj.GetComponentInChildren<Text>().text = special.name;
            obj.transform.SetParent(equipmentSpecials.transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.position = Vector3.zero;
            obj.interactable = (!duel.Player.specialsUsed.Contains(special.name));

            obj.onClick.AddListener( () => {
                Debug.Log($"<color=yellow>{special.name} {special.description}</color>");
                special.expended = true;
                obj.interactable = false;
                duel.Player.specialsUsed.Add(special.name);
                specialBlocker.transform.localScale = Vector3.one;

                // Apply the effects of the special
                duel.playerChoosesSpecial(special);
                if(duel.enemyChoiceVisible)
                    enemyMoveCard.color = duel.EnemyMove.CardClass == "Attack" ? 
                        CardUIController.Red : CardUIController.Blue;
            });
        }
    }

    void PopulateCardUIs(List<Card> hand, List<CardUIController> handUI, bool atk )
    {
        for(int i = 0; i < handUI.Count; i++)
        {
            if(i < hand.Count)
            {
                handUI[i].gameObject.SetActive(true);
                handUI[i].strength = hand[i].Strength;
                handUI[i].type = hand[i].Type;
                handUI[i].color = (atk)? CardUIController.Red: CardUIController.Blue;
            }
            else
            {
                handUI[i].gameObject.SetActive(false);
                handUI[i].strength = 0;
                handUI[i].type = "";
                handUI[i].color = CardUIController.Black;
            }
            
        }
    }

    void UpdateStressBar(int hp)
    {
        stressUI.HP = hp;
    }

    void HideMoveCards()
    {
        equipmentPannelBlocker.transform.localScale = Vector3.zero;
        
        playerMoveCard.color = CardUIController.Black;
        playerMoveName.text = "";
        
        enemyMoveCard.color = CardUIController.Black;
        enemyMoveName.text = "";

        if(duel.enemyChoiceVisible)
            enemyMoveCard.color = duel.EnemyMove.CardClass == "Attack" ? 
                CardUIController.Red : CardUIController.Blue;
        
    }
    void ShowMoveCards(Card played)
    {
        equipmentPannelBlocker.transform.localScale = Vector3.one;

        if(played != null)
        {
            // Add weapon specific names
            playerMoveName.text = played.LongName;
            playerMoveCard.strength = played.Strength;
            playerMoveCard.type = played.Type;

            playerMoveCard.color = played.CardClass == "Attack" ? 
                CardUIController.Red : CardUIController.Blue;
        } 

        if(duel.EnemyMove != null)
        {   
            enemyMoveName.text = duel.EnemyMove.LongName;
            enemyMoveCard.strength = duel.EnemyMove.Strength;
            enemyMoveCard.type = duel.EnemyMove.Type;
            enemyMoveCard.color = duel.EnemyMove.CardClass == "Attack" ? 
                CardUIController.Red : CardUIController.Blue;
        } 
    }

    void RegisterButtonListeners(List<CardUIController> uiCards)
    {
        foreach (CardUIController uiCard in uiCards)
        {
            // THIS IS TEST CODE


            // AddListen accepts a UnityAction
            uiCard.button.onClick.AddListener(() => {
                
                // Use the actual GameObject name to determine which card was clicked
                string[] cardparse = uiCard.name.Split(' ');
                
                // Play current card - Test Code!!!!
                Card played = duel.Player.PlayCard(cardparse[0], int.Parse(cardparse[1]));
                
                ShowMoveCards(played);
                duel.playerChoosesCard(played);
                
                StartCoroutine(waitABit(moveWaitTime));
                SwapHands(cardparse[0]);     
            });
        }
    }

    public void SwapHands(string hideCardClass)
    {
        attackCardPannel.transform.localScale = 
            (hideCardClass == "Attack")? Vector3.zero : Vector3.one;

        defenseCardPannel.transform.localScale = 
            (hideCardClass == "Defense")? Vector3.zero : Vector3.one;
        
        // If we are leaving Defense, reactivate the Specials for selection!
        if(hideCardClass == "Defense")
            specialBlocker.transform.localScale = Vector3.zero;
    }

    IEnumerator waitABit(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        HideMoveCards();
    }

    public void Requip(Item item)
    {
        Debug.Log("Requipping???");
        // Open the Requip Menu. Allow new gear selection.

        specialBlocker.transform.localScale = Vector3.one;
        requipPannel.transform.localScale = Vector3.zero;
        List<Equipment> inventory = duel.Player.InventoryToEquipment();
        foreach (Equipment gear in inventory) 
        {
            if(gear.name == item.name)
            {
                duel.Player.Equip(gear);
                if(item is Weapon)
                    playerWeapon.Item = duel.Player.Weapon.OverworldEquipment;
                else {
                    playerArmor.Item = duel.Player.Armor.OverworldEquipment;
                    Armor a = (Armor) playerArmor.Item;
                    playerPortrait.overrideSprite = a.Portrait;
                }    
                
                UpdateSpecials();
                return;
            }
                
        }
    }
}
