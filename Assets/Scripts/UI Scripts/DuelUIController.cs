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
    [SerializeField] Text playerMoveName;
    [SerializeField] CardUIController playerMoveCard;
    [SerializeField] List<CardUIController> attackHandUI;
    [SerializeField] List<CardUIController> defenseHandUI;
    [SerializeField] GameObject attackCardPannel;
    [SerializeField] GameObject defenseCardPannel;

    [SerializeField] Text enemyName;
    [SerializeField] ItemUiController enemyWeapon;
    [SerializeField] ItemUiController enemyArmor;
    [SerializeField] Text enemyMoveName;
    [SerializeField] CardUIController enemyMoveCard;
    

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
        UpdateMoveCards();
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

    void UpdateMoveCards()
    {
        if(duel.PlayerMove != null)
        {
            playerMoveName.text = duel.PlayerMove.LongName;
            playerMoveCard.transform.localScale = Vector3.one;
            playerMoveCard.strength = duel.PlayerMove.Strength;
            playerMoveCard.type = duel.PlayerMove.Type;
        } 
        else 
        {
            playerMoveName.text = "";
            playerMoveCard.transform.localScale = Vector3.zero;
        }

        if(duel.EnemyMove != null)
        {   
            enemyMoveName.text = duel.EnemyMove.LongName;

            enemyMoveCard.transform.localScale = Vector3.one;
            enemyMoveCard.strength = duel.EnemyMove.Strength;
            enemyMoveCard.type = duel.EnemyMove.Type;
        } 
        else 
        {
            enemyMoveName.text = "";
            enemyMoveCard.transform.localScale = Vector3.zero;
        }
    }

    void RegisterButtonListeners(List<CardUIController> uiCards)
    {
        foreach (CardUIController uiCard in uiCards)
        {
            // THIS IS TEST CODE


            // AddListen accepts a UnityAction
            uiCard.button.onClick.AddListener(() => {
                //print(uiCard.name); 
                // ^-- This will hold the object but Trial/Error showed 
                // this will not hold primitives. i.e. for int loop would produce last int only
                
                
                // Use the actual GameObject name to determine which card was clicked
                string[] cardparse = uiCard.name.Split(' ');
                
                // Play current card - Test Code!!!!
                Card played = duel.Player.PlayCard(cardparse[0], int.Parse(cardparse[1]));

                // Play a fake turn
                duel.FakeTurn(played);
                //StartCoroutine(waitABit(3.5f));

                // TEST CODE
                SwapHands(cardparse[0]);              
                // TEST CODE
                duel.Player.DrawCard(cardparse[0]);
            });
        }
    }

    public void SwapHands(string toCardClass)
    {
        attackCardPannel.transform.localScale = 
            (toCardClass == "Attack")? Vector3.zero : Vector3.one;

        defenseCardPannel.transform.localScale = 
            (toCardClass == "Defense")? Vector3.zero : Vector3.one;
    }

    IEnumerator waitABit(float seconds)
    {
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(seconds);

        duel.PlayerMove = null;
        duel.EnemyMove = null;
        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
