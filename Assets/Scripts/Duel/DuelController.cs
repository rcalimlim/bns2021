using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DuelID
{
    KeyboardWarrior,
    Stalker,
    ComputerBug
}
public enum DuelStatus
{
    Continue,
    PlayerWin,
    EnemyWin,
    Draw
}

public class DuelController : MonoBehaviour
{
    // REMOVE, for debugging only
    [SerializeField] DuelStatus debugDuelStatus;

    // win/loss transitions
    [SerializeField] Dialog playerWonDialog;
    [SerializeField] Dialog playerLostDialog;


    [SerializeField] Inventory playerInventory, enemyInventory;
    Card playerMove, enemyMove;

    public Card PlayerMove {get => playerMove; set => playerMove = value;}
    public Card EnemyMove {get => enemyMove; set => enemyMove = value;}

    public bool enemyChoiceVisible = false;

    /* game model variables and objects */

    [SerializeField] DuelID duelID;
    [SerializeField]
    DuelInfo[] duelInfos;
    DuelInfo duelInfo;

    DuelPlayer player, enemy;
    public DuelPlayer Player {get => player;}
    public DuelPlayer Enemy {get => enemy;}


    int round = 1;
    int turn = 0; 
    int dmg; // holds last damage
    //List<Special> activeSpecials = new List<Special>();
    Queue<Special> activeSpecials = new Queue<Special>();
    List<Card> playerActionHistory = new List<Card>();
    List<Card> enemyActionHistory = new List<Card>();

    public int RoundNo()
    {
        return round;
    }

    public int TurnNo()
    {
        return turn;
    }

    public DuelInfo DuelInfo {get => duelInfo;}

    /* game state helper methods */

    void SetDuelSpecifics(DuelID thisDuel)
    {
        // Hyjacking native conversion to int from Enum
        // This means that the data in the editor MUST be in the correct order
        duelInfo = duelInfos[(int)thisDuel];


        DuelWeapon duelWeapon = new DuelWeapon(duelInfo.Weapon);
        DuelArmor duelArmor = new DuelArmor(duelInfo.Armor);

        enemy = new DuelPlayer(thisDuel.ToString(), 
            duelWeapon, 
            duelArmor,
            new List<Equipment>{duelWeapon, duelArmor});
    }

    public void initDuel()
    {

        // any special set up we decide we need can go here
        
        SetDuelSpecifics(duelID); // These are actually generic!
        switch (duelID) {
            case DuelID.KeyboardWarrior: 
            case DuelID.Stalker:
                SoundManager.Instance.PlayMusic(duelInfo.BGM[0], true);
                break;
            case DuelID.ComputerBug:
                SoundManager.Instance.ScheduleTwoClips(duelInfo.BGM[0], duelInfo.BGM[1]);
                break;
            default:
                Debug.LogError(
                        "Invalid DuelID - is this set in the inspector? ID: " +
                        duelID.ToString()
                        );
                break;
        }
        
        enemyChoiceVisible = false; // stop seeing for now

        /* Creating the player and enemy creates them with full hands */
        player = new DuelPlayer("Bryce", 
            new DuelWeapon(playerInventory.EquipedWeapon), 
            new DuelArmor(playerInventory.EquipedArmor),
            Equipment.InventoryToEquipment(playerInventory));
        

        

        // no specials yet to trigger
        Debug.LogFormat("Staring Duel {0}", duelID.ToString());
        Debug.LogFormat("Round {0} Turn {1} : {2} to attack / {3} to defend",
                round,turn, player.Name, enemy.Name);
        
    }

    public DuelStatus status()
    {
        if (debugDuelStatus == DuelStatus.EnemyWin)
        {
            return DuelStatus.EnemyWin;
        }
        if (debugDuelStatus == DuelStatus.PlayerWin)
        {
            return DuelStatus.PlayerWin;
        }
        if (Player.HP == 200) return DuelStatus.PlayerWin;
        if (Player.HP == 0) return DuelStatus.EnemyWin;
        if (round >= 10) return DuelStatus.Draw;
        return DuelStatus.Continue;
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
        enemyMove = cpuChooseCard(enemyMove.CardClass);
    }

    void MangTomas()
    {
        Debug.Log("Player used M?ng Tom?s!");
        int temp = player.HP;
        player.HP = enemy.HP;
        enemy.HP = temp;
    }

