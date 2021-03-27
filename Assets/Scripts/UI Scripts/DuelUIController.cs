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
    [SerializeField] Image duelBottomBackground;

    /*
     * Player Related Fields
    */
    [SerializeField] Text playerName;
    [SerializeField] Image playerPortrait;
    [SerializeField] Image playerAvatar;
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
    [SerializeField] Image enemyAvatar;
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
    [SerializeField] UITower towSpecial;
    [SerializeField] Image RedVestImg;
    [SerializeField] Image HowCanYouSeeImg; 
    [SerializeField] Image SORDImg; 
    List<Special> specials; 


    // 
    float waitTime =  0;
    Special chineseRedSpecial;
    Special sordSpecial;
    

    // Start is called before the first frame update
    void Start()
    {
        playerName.text = duel.Player.Name;
        playerWeapon.Item = duel.Player.Weapon.OverworldEquipment;
        playerArmor.Item = duel.Player.Armor.OverworldEquipment;
        Armor a = (Armor) playerArmor.Item;
        playerPortrait.overrideSprite = a.Portrait;
        playerAvatar.overrideSprite = a.Avatar;

        playerMoveName.text = "";
        enemyMoveName.text = "";
        
        enemyName.text = duel.Enemy.Name;
        enemyWeapon.Item = duel.Enemy.Weapon.OverworldEquipment;
        enemyArmor.Item = duel.Enemy.Armor.OverworldEquipment;
        enemyPortrait.overrideSprite = duel.DuelInfo.Portrait;
        enemyAvatar.overrideSprite = duel.DuelInfo.Avatar;

        duelBackground.overrideSprite = duel.DuelInfo.DuelTopBg;
        duelBottomBackground.overrideSprite = duel.DuelInfo.DuelBottomBg;


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
        Chocobro();

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
            specials.Add(new Special(special));

        foreach(Special special in duel.Player.Armor.specials)
            specials.Add(new Special(special));
        
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

                //Display Special Sprite
                
                
                HowCanYouSeeActive();
                    switch(special.name)
                    {
                        case "How Can You See??":   
                            IEnumerator[] hcys_routines = {
                                UITower.ScaleOverTime(HowCanYouSeeImg.transform, 3*Vector3.one, 2f),
                                UITower.ScaleOverTime(HowCanYouSeeImg.transform, Vector3.one, 3f)
                            };
                            StartCoroutine(UITower.ChainCoroutines(hcys_routines));
                            break;
                        case "Chinese Red Vest": 
                            chineseRedSpecial = special;
                            Vector3 orgin = RedVestImg.transform.localPosition;
                            IEnumerator[] crv_routines = {
                                UITower.MoveOverTime(RedVestImg.transform, new Vector3(RedVestImg.transform.localPosition.x - 650f, RedVestImg.transform.localPosition.y, 0f), 3500f),
                                UITower.MoveOverTime(RedVestImg.transform, new Vector3(RedVestImg.transform.localPosition.x - 600f, RedVestImg.transform.localPosition.y, 0f), 30f),
                                UITower.MoveOverTime(RedVestImg.transform, orgin, 400f),
                            };
                            StartCoroutine(UITower.ScaleOverTime(RedVestImg.transform, Vector3.one, 3.5f));
                            StartCoroutine(UITower.ChainCoroutines(crv_routines));
                            break;
                        case "SORD": 
                            sordSpecial = special;
                            StartCoroutine(UITower.ScaleOverTime(SORDImg.transform, Vector3.one, 0.5f));
                            break;
                        default:
                            towSpecial.SetImage(special.sprite);
                            towSpecial.Teleport(new Vector3(0, 0, 0));
                            IEnumerator[] def_routines = {
                                UITower.ScaleOverTime(towSpecial.transform, Vector3.one, 1.5f),
                                UITower.MoveOverTime(towSpecial.transform, new Vector3(0f, 260f, 0f), 600f),
                                UITower.MoveOverTime(towSpecial.transform, new Vector3(500f, 330f, 0f), 600f),
                                UITower.ScaleOverTime(towSpecial.transform, Vector3.zero, 1.5f)
                            };
                            StartCoroutine(UITower.ChainCoroutines(def_routines));
                        break;
                    }

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

    void HowCanYouSeeActive()
    {
        if(duel.enemyChoiceVisible) {
            enemyMoveName.text = duel.DuelInfo.SkillNames[duel.EnemyMove.LongType];
            enemyMoveCard.strength = duel.EnemyMove.Strength;
            enemyMoveCard.type = duel.EnemyMove.Type;
            enemyMoveCard.color = duel.EnemyMove.CardClass == "Attack" ? 
                CardUIController.Red : CardUIController.Blue;
        } else if(Vector3.Distance(HowCanYouSeeImg.transform.localScale, Vector3.zero) > 0)
        {
            StartCoroutine(UITower.ScaleOverTime(HowCanYouSeeImg.transform, Vector3.zero, 2f));
        }    
    }
    void ChineseRedVestActive()
    {
        Debug.Log($"Chinese Red Vest in Specials {specials.Contains(chineseRedSpecial)}, duration is {chineseRedSpecial?.duration} Vector distance {Vector3.Distance(RedVestImg.transform.localScale, Vector3.zero)}");
        if(specials.Contains(chineseRedSpecial) && chineseRedSpecial?.duration < 1 && Vector3.Distance(RedVestImg.transform.localScale, Vector3.zero) > 0)
        {
            StartCoroutine(UITower.ScaleOverTime(RedVestImg.transform, Vector3.zero, 2f));
        }
    }

    void SORDActive()
    {
        if(sordSpecial != null && sordSpecial.duration < 1 && Vector3.Distance(SORDImg.transform.localScale, Vector3.zero) > 0)
        {
            StartCoroutine(UITower.ScaleOverTime(SORDImg.transform, Vector3.zero, 2f));
        }
    }

    void HideMoveCards()
    {
        equipmentPannelBlocker.transform.localScale = Vector3.zero;
        
        playerMoveCard.color = CardUIController.Black;
        playerMoveName.text = "";
        
        enemyMoveCard.color = CardUIController.Black;
        enemyMoveName.text = "";

        HowCanYouSeeActive();
        ChineseRedVestActive();
        SORDActive();
    }

    void ShowMoveCards(Card played)
    {
        equipmentPannelBlocker.transform.localScale = Vector3.one;

        if(played != null)
        {
            // Add weapon specific names
            Weapon weapon = (Weapon)playerWeapon.Item;
            playerMoveName.text = weapon.SkillNames[played.LongType];
            playerMoveCard.strength = played.Strength;
            playerMoveCard.type = played.Type;

            playerMoveCard.color = played.CardClass == "Attack" ? 
                CardUIController.Red : CardUIController.Blue;
        } 

        if(duel.EnemyMove != null)
        {   
            enemyMoveName.text = duel.DuelInfo.SkillNames[duel.EnemyMove.LongType];
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
            // AddListen accepts a UnityAction
            uiCard.button.onClick.AddListener(() => {
                
                // Use the actual GameObject name to determine which card was clicked
                string[] cardparse = uiCard.name.Split(' ');
                
                // Play current card - Test Code!!!!
                Card played = duel.Player.PlayCard(cardparse[0], int.Parse(cardparse[1]));
                
                // 
                if(cardparse[0] == "Attack")
                {
                    
                    Transform parent = playerAvatar.gameObject.transform.parent.transform;
                    Vector3 startPos = parent.localPosition;

                    Transform moveTrans = playerMoveName.transform.parent.transform;
                    Vector3 returnPos = moveTrans.localPosition; 

                    IEnumerator[] playerAtk_routines = {
                        UITower.MoveOverTime(moveTrans, returnPos + new Vector3(-70f,-120f, 0), 1000f),
                        UITower.RotateOverTime(moveTrans, -1f, -120f, 1000f),
                        UITower.MoveOverTime(parent, startPos + new Vector3(-650f, 0,0), 1000f),
                        UITower.MoveOverTime(parent, startPos + new Vector3(-649f, 0,0), 1f),
                        UITower.RotateOverTime(moveTrans, 1f, 70f, 1000f),
                        UITower.RotateOverTime(moveTrans, -1f, -90f, 1000f),
                        UITower.MoveOverTime(parent, startPos, 1000f),
                        UITower.RotateOverTime(moveTrans, 1f, 0f, 1000f),
                        UITower.MoveOverTime(moveTrans, returnPos, 1000f)
                    };
                    StartCoroutine(UITower.ChainCoroutines(playerAtk_routines));
                }
                else 
                {

                    Transform parent = enemyAvatar.gameObject.transform.parent.transform;
                    Vector3 startPos = parent.localPosition;

                    Transform moveTrans = enemyMoveName.transform.parent.transform;
                    Vector3 returnPos = moveTrans.localPosition; 

                    IEnumerator[] enemyAtk_routines = {
                        UITower.MoveOverTime(moveTrans, returnPos + new Vector3(50f,-100f, 0), 1000f),
                        UITower.RotateOverTime(moveTrans, 1f, 70f, 1000f),
                        UITower.MoveOverTime(parent, startPos + new Vector3(650f, 0,0), 1000f),
                        UITower.MoveOverTime(parent, startPos + new Vector3(649f, 0,0), 1f),
                        UITower.RotateOverTime(moveTrans, -1f, -70f, 1000f),
                        UITower.RotateOverTime(moveTrans, 1f, 90f, 1000f),
                        UITower.MoveOverTime(parent, startPos, 1000f),
                        UITower.RotateOverTime(moveTrans, -1f, 0f, 1000f),
                        UITower.MoveOverTime(moveTrans, returnPos, 1000f)
                    };

                    StartCoroutine(UITower.ChainCoroutines(enemyAtk_routines));
                }

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


    // IEnumerator moveAttackAcrossScreen(Vector3 start, Vector3 end)
    // {
    //     yield 
    // }

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
                    playerAvatar.overrideSprite = a.Avatar;
                }    
                
                UpdateSpecials();
                return;
            }
                
        }
    }

    
    void Chocobro()
    {
        if(playerArmor.Item.name == "Waaarkout Clothes" && Time.time > waitTime)
        {   
            Armor armor = (Armor)playerArmor.Item;
            int randomChoco = Random.Range(0,armor.ExtraPortraits.Count);
            int randomChocoAvatar = Random.Range(0,armor.ExtraAvatars.Count);
            
            playerPortrait.overrideSprite = armor.ExtraPortraits[randomChoco];
            playerAvatar.overrideSprite = armor.ExtraAvatars[randomChocoAvatar];

            waitTime = Time.deltaTime * Random.Range(50,500) + Time.time;
        }

    }
}
