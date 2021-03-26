using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelController : MonoBehaviour
{   
    [SerializeField] Inventory playerInventory, enemyInventory;
    DuelPlayer player, enemy;
    Card playerMove, enemyMove;

    public Card PlayerMove {get => playerMove; set => playerMove = value;}
    public Card EnemyMove {get => enemyMove; set => enemyMove = value;}

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

    /**
     * One Shot Specials
    */
    void BarbellMedic()
    {
        Debug.Log("Player used Barbell Medic!");
        player.HP = 100;
    }

    void FullThrust()
    {
        Debug.Log("Player used Full Thust!");
        List<Card> newHand = new List<Card>();
        foreach(Card card in player.AttackHand)
            newHand.Add(Card.MakeAttackCard(card.Strength, "Thrust"));
        
        player.ReplaceHand("Attack", newHand);
    }

    void Fahves()
    {
        Debug.Log("Player used FAHVES!");
        List<Card> newPlayerAttackHand = new List<Card>();
        List<Card> newPlayerDefenseHand = new List<Card>();
        List<Card> newEnemyAttackHand = new List<Card>();
        List<Card> newEnemyDefenseHand = new List<Card>();

        // build new hands of the same cards but strength 5
        for(int i = 0; i < 6; i++)
        {
            newPlayerAttackHand.Add(Card.MakeAttackCard(5,player.AttackHand[i].LongType));
            newPlayerDefenseHand.Add(Card.MakeDefenseCard(5,player.DefenseHand[i].LongType));

            newEnemyAttackHand.Add(Card.MakeAttackCard(5,enemy.AttackHand[i].LongType));
            newEnemyDefenseHand.Add(Card.MakeDefenseCard(5,enemy.DefenseHand[i].LongType));
        }
            
        // Replace the hands
        player.ReplaceHand("Attack", newPlayerAttackHand);
        player.ReplaceHand("Defense", newPlayerDefenseHand);

        enemy.ReplaceHand("Attack", newEnemyAttackHand);
        enemy.ReplaceHand("Defense", newEnemyDefenseHand);
    }

    void MangTomas()
    {
        Debug.Log("Player used M?ng Tom?s!");
        int temp = player.HP;
        player.HP = enemy.HP;
        enemy.HP = temp;
    }

    void Chocododge()
    {
        Debug.Log("Player used Chocododge!");
        List<Card> newEnemyAttackHand = new List<Card>();
        List<Card> newEnemyDefenseHand = new List<Card>();

        for(int i = 0; i < 6; i++)
        {
            newEnemyAttackHand.Add(Card.MakeAttackCard(enemy.AttackHand[i].Strength, "Strike"));
            newEnemyDefenseHand.Add(Card.MakeDefenseCard(enemy.DefenseHand[i].Strength, "Dodge"));
        }

        enemy.ReplaceHand("Attack", newEnemyAttackHand);
        enemy.ReplaceHand("Defense", newEnemyDefenseHand);
    }

    void ChineseRedVest()
    {
        Debug.Log("Player Used Chinese Red Vest!");
        List<Card> newHand = new List<Card>();
        foreach(Card card in player.DefenseHand)
            newHand.Add(Card.MakeDefenseCard(9, card.LongType));
        
        player.ReplaceHand("Defense", newHand);
    }

    void SORD()
    {
        Debug.Log("Player Used SORD!");
        List<Card> newHand = new List<Card>();
        foreach(Card card in player.AttackHand)
            newHand.Add(Card.MakeAttackCard(9, card.LongType));
        
        player.ReplaceHand("Attack", newHand);
    }

    void NO_theHolidaySpecial()
    {
        Debug.Log("Player Uesed NO!");
        player.HP -= 30;
    }

    /**
     * Effect Over Time Specials:
    */
    void ParryReposte()
    {   
        Debug.Log("Player Used Parry Reposte");
        // Set this boolean?
    }

    void UlamEquilibrium()
    {
        Debug.Log("Player Used Ulam Equilibrium");
        player.HP += 20;

        // Set recurring boolean? 

    }

    void HowCanYouSee()
    {
        Debug.Log("Player Used How Can You See??");
        // Set boolean
    }

    public void UseSpecial(Special special)
    {
        switch(special.name)
        {
            case "Barbell Medic": BarbellMedic(); break;
            case "Full Thrust": FullThrust(); break;
            case "Parry Reposte": ParryReposte(); break;
            case "FAHVES": Fahves(); break;
            case "Ulam Equilibrium": UlamEquilibrium(); break;
            case "M?ng Tom?s": MangTomas(); break;
            case "Chocododge": Chocododge(); break;
            case "How Can You See??":HowCanYouSee(); break;
            case "Chinese Red Vest": ChineseRedVest(); break;
            case "SORD": SORD(); break;
            case "NO": NO_theHolidaySpecial(); break;
        }
    }

    public void FakeTurn(Card played)
    {
        playerMove = played;
        // Is the enemy Attacking or Defending? 
        string enemyHand = (played.CardClass == "Attack")? "Defense": "Attack";
        
        // Enemy plays a random card from their hand
        Card enemyPlayed = enemy.PlayCard(enemyHand, Random.Range(0, 6));
        enemyMove = enemyPlayed;

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