    void Chocododge()
    // All enemy attacks become S, all player defenses become D
    {
        Debug.Log("Player used Chocododge!");
        List<Card> newEnemyAttackHand = new List<Card>();
        List<Card> newPlayerDefenseHand = new List<Card>();

        for(int i = 0; i < 6; i++)
        {
            newEnemyAttackHand.Add(Card.MakeAttackCard(enemy.AttackHand[i].Strength, "Strike"));
            newPlayerDefenseHand.Add(Card.MakeDefenseCard(player.DefenseHand[i].Strength, "Dodge"));
        }

        enemy.ReplaceHand("Attack", newEnemyAttackHand);
        player.ReplaceHand("Defense", newPlayerDefenseHand);

        string cardClass = playerMove?.CardClass;
        enemyMove = cpuChooseCard(enemyMove.CardClass);
    }

    void ChineseRedVest()
    // All defenses become 9
    {
        Debug.Log("Player Used Chinese Red Vest!");
        List<Card> newHand = new List<Card>();
        foreach(Card card in player.DefenseHand)
            newHand.Add(Card.MakeDefenseCard(9, card.LongType));
        
        player.ReplaceHand("Defense", newHand);
    }

    void SORD()
    // All attacks become 9
    {
        Debug.Log("Player Used SORD!");
        List<Card> newHand = new List<Card>();
        foreach(Card card in player.AttackHand)
            newHand.Add(Card.MakeAttackCard(9, card.LongType));
        
        player.ReplaceHand("Attack", newHand);
    }

    void NO_theHolidaySpecial()
    // Player takes damage
    {
        Debug.Log("Player Used NO!");
        player.HP -= 30;
    }

    /**
     * Effect Over Time Specials:
    */
    void ParryReposte()
    // While the effect is active, if the enemy's attack is countered,
    // the enemy takes 15 damage
    {   
        Debug.Log("Player Used Parry Reposte");
        if (turn == 1 && dmg == 0) player.HP += 15;
    }

    void UlamEquilibrium()
    // Restore 10 HP per turn for 10 turns (5 rounds) 
    {
        Debug.Log("Player Used Ulam Equilibrium");
        player.HP += 10;
    }

    void HowCanYouSee()
    // CPU selected action will show up in the UI
    {
        Debug.Log("Player Used How Can You See??");
        // Set boolean
        enemyChoiceVisible = true;
    }

    public void useSpecial(Special special)
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
            default:
                Debug.LogError("Invalid Special name: " + special.name);
                break;
        }
    }

    private void runSpecials(bool afterDamage = false)
    /* applies effects in the specials queue, decrements duration of each one
     * that runs
     * */
    {
        // timing 1 means it runs when afterDamage = true
        int timing = (afterDamage) ? 1 : 0;
        
        int count = activeSpecials.Count;
        Queue<Special> stillActive = new Queue<Special>();
        for(int i=0; i < count; i++){
            Special special = activeSpecials.Dequeue();

            if (special.timing == timing) {
                useSpecial(special);
                special.duration--;
            }
            if(special.duration > 0)
                stillActive.Enqueue(special);
        }
        activeSpecials = stillActive;
    }

    /* CPU action selection */

    private int simulateAttack(Card aCard, Card dCard, DuelPlayer attacker, DuelPlayer defender)
    {
        Attack attack = parseAttackString(aCard.Type);
        Defense defense = parseDefenseString(dCard.Type);
        DuelWeapon weapon = attacker.Weapon;
        DuelArmor armor = defender.Armor;

        return Damage.damage(
                attack,
                defense,
                aCard.Strength,
                dCard.Strength,
                weapon.weaponType,
                weapon.rating,
                armor.armorType,
                armor.rating
                );
    }

    const int _CARD_NOT_FOUND = -1;

    private int cardIndex(Card c, DuelPlayer p, string cardType)
    {
        List<Card> h;
        if (cardType == "Attack") {
            h = p.AttackHand;
        } else {
            h = p.DefenseHand;
        }
        for (int i = 0; i < h.Count; i++) {
            if (c.Type == h[i].Type && c.Strength == h[i].Strength) {
                return i;
            }
        }
        Debug.LogError("Could not find card with " +
                c.AllCardInfo() + " " +
                "in player " + p.Name + " " + cardType + " hand");
        return _CARD_NOT_FOUND;
    }

    private Card cpuMaxNeutralAttack()
    /*
     * Gets the best attack that causes the most damage against a corresponding
     * neutral defense with strength 5.
     */
    {
        int best = 0;
        int i = 0;
        int best_i = 0;
        foreach (Card aCard in enemy.AttackHand) {
            Defense d = Damage.neutralDefenseForAttack(parseAttackString(aCard.Type));
            Card dCard =Card.MakeDefenseCard(5, d.ToString());
            int result = simulateAttack(aCard,dCard,enemy,player);
            if (result > best) {
                best = result;
                best_i = i;
            }
            i++;
        }
        return enemy.AttackHand[best_i];
    }

    private Card cpuMinNeutralDefense()
    /*
     * Gets the best defense that receives the least damage against a corresponding
     * neutral attack with strength 5.
     */
    {
        int best = 200;
        int i = 0;
        int best_i = 0;
        foreach (Card dCard in enemy.DefenseHand) {
            Attack a = Damage.neutralAttackForDefense(parseDefenseString(dCard.Type));
            Card aCard = Card.MakeAttackCard(5,a.ToString());
            int result = simulateAttack(aCard,dCard,player,enemy);
            if (result < best) {
                best = result;
                best_i = i;
            }
            i++;
        }
        return enemy.DefenseHand[best_i];
    }

    private Card cpuMaxEffectiveAttack()
    /*
     * Gets the attack that causes the most damage against a corresponding
     * defense defense with strength 5.
     */
    {
        int best = 0;
        int i = 0;
        int best_i = 0;
        foreach (Card aCard in enemy.AttackHand) {
            Defense d = Damage.weakestDefenseForAttack(parseAttackString(aCard.Type));
            Card dCard = Card.MakeDefenseCard(5, d.ToString());
            int result = simulateAttack(aCard,dCard,enemy,player);
            if (result > best) {
                best = result;
                best_i = i;
            }
            i++;
        }
        return enemy.AttackHand[best_i];
    }
    
    private Card cpuMinWeakDefense()
    /*
     * Gets the best defense that receives the least damage against a corresponding
     * neutral attack with strength 5.
     */
    {
        int best = 200;
        int i = 0;
        int best_i = 0;
        foreach (Card dCard in enemy.DefenseHand) {
            Attack a = Damage.effectiveAttackForDefense(parseDefenseString(dCard.Type));
            Card aCard = Card.MakeAttackCard(5,a.ToString());
            int result = simulateAttack(aCard,dCard,player,enemy);
            if (result < best) {
                best = result;
                best_i = i;
            }
            i++;
        }
        return enemy.DefenseHand[best_i];
    }

    private Card cpuImitateAttack()
    {
        if (round > 1) {
            // round 1 at index 0, round 2 at index 2, etc.
            int i = (round - 2) * 2; 
            Card lastPlayerAttack = playerActionHistory[i];
            string aStr = lastPlayerAttack.Type;
            foreach (Card c in enemy.AttackHand) {
                if (c.Type == aStr) {
                    return c;
                }
            }
        }
        return enemy.AttackHand[0];
    }

    private Card cpuImitateDefense()
    {
        if (round > 1) {
            // round 1 at index 0, round 2 at index 2, etc.
            int i = (round - 2) * 2; 
            Card lastPlayerDefense = playerActionHistory[i];
            string dStr = lastPlayerDefense.Type;
            foreach (Card c in enemy.DefenseHand) {
                if (c.Type == dStr) {
                    return c;
                }
            }
        }
        return enemy.DefenseHand[0];
    }

    private Card cpuChooseCard(string cardType)
    {

        if (cardType == "Attack") {
            // select attack
            switch (duelID) {
                case DuelID.KeyboardWarrior:
                    return cpuMaxNeutralAttack();
                case DuelID.Stalker:
                    return cpuImitateAttack();
                case DuelID.ComputerBug:
                    return cpuMaxEffectiveAttack();
                default:
                    Debug.LogError(
                            "Invalid DuelID - is this set in the inspector? ID: " +
                            duelID.ToString()
                            );
                    return cpuMaxNeutralAttack();
            }
        } else {
            // select defense
            switch (duelID) {
                case DuelID.KeyboardWarrior:
                    return cpuMinNeutralDefense();
                case DuelID.Stalker:
                    return cpuImitateDefense();
                case DuelID.ComputerBug:
                    return cpuMinWeakDefense();
                default:
                    Debug.LogError(
                            "Invalid DuelID - is this set in the inspector? ID: " +
                            duelID.ToString()
                            );
                    return cpuMinNeutralDefense();
            }
         }
    }

    private Attack parseAttackString(string cs)
    {//return (Attack) Enum.Parse(typeof(Attack), cs) Where cs is card.LongType
       switch (cs) {
           case "S" :
           case "Strike" :
               return Attack.Strike;
           case "L" :
           case "Lash" :
               return Attack.Lash;
           case "T" :
           case "Thrust" :
               return Attack.Thrust;
           default : {
                         Debug.LogError("Can't parse attack string: " + cs);
                         return Attack.Strike;
                     }
       }
    }

    private Defense parseDefenseString(string cs)
    { //return (Defense) Enum.Parse(typeof(Defense), cs) Where cs is card.LongType
        switch (cs) {
            case "D" :
            case "Dodge" :
                return Defense.Dodge;
            case "B" :
            case "Block" :
                return Defense.Block;
            case "P" :
            case "Parry" :
                return Defense.Parry;
            default : {
                          Debug.LogError("Can't parse attack string: " + cs);
                          return Defense.Dodge;
                      }
        }
    }

    private Card getAttackCard()
    {
        if (playerMove.CardClass == "Attack") {
            return playerMove;
        }
        return enemyMove;
    }

    private Card getDefenseCard()
    {
        if (playerMove.CardClass == "Defense") {
            return playerMove;
        }
        return enemyMove;
    }

    private DuelWeapon getAttackWeapon()
    {
        if (playerMove.CardClass == "Attack") {
            return player.Weapon;
        }
        return enemy.Weapon;
    }

    private DuelArmor getDefenseArmor()
    {
        if (playerMove.CardClass == "Defense") {
            return player.Armor;
        }
        return enemy.Armor;
    }

    private int resolveDamage()
    {
        Card aCard = getAttackCard(); 
        Card dCard = getDefenseCard();
        Attack attack = parseAttackString(aCard.Type);
        Defense defense = parseDefenseString(dCard.Type);
        DuelWeapon weapon = getAttackWeapon();
        DuelArmor armor = getDefenseArmor();

        int result = Damage.damage(
                attack,
                defense,
                aCard.Strength,
                dCard.Strength,
                weapon.weaponType,
                weapon.rating,
                armor.armorType,
                armor.rating
                );
        if (turn == 0) {
            player.HP += result; // player is attacking
            enemy.HP -= result; // enemy is defending
        } else {
            player.HP -= result; // player is defending
            enemy.HP += result; // player is attacking
        }
        return result;
    }

    private void buildHistory()
    {
       playerActionHistory.Add(playerMove);
       enemyActionHistory.Add(enemyMove);
    }

    private void nextTurn()
    {
        buildHistory();
        if (turn == 1) {
            turn = 0;
            round++; // next round
        }
        else
            turn++;
    }

    /* specials code */

    /* Controller Lifecycle and Triggered Actions */

    private void Awake() 
    {

        duelID = PlayerDataManager.Instance.NextDuelID;

        initDuel();
    }

    // Start is called before the first frame update
    void Start()
    {
        // this is needed in order to see
        enemyMove = cpuChooseCard("Defense");
    }

    public void playerChoosesSpecial(Special s)
    /*
     * Player chooses a special.
     */
    {
        // trigger effects if duration is zero
        if ( s.timing == 0) {
            useSpecial(s);
            s.duration--;
        }

        // else add to specials list
        activeSpecials.Enqueue(s);
        //runSpecials(false);
    }

    public void playerChoosesCard(Card c)
    {
        // is player attacking or defending?
        // set player move
        playerMove = c;
        
        
        string enemyCardType = (playerMove.CardClass == "Defense") ? "Attack" : "Defense";
        // This removes the enemy's card from the hand
        enemy.PlayCard(
                enemyCardType,
                cardIndex(enemyMove, enemy, enemyCardType)
                );


        // enemyMove is already chosen
        // resolve damage
        dmg = resolveDamage(); // resolve damage doesn't need any parameters

        // TODO: this is where the update damage UI would happen
        // Here's additional information that might be useful for the UI
        // attack type and weapon
        Card attackC = getAttackCard();
        DuelWeapon attackW = getAttackWeapon();
        Attack attack = parseAttackString(attackC.Type);

        // defense type and armor
        Card defenseC = getDefenseCard();
        DuelArmor defenseA = getDefenseArmor();
        Defense defense = parseDefenseString(defenseC.Type);

        // Some nice info for custom messages
        bool hit = ! Damage.isCounter(attack,defense);
        bool criticalHit = Damage.isEffective(attack,defense);
        bool weaponBonus = hit && Damage.isWeaponSpecialty(attack, attackW.weaponType);
        bool armorBonus = hit && Damage.isArmorSpecialty(defense, defenseA.armorType);
        
        // Examples of how you can use this
        if (hit) {
            Debug.LogFormat("{0}{1} vs. {2}{3} -{4}{5}{6} hit : {7}",
                    attackC.Type, attackC.Strength,
                    defenseC.Type, defenseC.Strength,
                    (criticalHit) ? " +critical" : "",
                    (weaponBonus) ? " +weaponSpec" : "",
                    (armorBonus) ? " -armorSpec" : "",
                    dmg);
        } else {
            Debug.LogFormat($"{attackC.Type}{attackC.Strength} vs. {defenseC.Type}{defenseC.Strength} - miss : {dmg}");
        }
        // TODO: UI update calls here?
        

        // trigger post damage resolution specials
        runSpecials(afterDamage: true);

        // check duel status
        if (status() == DuelStatus.Continue) {
            // next turn
            nextTurn();
            string attackerName = (turn == 0) ? player.Name : enemy.Name;
            string defenderName = (turn == 0) ? enemy.Name : player.Name;
            Debug.LogFormat("Round {0} Turn {1} : {2} to attack / {3} to defend",round,turn, attackerName, defenderName);
            enemyChoiceVisible = false; // stop seeing for now

            // trigger pre attack specials in queue
            runSpecials(afterDamage: false);

            
            // We can assume that the hand we have yet to play is already full
            // thus we only need to Refill the hand of the card type you just played
            player.DrawCard(playerMove.CardClass);
            enemy.DrawCard(enemyCardType);
            // this is needed in order to see
            enemyMove = cpuChooseCard(playerMove.CardClass);         

        } else if (status() == DuelStatus.PlayerWin) {
           
            // get next scene
            string nextScene = PlayerDataManager.Instance.PrevScene;

            // track scene change with bogus door
            PlayerDataManager.Instance.TrackSceneChange(
                PlayerDataManager.Instance.LoadAtPos,
                SceneManager.GetActiveScene().name,
                PlayerDataManager.Instance.PrevScene
            );

            // set battle flag
            switch (duelID)
            {
                case DuelID.KeyboardWarrior:
                    PlayerDataManager.Instance.SetTriggerFlag("TrollFought", true);
                    break;
                case DuelID.Stalker:
                    PlayerDataManager.Instance.SetTriggerFlag("CringeFought", true);
                    break;
                case DuelID.ComputerBug:
                    PlayerDataManager.Instance.SetTriggerFlag("BugFought", true);
                    break;
                default:
                    break;
            }

            // set next duel
            PlayerDataManager.Instance.CompletePlayerBattle(duelID);

            // change scene
            SceneManager.LoadScene(nextScene);
        } else if (status() == DuelStatus.EnemyWin || status() == DuelStatus.Draw) {
            
            StartCoroutine(InvokePlayerLost());
        }
    }

    private IEnumerator InvokePlayerLost()
    {
        yield return new WaitForEndOfFrame();

        yield return StartCoroutine(DialogManager.Instance.ShowDialog(playerLostDialog));


        yield return new WaitForSecondsRealtime(3);
        DialogManager.Instance.CloseDialog();

        PlayerDataManager.Instance.HasDied = true;
        PlayerDataManager.Instance.TrackSceneChange("", SceneManager.GetActiveScene().name, PlayerDataManager.Instance.PrevScene);
        SceneManager.LoadScene("BryceBedroom");
    }
}